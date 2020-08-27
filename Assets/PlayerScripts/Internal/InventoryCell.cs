namespace PlayerScripts.Internal
{

    internal sealed class InventoryCell
    {
        public InventoryCell()
        {
            this.InventoryItem = null;
        }

        public bool IsEmpty => this.InventoryItem == null;
        public InventoryItem InventoryItem { get; private set; }

        public void SetItem(InventoryItem inventoryItem)
        {
            this.InventoryItem = inventoryItem;
        }

        public void RemoveItem()
        {
            this.InventoryItem = null;
        }
    }
}

