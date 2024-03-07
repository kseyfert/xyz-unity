using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class CircularSpawnComponent : SpawnComponent
    {
        [SerializeField] private int currentProfile = 0;
        [SerializeField] private List<CircularSpawnProfile> profiles = new List<CircularSpawnProfile>();

        private readonly List<Coroutine> _coroutines = new List<Coroutine>();

        [ContextMenu("SpawnCircular")]
        public override void Spawn()
        {
            if (prefab != null)
            {
                var c = StartCoroutine(Launch(target.position, prefab));
                _coroutines.Add(c);
            }
            action?.Invoke();
        }

        public override void SpawnAt(Vector3 position)
        {
            if (prefab == null) return;

            var c = StartCoroutine(Launch((Vector2)position, prefab));
            _coroutines.Add(c);
        }

        public override void SpawnCustom(GameObject customPrefab)
        {
            if (customPrefab != null)
            {
                var c = StartCoroutine(Launch((Vector2)target.position, customPrefab));
                _coroutines.Add(c);
            }
            action?.Invoke();
        }

        private IEnumerator Launch(Vector2 center, GameObject go)
        {
            var profile = profiles[currentProfile];
            
            var projectile = go.GetComponent<DirectionalProjectile>();
            if (projectile == null) throw new Exception("Prefab has to be directional projectile");

            var startAngle = profile.startAngleDegrees * Mathf.PI / 180;
            
            var sectorStep = profile.turns * 2 * Mathf.PI / profile.count;
            for (var i = 0; i < profile.count; i++)
            {
                var angle = startAngle + sectorStep * i * (profile.clockwise ? -1 : 1);
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                var position = center + direction * profile.radius;

                var spawned = CreateObject(
                    go,
                    position,
                    target.rotation,
                    Vector3.one,
                    parent
                );
                spawned.transform.AdjustScale(target);
                spawned.GetComponent<DirectionalProjectile>().SetDirection(direction);

                yield return new WaitForSeconds(profile.delay);
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var radius = profiles[currentProfile].radius;
            UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(target.position, Vector3.forward, radius);
        }
#endif

        public void Interrupt()
        {
            while (_coroutines.Count > 0)
            {
                var pop = _coroutines[0];
                _coroutines.RemoveAt(0);
                StopCoroutine(pop);
            }
        }

        public void NextProfile()
        {
            currentProfile++;
            if (currentProfile >= profiles.Count) currentProfile = profiles.Count - 1;
        }
    }

    [Serializable]
    public struct CircularSpawnProfile
    {
        [SerializeField] public int count;
        [SerializeField] public float radius;
        [SerializeField] public float delay;
        [SerializeField] public int turns;
        [SerializeField] public float startAngleDegrees;
        [SerializeField] public bool clockwise;
    }
}