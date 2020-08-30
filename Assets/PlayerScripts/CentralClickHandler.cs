using UnityEngine;

public class CentralClickHandler : MonoBehaviour
{
    public MouseOverGuiElementHandler mouseOverGuiElementHandler;
    public InventoryGui inventoryGui;
    public ClickToAttack clickToAttack;
    public ClickToMove clickToMove;
    public ClickToPickup clickToPickup;
    public ClickToDrop clickToDrop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.HandleLeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            this.HandleRightClick();
        }
    }

    private void HandleLeftClick()
    {
        var isMouseOverIntentory = this.inventoryGui.IsMouseOverInventory();
        var isItemInHand = this.clickToDrop.IsItemInHand();

        if (isMouseOverIntentory)
        {
            this.inventoryGui.HandleLeftClick(isItemInHand);
            return;
        }
        if (isItemInHand)
        {
            this.clickToDrop.DropItemInHand();
            return;
        }

        this.clickToMove.HandleLeftClick();
        this.clickToPickup.HandleLeftClick();
        this.clickToAttack.HandleLeftClick();
    }

    private void HandleRightClick()
    {

    }
}
