﻿using UnityEngine;

public class ClickToDrop : MonoBehaviour
{
    private ItemAttributes itemInHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInHand(ItemAttributes itemAttributes)
    {
        this.itemInHand = itemAttributes;
        Cursor.SetCursor(itemAttributes.ItemTexture, Vector2.zero, CursorMode.Auto);
    }

    public void DropItemInHand()
    {
        // TODO: I think I need to centralize my click behavior. It's getting cumbersome to check everywhere what state the click is in
    }
}
