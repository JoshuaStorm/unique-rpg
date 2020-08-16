using PlayerScripts.Internal;
using UnityEngine;

public partial class InventoryBehavior : MonoBehaviour
{
    private const int InventoryWidth = 18;
    private const int InventoryHeight = 8;

    private InventoryMatrix inventoryMatrix;

    // Start is called before the first frame update
    void Start()
    {
        this.inventoryMatrix = new InventoryMatrix(width: InventoryWidth, height: InventoryHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickupItem(string standinItemName)
    {
        var width = 1;
        var height = 3;
        this.inventoryMatrix.TryPlaceItemAnywhere(width, height, standinItemName);
    }

    public bool ContainsItemInCell(int i, int j)
    {
        return !this.inventoryMatrix.IsCellEmpty(i, j);
    }

    public int GetInventoryWidth()
    {
        return this.inventoryMatrix.GetMaxWidth();
    }

    public int GetInventoryHeight()
    {
        return this.inventoryMatrix.GetMaxHeight();
    }
}
