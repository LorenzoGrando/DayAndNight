using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class Inventory
{
    public Item[] consumableItems;
    public Item[] capeItems;
    public Item activeCape = null;

    public Inventory(Inventory existingInventory = null)
    {
        if(existingInventory == null) {
            consumableItems = new Item[2];
            capeItems = new Item[4];
            activeCape = capeItems[0];
        }
        else {
            consumableItems = existingInventory.consumableItems;
            capeItems = existingInventory.capeItems;
            activeCape = existingInventory.activeCape;
        }
    }

    public void UpdateInventoryData(InventorySaveData inventoryToCopy) {
        for(int i = 0; i < consumableItems.Length; i++) {
            consumableItems[i].itemQuantity = inventoryToCopy.consumableQuantities[i];
        }

        for(int c = 0; c < capeItems.Length; c++) {
            capeItems[c].isUnlocked = inventoryToCopy.capesUnlockStatus[c];
        }
        activeCape = capeItems[inventoryToCopy.activeCapeIndexValue];
    }

    public InventorySaveData ReturnCurrentInventoryData() {
        InventorySaveData inventorySaveData = new InventorySaveData();
        inventorySaveData.consumableQuantities = new float[2];
        inventorySaveData.capesUnlockStatus = new bool[4];

        for(int s = 0; s < inventorySaveData.capesUnlockStatus.Length; s++) {
            if(s < 2) {
                inventorySaveData.consumableQuantities[s] = consumableItems[s].itemQuantity;
            }

            inventorySaveData.capesUnlockStatus[s] = capeItems[s].isUnlocked;

            if(activeCape == capeItems[s]) {
                inventorySaveData.activeCapeIndexValue = s;
            }
        }

        return inventorySaveData;
    }
}

[System.Serializable]
public class InventorySaveData {
    public float[] consumableQuantities;
    public bool[] capesUnlockStatus;
    public int activeCapeIndexValue;
}
