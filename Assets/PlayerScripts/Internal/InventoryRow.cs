using System;
using System.Collections.Generic;

namespace PlayerScripts.Internal
{
    internal sealed class InventoryRow
    {
        private readonly IReadOnlyList<InventoryCell> inventoryCells;

        public InventoryRow(int length)
        {
            var inventoryCells = new List<InventoryCell>(capacity: length);
            for (var i = 0; i < length; i++)
            {
                inventoryCells.Add(new InventoryCell());
            }
            this.inventoryCells = inventoryCells;
            UnityEngine.Debug.Log($"Made row with {this.inventoryCells.Count} cells");
        }

        public bool IsCellEmpty(int x)
        {
            this.AssertValidLocation(x);
            return this.inventoryCells[x].IsEmpty;
        }

        public InventoryCell GetCell(int x)
        {
            this.AssertValidLocation(x);
            return this.inventoryCells[x];
        }

        private void AssertValidLocation(int x)
        {
            if (x < 0)
            {
                throw new ApplicationException($"IsCellEmpty({x}) failed, {x} < 0");
            }
            if (x >= this.inventoryCells.Count)
            {
                throw new ApplicationException($"IsCellEmpty({x}) failed, {x} >= InventoryCellsLength({this.inventoryCells.Count})");
            }
        }
    }
}
