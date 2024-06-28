namespace Modules.Loadout.Scripts.Guns
{
    public class PistolSemiAutomatic: SemiAutomaticGun
    {
        public override void OnItemUse()
        {
            // Debug.Log("Pistol on trigger");
            
            ManualShoot();
        }

        public override void OnItemUseStop()
        {
            // Debug.Log("Pistol trigger released");

        }
    }
}