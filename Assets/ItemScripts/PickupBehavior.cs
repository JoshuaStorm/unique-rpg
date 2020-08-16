using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public ClickToPickup playerCharacterClickToPickup;
    public ItemAttributes itemAttributes;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        this.playerCharacterClickToPickup.SetTarget(this);
    }

    void OnMouseExit()
    {
        this.playerCharacterClickToPickup.SetTarget(null); // TODO: do I really want to accept using null patterns? Optional maybe?
    }

    public ItemAttributes Pickup()
    {
        var itemAttributes = this.itemAttributes;
        Destroy(this.gameObject);
        return itemAttributes; // XXX: maybe this just returns null due to destroying the game object?
    }
}
