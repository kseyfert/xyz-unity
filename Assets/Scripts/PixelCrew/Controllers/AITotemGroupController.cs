using System.Collections.Generic;
using System.Linq;
using PixelCrew.Creatures;
using PixelCrew.Creatures.Controllers;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Controllers
{
    public class AITotemGroupController : MonoBehaviour
    {
        [SerializeField] private List<Totem> totems;
        [SerializeField] private float cooldownTime;

        private Cooldown _cooldown = new Cooldown();
        private Creature _target;
        private List<AttackController2> _attackControllers = new List<AttackController2>();
        private List<HealthController> _healthControllers = new List<HealthController>();

        private bool _canThrow;

        private int _activeHead = 0;

        private void Start()
        {
            foreach (var totem in totems)
            {
                _attackControllers.Add(totem.AttackController);
                _healthControllers.Add(totem.HealthController);
            }
        }

        private void Update()
        {
            if (!_canThrow) return;
            if (!_cooldown.IsReady) return;
            if (_healthControllers.TrueForAll(item => item.GetHealthComponent().IsDead()))
            {
                Die();
                return;
            }

            while (true)
            {
                _activeHead %= totems.Count;
                
                var healthController = _healthControllers[_activeHead];
                var attackController = _attackControllers[_activeHead];
                
                _activeHead++;
            
                if (healthController.GetHealthComponent().IsDead()) continue;
                
                attackController.RequestRange();
                _cooldown.Reset(cooldownTime);
                
                break;
            }
        }

        public void OnSpotted(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;

            _target = hero;
            _canThrow = true;
        }

        public void OnMissed(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;
            if (hero != _target) return;

            _target = null;
            _canThrow = false;
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}