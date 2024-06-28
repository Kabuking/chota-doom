namespace Modules.Loadout.Scripts.Actions
{
    public class ItemActionHandler
    {
        public DropItemHandler DropItemHandler { get; private set; }
        public EquipUnequipHandler EquipUnequipHandler{ get; private set; }
        public PickupItemActionHandler PickupItemActionHandler{ get; private set; }
        public SwapActionHandler SwapActionHandler{ get; private set; }
        public SwitchItemHandler SwitchItemHandler{ get; private set; }
        
        public ItemActionHandler()
        {
            DropItemHandler = new DropItemHandler();
            EquipUnequipHandler = new EquipUnequipHandler();
            PickupItemActionHandler = new PickupItemActionHandler();
            SwapActionHandler = new SwapActionHandler();
            SwitchItemHandler = new SwitchItemHandler();

        }
    }
}
