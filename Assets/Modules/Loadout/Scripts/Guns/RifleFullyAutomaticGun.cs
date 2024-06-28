namespace Modules.Loadout.Scripts.Guns
{
    public class RifleFullyAutomaticGun: FullyAutomaticGun
    {
        public override void OnItemUse()
        {
            activateAutomaticFire = true;
        }

        public override void OnItemUseStop()
        {
            activateAutomaticFire = false;
        }
    }
}