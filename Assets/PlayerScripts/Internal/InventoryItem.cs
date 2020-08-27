using System.Collections.Generic;

namespace PlayerScripts.Internal
{
    internal sealed class InventoryItem
    {
        public InventoryItem(ItemAttributes itemAttributes, IReadOnlyList<InventoryCell> cellsItemIsFilling)
        {
            this.ItemAttributes = itemAttributes;
            this.CellsFilled = cellsItemIsFilling;
        }

        public ItemAttributes ItemAttributes { get; }
        public IReadOnlyList<InventoryCell> CellsFilled { get; }
        // TODO: include some reference to what item this actually is (e.g. attributes/model)
    }
}

