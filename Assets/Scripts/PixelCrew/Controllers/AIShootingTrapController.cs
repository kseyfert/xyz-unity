using PixelCrew.Creatures;
using PixelCrew.Creatures.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Controllers
{
    public class AIShootingTrapController : MonoBehaviour
    {
        [SerializeField] private Creature creature;
        [SerializeField] private float maxProbability;

        private AttackController _attackController;

        private Creature _target;

        private bool _canThrow;
        private bool _canAttack;

        private void Start()
        {
            _attackController = creature.AttackController;
        }

        private void Update()
        {
            if (_canAttack) _attackController.RequestMelee();
            else if (_canThrow)
            {
                var x = Random.value;
                if (x <= maxProbability) _attackController.RequestRangeMax();
                else _attackController.RequestRange();
            }
        }

        public void OnSpotted(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;

            _target = hero;
            _canThrow = true;
            
            creature.ParticlesController.Spawn("exclamation");
        }

        public void OnMissed(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;
            if (hero != _target) return;

            _target = null;
            _canThrow = false;
        }

        public void OnAttackable(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;
            if (hero != _target) return;

            _canAttack = true;
        }

        public void OnNotAttackable(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;
            if (hero != _target) return;

            _canAttack = false;
        }
    }
}