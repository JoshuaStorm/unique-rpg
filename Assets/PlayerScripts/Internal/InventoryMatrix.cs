using System;
using System.Collections.Generic;

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
            UnityEngine.Debug.Log($"Making inventory with {this.inventoryRows.Count} rows");
        }
        
        // TODO: for now just assume every item is placed from the top-left corner
        public bool ItemFits(int x, int y, int width, int height)
        {
            this.AssertValidRow(x, y);
            if (x + width > this.maxWidth || y + height >= this.maxHeight) 
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
        public bool TryPlaceItemAnywhere(int width, int height, string standinItemName)
        {
            for (var j = 0; j < this.maxHeight; j++)
            {
                for (var i = 0; i < this.maxWidth; i++)
                {
                    if (this.ItemFits(i, j, width, height))
                    {
                        this.FillCellsWithItem(i, j, width, height, standinItemName);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryPlaceItem(int x, int y, int width, int height, string standinItemName)
        {
            this.AssertValidRow(x, y);
            if (!this.ItemFits(x, y, width, height))
            {
                return false;
            }

            this.FillCellsWithItem(x, y, width, height, standinItemName);
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
            return this.inventoryRows[y].IsCellEmpty(x: x);
        }

        private void FillCellsWithItem(int x, int y, int width, int height, string standinItemName)
        {
            for (var j = 0; j < height; j++)
            {
                var inventoryRow = this.inventoryRows[y+j];
                for (var i = 0; i < width; i++)
                {
                    UnityEngine.Debug.Log($"Filling Inventory ({x+i},{y+j}) = {standinItemName}");
                    inventoryRow.GetCell(x+i).SetItem(standinItemName);
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
