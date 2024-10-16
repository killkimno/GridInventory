namespace Script.Inventory
{
    public interface IDropFieldContract
    {
        void OnDropSlotItem(InventorySlot slot, int x, int y);
    }

    public interface IRemoveFiledContract
    {
        void OnDropRemoveItem(InventorySlot slot);
    }
}
