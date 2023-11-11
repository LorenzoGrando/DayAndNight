using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public Item thisConsumableType;
    public bool wasCollected;
    void Awake()
    {
        wasCollected = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            CollectionBehaviour(other.gameObject);
        }
    }

    public void CollectItem() {
        wasCollected = true;
        gameObject.SetActive(false);
        FindObjectOfType<ConsumableLoader>().UpdateConsumableData();
    }

    public void UpdateItem(Item newItem) {
        thisConsumableType = newItem;
        gameObject.name = newItem.name;
    }

    private void CollectionBehaviour(GameObject playerObject) {
        PlayerDataManager dataManager = playerObject.GetComponent<PlayerDataManager>();
        Inventory existingInventory = dataManager.currentPlayerData.currentInventory;

        foreach(Item item in existingInventory.consumableItems) {
            if(item.consumableType == thisConsumableType.consumableType) {
                item.itemQuantity++;
                CollectItem();
                break;
            }
        }

        dataManager.UpdateInventory(existingInventory);
    }
}
