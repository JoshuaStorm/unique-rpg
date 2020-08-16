namespace PlayerScripts.Internal
{
    internal sealed class InventoryCell
    {
        private string itemName; // TODO: make this item behavior type not just item name

        public InventoryCell()
        {
            this.itemName = null;
        }

        public bool IsEmpty => this.itemName == null;

        public void SetItem(string standinItemName)
        {
            this.itemName = standinItemName;
        }

        public void RemoveItem()
        {
            this.itemName = null;
        }
    }
}

