using UnityEngine;

public class InventoryGui : MonoBehaviour
{
    public Texture2D inventoryCellImage;
    public Texture2D filledInventoryCellImage;
    public InventoryBehavior inventoryBehavior;

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
            this.isInventoryOpen = !this.isInventoryOpen;
        }
    }

    // OnGUI is some Unity magic that I don't fully understand how it differs from Update(). Maybe called less often?
    void OnGUI()
    {
        if (this.isInventoryOpen)
        {
            this.DrawInventory();
        }
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
                var getThisCellImage = this.GetCellImage(i, j);
                GUI.DrawTexture(position: cellPosition, image: getThisCellImage);
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

    private int GetStartingY()
    {
        return Screen.height - this.GetCellWidth() * this.inventoryBehavior.GetInventoryHeight();
    }

    private int GetCellWidth()
    {
        return (Screen.width - this.GetStartingX()) / this.inventoryBehavior.GetInventoryWidth();
    }
}
