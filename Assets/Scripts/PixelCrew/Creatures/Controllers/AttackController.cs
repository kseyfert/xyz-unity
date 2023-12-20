using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class AttackController : AController
    {
        public const string RangeParticle = "range-projectile";

        public delegate void AttackEvent();

        public AttackEvent onMeleeRequested;
        public AttackEvent onMeleeApplied;

        public AttackEvent onRangeRequested;
        public AttackEvent onRangeApplied;

        public AttackEvent onRangeMaxRequested;
        public AttackEvent onRangeMaxApplied;

        public AttackEvent onWeaponsChanged;
        
        [SerializeField] private CircleOverlapCheckComponent meleePosition;
        
        [SerializeField] private int meleePower = 10;
        [SerializeField] private float meleeTimeout = 0;
        
        [SerializeField] private float rangeTimeout = 1f;
        
        [SerializeField] private int rangeMaxCount = 3;
        [SerializeField] private float rangeMaxDelay = 0.3f;
        [SerializeField] private float rangeMaxTimeout = 1f;
        
        [SerializeField] private bool alwaysCanMelee = false;
        [SerializeField] private bool alwaysCanRange = false;
        [SerializeField] private bool alwaysCanRangeMax = false;

        private readonly Cooldown _meleeCooldown = new Cooldown();
        private readonly Cooldown _rangeCooldown = new Cooldown();
        private readonly Cooldown _rangeMaxCooldown = new Cooldown();

        private ParticlesController _particlesController;
        private InventoryData _inventory;
        private QuickInventoryModel _quickInventory;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _particlesController = Creature.ParticlesController;
            
            var sessionController = Creature.SessionController;
            if (sessionController != null) _inventory = sessionController.GetModel().inventory;
            if (sessionController != null) _quickInventory = sessionController.GetQuickInventory();

            if (_inventory != null) _trash.Retain(_inventory.SubscribeAndInvoke(OnInventoryChanged));
        }

        public bool CanMelee()
        {
            if (!_meleeCooldown.IsReady) return false;

            return alwaysCanMelee || HasMeleeWeapon();
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
                if (otherCreature == Creature) continue;
                
                var health = item.GetComponent<HealthComponent>();

                if (otherCreature != null)
                {
                    var hc = otherCreature.HealthController;
                    if (hc != null) health = hc.GetHealthComponent();
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
            
            if (_inventory == null) return false;
            if (!InventoryIsSelectedThrowable()) return false;

            var throwableDef = DefsFacade.I.Throwable.Get(_quickInventory.SelectedItem.id);
            if (throwableDef.AllowThrowLast) return true;

            return _quickInventory.SelectedItem.value > 1;
        }
        public void RequestRange()
        {
            if (!CanRange()) return;

            _rangeCooldown.Reset(rangeTimeout);
            onRangeRequested?.Invoke();
        }

        public void DoRange()
        {
            ThrowSelectedItem(1);
            
            if (_inventory == null)
            {
                _particlesController.Spawn(AttackController.RangeParticle);
                onRangeApplied?.Invoke();
                return;
            }

            var def = DefsFacade.I.Throwable.Get(_quickInventory.SelectedItem.id);
            var projectile = def.Projectile;
            if (projectile == null) _particlesController.Spawn(AttackController.RangeParticle);
            else _particlesController.SpawnCustom(AttackController.RangeParticle, projectile);
            
            onRangeApplied?.Invoke();
        }
        
        public bool CanRangeMax()
        {
            if (!_rangeMaxCooldown.IsReady) return false;
            if (alwaysCanRangeMax) return true;
            
            if (_inventory == null) return false;
            if (!InventoryIsSelectedThrowable()) return false;

            var throwableDef = DefsFacade.I.Throwable.Get(_quickInventory.SelectedItem.id);
            if (throwableDef.AllowThrowLast) return _quickInventory.SelectedItem.value >= rangeMaxCount;

            return _quickInventory.SelectedItem.value > rangeMaxCount;
        }
        public void RequestRangeMax()
        {
            if (!CanRangeMax()) return;
            
            _rangeMaxCooldown.Reset(rangeMaxTimeout);
            onRangeMaxRequested?.Invoke();
        }
        public void DoRangeMax()
        {
            ThrowSelectedItem(rangeMaxCount);
            
            if (_inventory == null)
            {
                _particlesController.SpawnSeq(AttackController.RangeParticle, rangeMaxCount, rangeMaxDelay);
                onRangeMaxApplied?.Invoke();
                return;
            }

            var def = DefsFacade.I.Throwable.Get(_quickInventory.SelectedItem.id);
            var projectile = def.Projectile;
            if (projectile == null) _particlesController.SpawnSeq(AttackController.RangeParticle, rangeMaxCount, rangeMaxDelay);
            else _particlesController.SpawnSeqCustom(AttackController.RangeParticle, rangeMaxCount, rangeMaxDelay, projectile);
            
            onRangeMaxApplied?.Invoke();
        }

        private void ThrowSelectedItem(int count = 1)
        {
            InventoryApplyToSelected(-count);
        }

        public bool HasMeleeWeapon(int atLeast=1)
        {
            return _inventory?.Has(PlayerData.Weapons, atLeast) ?? false;
        }

        private void InventoryApplyToSelected(int value)
        {
            _inventory?.Apply(_quickInventory.SelectedItem.id, value);
        }

        private bool InventoryIsSelectedThrowable()
        {
            if (_inventory == null) return false;

            return _quickInventory.SelectedItemDef.HasTag(ItemTag.Throwable);
        }

        private void OnInventoryChanged(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != PlayerData.Weapons) return;
            onWeaponsChanged?.Invoke();
        }

        public override void Die()
        {
            _trash.Dispose();
            onWeaponsChanged = delegate {};
            
            base.Die();
        }
    }
}