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

    public void UpdateInventoryData(Inventory inventoryToCopy) {
        for(int i = 0; i < consumableItems.Length; i++) {
            consumableItems[i].itemStorageType = inventoryToCopy.consumableItems[i].itemStorageType;
            consumableItems[i].itemType = inventoryToCopy.consumableItems[i].itemType;
            consumableItems[i].consumableType = inventoryToCopy.consumableItems[i].consumableType;
            consumableItems[i].itemQuantity = inventoryToCopy.consumableItems[i].itemQuantity;
            consumableItems[i].isUnlocked = inventoryToCopy.consumableItems[i].isUnlocked;
            consumableItems[i].itemName = inventoryToCopy.consumableItems[i].itemName;
            consumableItems[i].itemThemeDescription = inventoryToCopy.consumableItems[i].itemThemeDescription;
            consumableItems[i].itemFunctionalitDescription = inventoryToCopy.consumableItems[i].itemFunctionalitDescription;
            consumableItems[i].itemSprite = inventoryToCopy.consumableItems[i].itemSprite;
            consumableItems[i].capeGlowType = inventoryToCopy.consumableItems[i].capeGlowType;
            for(int j = 0; j < consumableItems[i].buyRequirements.Length; j++) {
                consumableItems[i].buyRequirements[j] = inventoryToCopy.consumableItems[i].buyRequirements[j];
            }
        }

        for(int c = 0; c < capeItems.Length; c++) {
            capeItems[c].itemStorageType = inventoryToCopy.capeItems[c].itemStorageType;
            capeItems[c].itemType = inventoryToCopy.capeItems[c].itemType;
            capeItems[c].consumableType = inventoryToCopy.capeItems[c].consumableType;
            capeItems[c].itemQuantity = inventoryToCopy.capeItems[c].itemQuantity;
            capeItems[c].isUnlocked = inventoryToCopy.capeItems[c].isUnlocked;
            capeItems[c].itemName = inventoryToCopy.capeItems[c].itemName;
            capeItems[c].itemThemeDescription = inventoryToCopy.capeItems[c].itemThemeDescription;
            capeItems[c].itemFunctionalitDescription = inventoryToCopy.capeItems[c].itemFunctionalitDescription;
            capeItems[c].itemSprite = inventoryToCopy.capeItems[c].itemSprite;
            capeItems[c].capeGlowType = inventoryToCopy.capeItems[c].capeGlowType;
            for(int v = 0; v < capeItems[c].buyRequirements.Length; v++) {
                capeItems[c].buyRequirements[v] = inventoryToCopy.capeItems[c].buyRequirements[v];
            }
        }
        int activeCapeIndex = 0;
        for(int a = 0; a < inventoryToCopy.capeItems.Length; a++) {
            if(inventoryToCopy.capeItems[a] == inventoryToCopy.activeCape) {
                activeCapeIndex = a;
                break;
            }
        }

        activeCape = capeItems[activeCapeIndex];
    }
}
