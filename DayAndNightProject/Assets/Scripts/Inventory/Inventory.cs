using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory
{
    public Item[] consumableItems;
    public Item[] capeItems;

    public Inventory(Inventory existingInventory = null)
    {
        if(existingInventory == null) {
            consumableItems = new Item[2];
            capeItems = new Item[4];
        }
        else {
            consumableItems = existingInventory.consumableItems;
            capeItems = existingInventory.capeItems;
        }
    }
}
