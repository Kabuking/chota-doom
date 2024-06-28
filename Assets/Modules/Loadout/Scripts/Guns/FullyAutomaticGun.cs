using UnityEngine;

namespace Modules.Loadout.Scripts.Guns
{
    //BASE
    public abstract class FullyAutomaticGun : TriggerWeaponBase
    {
        [SerializeField] private float firingRate=0.5f;
        protected bool activateAutomaticFire = false;
        private float _lastShootTimeForAutomatic;
        
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (activateAutomaticFire)
            {
                AutomaticShoot();
            }
        }

        void AutomaticShoot()
        {
            if (Time.time > _lastShootTimeForAutomatic + firingRate)
            {
                _lastShootTimeForAutomatic = Time.time;
                ApplyFire();
            }
        }
        
        void ApplyFire()
        {
            ManualShoot();
        }

        /*public override void OnItemCarry(PlayerStateName playerStateName)
        {
            base.OnItemCarry(playerStateName);
            activateAutomaticFire = false;
            PositionConstraint p;
        }*/
    }
}