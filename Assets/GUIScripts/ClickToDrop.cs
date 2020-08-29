using Assets.Utilities;
using UnityEngine;

public class ClickToDrop : MonoBehaviour
{
    public GameObject playerCharacter;
    public float dropDistance;
    private IOptional<ItemAttributes> itemInHand;

    // Start is called before the first frame update
    void Start()
    {
        this.itemInHand = Optional.None<ItemAttributes>("Never placed item in hand");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInHand(ItemAttributes itemAttributes)
    {
        this.itemInHand = Optional.Some(itemAttributes);
        Debug.Log($"Set Item in hand = {itemAttributes.ItemName}");
        Cursor.SetCursor(itemAttributes.ItemTexture, Vector2.zero, CursorMode.Auto);
    }

    public bool IsItemInHand()
    {
        Debug.Log($"Holding item? {this.itemInHand.HasValue()}");
        return this.itemInHand.HasValue();
    }

    public void DropItemInHand()
    {
        var itemAttribute = this.itemInHand.GetValue();
        

        const int RaycastLength = 1000; // Arbitrary number that is just sufficiently large to hit anything within screen
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // TODO: I gotta do something about this cus when the ray hits the player you just kinda don't move (or attack an enemy just beyond the player)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RaycastLength))
        {
            Debug.Log("Dropping item in hand");
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            this.itemInHand = Optional.None<ItemAttributes>("Dropped item in hand");

            var raycastLocation = new Vector3(x: hit.point.x, y: hit.point.y, z: hit.point.z);
            var raycastMagnitude = raycastLocation.magnitude;
            var dropLocationX = this.dropDistance * (raycastLocation.x - this.playerCharacter.transform.position.x) / raycastMagnitude + this.playerCharacter.transform.position.x;
            var dropLocationY = this.dropDistance * (raycastLocation.y - this.playerCharacter.transform.position.y) / raycastMagnitude + this.playerCharacter.transform.position.y;
            var dropLocationZ = this.dropDistance * (raycastLocation.z - this.playerCharacter.transform.position.z) / raycastMagnitude + this.playerCharacter.transform.position.z;
            itemAttribute.itemGameObject.SetActive(true);
            itemAttribute.itemGameObject.transform.position = new Vector3(dropLocationX, dropLocationY, dropLocationZ);
        }
    }
}
