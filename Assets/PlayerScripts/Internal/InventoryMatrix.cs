using Assets.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PlayerScripts.Internal
{
    internal sealed class InventoryMatrix
    {
        private readonly IReadOnlyList<InventoryRow> inventoryRows;
        private readonly int maxWidth;
        private readonly int maxHeight;

        public InventoryMatrix(int width, int height)
        {
            this.maxWidth = width;
            this.maxHeight = height;
            var inventoryRows = new List<InventoryRow>(capacity: height);
            for (var i = 0; i < height; i++)
            {
                inventoryRows.Add(new InventoryRow(width));
            }
            this.inventoryRows = inventoryRows;
        }
        
        // TODO: for now just assume every item is placed from the top-left corner
        public bool DoesItemFit(int x, int y, ItemAttributes itemAttributes)
        {
            this.AssertValidRow(x, y);
            var width = itemAttributes.Width;
            var height = itemAttributes.Height;
            if (x + width > this.maxWidth || y + height > this.maxHeight) 
            {
                return false;
            }

            var itemFits = true;
            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    itemFits &= this.IsCellEmpty(x + i, y + j);
                }
            }
            return itemFits;
        }

        //  This is hella inefficient, sure but like it's a 10x25 inventory. Maybe I'll make it 20x40 or something but it isn't like this needs to scale to 1000x1000
        public bool TryPlaceItemAnywhere(ItemAttributes itemAttributes)
        {
            for (var j = 0; j < this.maxHeight; j++)
            {
                for (var i = 0; i < this.maxWidth; i++)
                {
                    if (this.DoesItemFit(i, j, itemAttributes))
                    {
                        this.FillCellsWithItem(i, j, itemAttributes);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryPlaceItem(int x, int y, ItemAttributes itemAttributes)
        {
            this.AssertValidRow(x, y);
            if (!this.DoesItemFit(x, y, itemAttributes))
            {
                return false;
            }

            this.FillCellsWithItem(x, y, itemAttributes);
            return true;
        }

        public int GetMaxWidth()
        {
            return this.maxWidth;
        }

        public int GetMaxHeight()
        {
            return this.maxHeight;
        }

        public bool IsCellEmpty(int x, int y)
        {
            this.AssertValidRow(x, y);
            return this.IsCellEmptyUnsafe(x, y);
        }

        public IOptional<ItemAttributes> TakeItemInCell(int x, int y)
        {
            this.AssertValidRow(x, y);
            if (this.IsCellEmptyUnsafe(x, y))
            {
                return Optional.None<ItemAttributes>("No item found in cell");
            }

            // TODO: need to store more than just a string for the item in order to TakeItem, since I need to know which cells a single item is actually taking up

            var item = this.RemoveItemFromCellUnsafe(x, y);
            return Optional.Some(item);
        }

        private ItemAttributes RemoveItemFromCellUnsafe(int x, int y)
        {
            var clickedCell = this.inventoryRows[y].GetCell(x);
            var inventoryItem = clickedCell.InventoryItem;
            foreach (var cell in inventoryItem.CellsFilled)
            {
                cell.RemoveItem();
            }

            return inventoryItem.ItemAttributes;
        }

        private bool IsCellEmptyUnsafe(int x, int y)
        {
            return this.inventoryRows[y].IsCellEmpty(x: x);
        }

        private void FillCellsWithItem(int x, int y, ItemAttributes itemAttributes)
        {
            var height = itemAttributes.Height;
            var width = itemAttributes.Width;
            var cellsItemIsFilling = new List<InventoryCell>();
            for (var j = 0; j < height; j++)
            {
                var inventoryRow = this.inventoryRows[y+j];
                for (var i = 0; i < width; i++)
                {
                    var cell = inventoryRow.GetCell(x + i);
                    cellsItemIsFilling.Add(cell);
                }
            }

            var inventoryItem = new InventoryItem(itemAttributes, cellsItemIsFilling);
            for (var j = 0; j < height; j++)
            {
                var inventoryRow = this.inventoryRows[y+j];
                for (var i = 0; i < width; i++)
                {
                    inventoryRow.GetCell(x + i).SetItem(inventoryItem);
                }
            }
        }

        private void AssertValidRow(int x, int y)
        {
            if (y < 0)
            {
                throw new ApplicationException($"IsCellEmpty({x}, {y}) failed, {y} < 0");
            }
            if (y >= this.inventoryRows.Count)
            {
                throw new ApplicationException($"IsCellEmpty({x}, {y}) failed, {y} >= InventoryRowsLength({this.inventoryRows.Count})");
            }
        }
    }
}
