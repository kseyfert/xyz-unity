using System;
using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Creatures.Model;
using PixelCrew.Creatures.Model.Data;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Creatures.Controllers
{
    public class AttackController : AController
    {
        public static readonly string RangeParticle = "range-projectile";
        
        public delegate void AttackEvent();

        public AttackEvent onMeleeRequested;
        public AttackEvent onMeleeApplied;

        public AttackEvent onRangeRequested;
        public AttackEvent onRangeApplied;

        public AttackEvent onRangeMaxRequested;
        public AttackEvent onRangeMaxApplied;

        public AttackEvent onWeaponsChanged;
        
        [SerializeField] private Creature creature;
        
        [SerializeField] private CircleOverlapCheckComponent meleePosition;
        
        [SerializeField] private int meleePower = 10;
        [SerializeField] private float meleeTimeout = 0;
        
        [SerializeField] private float rangeTimeout = 1f;
        [SerializeField] private bool allowRangeLast = false;
        
        [SerializeField] private int rangeMaxCount = 3;
        [SerializeField] private float rangeMaxDelay = 0.3f;
        [SerializeField] private float rangeMaxTimeout = 1f;
        
        [SerializeField] private bool alwaysCanMelee = false;
        [SerializeField] private bool alwaysCanRange = false;
        [SerializeField] private bool alwaysCanRangeMax = false;

        private Cooldown _meleeCooldown = new Cooldown();
        private Cooldown _rangeCooldown = new Cooldown();
        private Cooldown _rangeMaxCooldown = new Cooldown();

        private ParticlesController _particlesController;
        private InventoryData _inventory;

        private void Start()
        {
            _particlesController = creature.ParticlesController;
            
            var sessionController = creature.SessionController;
            if (sessionController != null) _inventory = sessionController.GetModel().inventory;

            if (_inventory != null) _inventory.onChange += OnInventoryChanged;
        }

        public bool CanMelee()
        {
            if (!_meleeCooldown.IsReady) return false;

            return alwaysCanMelee || HasWeapon();
        }
        public void RequestMelee()
        {
            if (!CanMelee()) return;
            
            _meleeCooldown.Reset(meleeTimeout);
            onMeleeRequested?.Invoke();
        }
        public void DoMelee()
        {
            var gos = meleePosition.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var otherCreature = item.GetComponent<Creature>();
                if (otherCreature == creature) continue;
                
                var health = item.GetComponent<HealthComponent>();

                if (otherCreature != null)
                {
                    var hc = otherCreature.HealthController;
                    health = hc.GetHealthComponent();
                }
                
                if (health == null) continue;
                
                health.ApplyDamage(meleePower);        
            }
            onMeleeApplied?.Invoke();
        }

        public bool CanRange()
        {
            if (!_rangeCooldown.IsReady) return false;
            if (alwaysCanRange) return true;
            if (allowRangeLast && HasWeapon()) return true;

            return HasWeapon(2);
        }
        public void RequestRange()
        {
            if (!CanRange()) return;

            _rangeCooldown.Reset(rangeTimeout);
            onRangeRequested?.Invoke();
        }

        public void DoRange()
        {
            ApplyToInventory(-1);
            _particlesController.Spawn(AttackController.RangeParticle);
            
            onRangeApplied?.Invoke();
        }
        
        public bool CanRangeMax()
        {
            if (!_rangeMaxCooldown.IsReady) return false;
            if (alwaysCanRangeMax) return true;
            if (allowRangeLast && HasWeapon(rangeMaxCount)) return true;

            return HasWeapon(rangeMaxCount + 1);
        }
        public void RequestRangeMax()
        {
            if (!CanRangeMax()) return;
            
            _rangeMaxCooldown.Reset(rangeMaxTimeout);
            onRangeMaxRequested?.Invoke();
        }
        public void DoRangeMax()
        {
            ThrowWeapon(-rangeMaxCount);
            _particlesController.SpawnSeq(AttackController.RangeParticle, rangeMaxCount, rangeMaxDelay);
            
            onRangeMaxApplied?.Invoke();
        }

        public void TakeWeapon(int count = 1)
        {
            ApplyToInventory(count);
        }

        public void ThrowWeapon(int count = 1)
        {
            ApplyToInventory(-count);
        }

        public bool HasWeapon(int atLeast=1)
        {
            return _inventory?.Has(CreatureModel.Weapons, atLeast) ?? false;
        }

        public int CountWeapon()
        {
            return _inventory?.Count(CreatureModel.Weapons) ?? 0;
        }

        private void ApplyToInventory(int value)
        {
            _inventory?.Apply(CreatureModel.Weapons, value);
        }

        private void OnInventoryChanged(string id)
        {
            if (id != CreatureModel.Weapons) return;
            onWeaponsChanged?.Invoke();
        }
        
        protected override Creature GetCreature()
        {
            return creature;
        }

        public override void Die()
        {
            if (_inventory != null) _inventory.onChange -= OnInventoryChanged;
            onWeaponsChanged = delegate {};
            
            base.Die();
        }
    }
}