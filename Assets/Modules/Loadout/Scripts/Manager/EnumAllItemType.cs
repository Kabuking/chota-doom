namespace Modules.Loadout.Scripts.Manager
{
    public class EnumAllItemType
    {
        public enum ItemCategory
        {
            Weapon,
            Ammunition,
            Consumables,
            Gear,
            None
        }
        
        public enum ItemId
        {
            Pistol, Rifle
        }

        public enum WeaponTriggerType
        {
            FullyAutomatic,
            SemiAutomatic,
            Burst
        }

        public enum ItemParentSocketName
        {
            PistolSocket,
            RifleSocket,
            FullyAutomaticSocket
        }
        
        public enum ItemState
        {
            OnGround_Idle,
            OnGround_PickupShowing,
            Unequip,
            Equipped,
            Unmarked
        }

        public enum AmmoType
        {
            Light,
            Medium,
            Heavy,
            Shells,
            Snipe
        }

        public enum GunState
        {
            Firing,
            Carrying,
            Reloading
        }
    }
}