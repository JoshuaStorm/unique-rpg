using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    // TODO: take in a drop table and drop randomly based on drop table. For now just get anything to drop
    public Transform loot;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // TODO: maybe initially give it some up velocity for diablo-style drop
    }


    public void DropLoot(Vector3 location)
    {
        var trueRotationToLocation = Quaternion.LookRotation(location);
        location.y = location.y + 2f;
        Instantiate(this.loot, location, trueRotationToLocation);
    }
}
