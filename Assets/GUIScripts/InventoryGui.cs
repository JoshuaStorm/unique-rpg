using Assets.Utilities;
using System;
using UnityEngine;

public class InventoryGui : MonoBehaviour
{
    public Texture2D inventoryCellImage;
    public Texture2D filledInventoryCellImage;
    public ClickToDrop clickToDrop;
    public InventoryBehavior inventoryBehavior;
    public MouseOverGuiElementHandler mouseOverGuiElementHandler;

    private bool isInventoryOpen;

    // Start is called before the first frame update
    void Start()
    {
        this.isInventoryOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("i"))
        {
            if (this.isInventoryOpen)
            {
                this.mouseOverGuiElementHandler.SetMouseExitGuiElement();
            }
            this.isInventoryOpen = !this.isInventoryOpen;
        }
    }

    // OnGUI sometimes called more than once a frame, so shouldn't check clicks in it
    void OnGUI()
    {
        if (this.isInventoryOpen)
        {
            this.DrawInventory();
        }
    }

    public void HandleLeftClick(bool isItemInHand)
    {
        AssertMouseOverInventory();
        if (isItemInHand)
        {
            this.HandleLeftClickWithItemInHandUnsafe();
        }
        else
        {
            this.HandleLeftClickItemInInventoryUnsafe();
        }
    }

    private void HandleLeftClickWithItemInHandUnsafe()
    {
        var x = this.GetCellColumnMouseIsOverUnsafe();
        var y = this.GetCellRowMouseIsOverUnsafe();
        var itemInHand = this.clickToDrop.RemoveItemInHand();
        if (this.inventoryBehavior.DoesItemFit(x, y, itemInHand))
        {
            this.inventoryBehavior.PlaceItem(x, y, itemInHand);
        }
        else
        {
            this.clickToDrop.SetItemInHand(itemInHand);
        }
    }

    private void HandleLeftClickItemInInventoryUnsafe()
    {
        var x = this.GetCellColumnMouseIsOverUnsafe();
        var y = this.GetCellRowMouseIsOverUnsafe();
        var item = this.inventoryBehavior.TakeItemInCell(x, y);
        if (item.HasValue())
        {
            this.clickToDrop.SetItemInHand(item.GetValue());
        }
    }

    public bool IsMouseOverInventory()
    {
        if (!this.isInventoryOpen)
        {
            this.mouseOverGuiElementHandler.SetMouseExitGuiElement();
            return false;
        }

        var mouseX = GuiSpaceMousePosition.GetMouseX();
        var mouseY = GuiSpaceMousePosition.GetMouseY();
        var mouseWithinXBounds = mouseX >= this.GetStartingX() && mouseX < this.GetEndX();
        var mouseWithinYBounds = mouseY >= this.GetStartingY() && mouseY < this.GetEndY();
        if (mouseWithinXBounds && mouseWithinYBounds)
        {
            this.mouseOverGuiElementHandler.SetMouseIsOverGuiElement();
            return true;
        }
        else
        {
            this.mouseOverGuiElementHandler.SetMouseExitGuiElement();
            return false;
        }
    }

    private void AssertMouseOverInventory()
    {
        if (!this.IsMouseOverInventory())
        {
            throw new ApplicationException($"Called function that assumed mouse is over inventory, but it was not");
        }
    }

    private int GetCellColumnMouseIsOverUnsafe()
    {
        var rawColumnNumber = Convert.ToInt32((GuiSpaceMousePosition.GetMouseX() - this.GetStartingX() - this.GetCellWidth() / 2.0) / this.GetCellWidth());
        // Clamp down to avoid indexing issues from hitting around the last pixel
        return Math.Min(rawColumnNumber, this.inventoryBehavior.GetInventoryWidth() - 1); 
    }
    
    private int GetCellRowMouseIsOverUnsafe()
    {

        var rawRowNumber =  Convert.ToInt32((GuiSpaceMousePosition.GetMouseY() - this.GetStartingY() - this.GetCellWidth() / 2.0) / this.GetCellWidth());
        // Clamp down to avoid indexing issues from hitting around the last pixel
        return Math.Min(rawRowNumber, this.inventoryBehavior.GetInventoryHeight() - 1);
    }

    private void DrawInventory()
    {
        var heightInCells = this.inventoryBehavior.GetInventoryHeight();
        var widthInCells = this.inventoryBehavior.GetInventoryWidth();
        var cellWidthInPixels = this.GetCellWidth();
        for (var i = 0; i < widthInCells; i++)
        {
            var x = this.GetStartingX() + i * cellWidthInPixels;
            for (var j = 0; j < heightInCells; j++)
            {
                var y = GetStartingY() + j * cellWidthInPixels; // For now use width to ensure square cells
                var cellPosition = new Rect(x: x, y: y, width: cellWidthInPixels, height: cellWidthInPixels);
                var thisCellImage = this.GetCellImage(i, j);
                GUI.DrawTexture(position: cellPosition, image: thisCellImage);
            }
        }

    }

    private Texture2D GetCellImage(int i, int j)
    {
        if (this.inventoryBehavior.ContainsItemInCell(i, j))
        {
            return this.filledInventoryCellImage;
        }

        return this.inventoryCellImage;
    }

    private int GetStartingX()
    {
        return Screen.width - 3 * Screen.height / 4; // Get fucked if you're in potrait mode
    }

    private int GetEndX()
    {
        return this.GetStartingX() + this.GetCellWidth() * this.inventoryBehavior.GetInventoryWidth();
    }

    private int GetStartingY()
    {
        return Screen.height - this.GetCellWidth() * this.inventoryBehavior.GetInventoryHeight();
    }

    private int GetEndY()
    {
        return Screen.height;
    }

    private int GetCellWidth()
    {
        return (Screen.width - this.GetStartingX()) / this.inventoryBehavior.GetInventoryWidth();
    }
}
