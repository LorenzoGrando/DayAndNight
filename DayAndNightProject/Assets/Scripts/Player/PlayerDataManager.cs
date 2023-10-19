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

        UpdateInventory(inventory);
    }

    public Inventory GetInventory() {
        return currentPlayerData.currentInventory;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            ResetInventory();
        }
    }
 }

[System.Serializable]
public class PlayerData {
    public CrystalSphere currentHeldSphere;
    [SerializeField]
    public Inventory currentInventory = new Inventory();
}