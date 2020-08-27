using UnityEngine;

public class ClickToPickup : MonoBehaviour
{
    public CharacterController characterController;
    public float pickupRange;
    public InventoryBehavior inventoryBehavior;

    private PickupBehavior targetPickup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && this.HasTargetWithinRange())
        {
            this.Pickup();
        }

        // TODO: need to make move toward toward item
    }

    public void SetTarget(PickupBehavior target)
    {
        this.targetPickup = target;
    }

    public PickupBehavior GetTarget()
    {
        return this.targetPickup;
    }

    public bool HasTarget()
    {
        return this.targetPickup != null;
    }

    private void Pickup()
    {
        var itemAttributes = this.targetPickup.Pickup();
        this.inventoryBehavior.PickupItem(itemAttributes);
    }

    private bool HasTargetWithinRange()
    {
        return this.targetPickup != null
            && Vector3.Distance(this.targetPickup.transform.position, this.transform.position) <= this.pickupRange;
    }
}
