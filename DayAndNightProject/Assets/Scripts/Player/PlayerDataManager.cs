using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour {
    [SerializeField]
    public PlayerData currentPlayerData = new PlayerData();

    void OnEnable()
    {
        UpdateInventory(GetInventory());
    }

    public void UpdateInventory(Inventory newInventory) {
        currentPlayerData.currentInventory = newInventory;
        if(currentPlayerData.currentInventory.activeCape == null) {
            currentPlayerData.currentInventory.activeCape = currentPlayerData.currentInventory.capeItems[0];
        }
    }

    public void ResetInventory() {
        Inventory inventory = currentPlayerData.currentInventory;
        foreach(Item consumable in inventory.consumableItems) {
            consumable.itemQuantity = 0;
        }

        bool isFirstCape = true;
        foreach(Item cape in inventory.capeItems) {
            if(isFirstCape) {
                isFirstCape = false;
                continue;
            }
            cape.isUnlocked = false;
        }

        inventory.activeCape = inventory.capeItems[0];

        UpdateInventory(inventory);
    }

    public Inventory GetInventory() {
        return currentPlayerData.currentInventory;
    }

    public bool UpdateActiveCape(Item newCapeRef) {
        if(currentPlayerData.currentInventory.activeCape != newCapeRef) {
            currentPlayerData.currentInventory.activeCape = newCapeRef;
            return true;
        }
        return false;
    }

    public void UpdatePlayerData(PlayerData newData) {
        currentPlayerData.currentHeldSphere = newData.currentHeldSphere;
        ResetInventory();
        PerformLoadInventoryData(newData.inventorySaveData);
        UpdateInventory(currentPlayerData.currentInventory);
        UpdateActiveCape(currentPlayerData.currentInventory.activeCape);
        gameObject.GetComponentInChildren<GlowEffectManager>().ApplyCapeEffect(currentPlayerData.currentInventory.activeCape.capeGlowType);
    }

    public PlayerData GetPlayerData() {
        currentPlayerData.inventorySaveData = currentPlayerData.currentInventory.ReturnCurrentInventoryData();
        return currentPlayerData;
    }

    private void PerformLoadInventoryData(InventorySaveData inventoryToLoad) {
        currentPlayerData.currentInventory.UpdateInventoryData(inventoryToLoad);
    }
 }

[System.Serializable]
public class PlayerData {
    public CrystalSphere currentHeldSphere;
    [SerializeField]
    public Inventory currentInventory = new Inventory();
    public InventorySaveData inventorySaveData = new InventorySaveData();

    public PlayerData() {
        currentHeldSphere = null;
         currentInventory = new Inventory();
    }
}