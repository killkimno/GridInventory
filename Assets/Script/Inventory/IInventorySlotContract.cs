namespace Script.Inventory
{
    public interface IInventorySlotContract
    {
        void OnDragSlotItem(InventorySlot slot);
        void OnDragStartSlotItem(InventorySlot slot);
        void OnDragEndSlotItem(InventorySlot slot);
    }
}
