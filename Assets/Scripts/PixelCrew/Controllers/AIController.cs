using System.Collections;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Creatures;
using PixelCrew.Creatures.Controllers;
using UnityEngine;

namespace PixelCrew.Controllers
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private Creature creature;
        [SerializeField] private AreaComponent patrollingArea;

        private Transform _transform;
        private MovementController _movementController;
        private AttackController _attackController;

        private Creature _target;
        private bool _canAttack;
        private bool _atBase;

        private Coroutine _currentState;

        private void Start()
        {
            _transform = creature.Transform;
            _movementController = creature.MovementController;
            _attackController = creature.AttackController;

            StartState(Patrolling());
        }

        private Coroutine StartState(IEnumerator state)
        {
            if (_currentState != null) StopCoroutine(_currentState);
            _currentState = StartCoroutine(state);

            return _currentState;
        }

        private IEnumerator Patrolling()
        {
            _movementController.SpeedDown();
            
            var dir = _movementController.GetDirection();
            if (dir == 0) _movementController.SetDirection(1);
            
            yield return null;

            while (true)
            {
                if (_target != null) break;
                
                if (!_atBase) _movementController.SetDirection(patrollingArea.GetCenter().x - _transform.position.x);
                
                yield return new WaitForSeconds(1f);
            }

            StartState(Pursuit());
        }

        private IEnumerator Pursuit()
        {
            _movementController.SpeedUp();
            while (true)
            {
                if (_target == null) break;

                if (_canAttack)
                {
                    _movementController.SetDirection(0);
                    _attackController.Attack();
                }
                else if (_atBase)
                {
                    _movementController.SetDirection(_target.transform.position.x - _transform.position.x);
                }
                else
                {
                    _movementController.SetDirection(0);
                }

                yield return null;
            }

            StartState(Patrolling());
        }

        public void OnLeavingPatrollingArea()
        {
            _atBase = false;
        }

        public void OnEnteringPatrollingArea()
        {
            _atBase = true;
        }

        public void OnSpotted(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;

            _target = hero;
            
            creature.ParticlesController.Spawn("exclamation");
        }

        public void OnMissed(GameObject go)
        {
            var hero = go.GetComponent<Creature>();
            if (hero == null) return;
            if (hero != _target) return;

            _target = null;
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