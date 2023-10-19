using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuManager : MonoBehaviour
{
    private PlayerDataManager dataManager;
    [SerializeField]
    private GameObject canvasEnablerObject;
    [SerializeField]
    private GameObject capeTileHolder;
    [SerializeField]
    private GameObject collectableTileHolder;
    [SerializeField]
    private DescriptionArea descriptionManager;
    [SerializeField]
    private GameObject selectorObject;
    private CapeTile[] capeTiles;
    private CollectableTile[] collectableTiles;
    private int currentHoverIndex;

    private float lastSelectorUpdateTime;


    void OnEnable()
    {
        dataManager = FindObjectOfType<PlayerDataManager>();
        capeTiles = capeTileHolder.GetComponentsInChildren<CapeTile>();
        collectableTiles = collectableTileHolder.GetComponentsInChildren<CollectableTile>();
        Debug.Log(capeTiles.Length);
    }

    public void EnableMenu() {
        UpdateInventory();
        canvasEnablerObject.SetActive(true);
        currentHoverIndex = 0;
    }

    public void DisableMenu() {
        canvasEnablerObject.SetActive(false);
    }

    public void InputUpdate(float directional, float interact, float escape, float characterMenuLeaver) {
        if(characterMenuLeaver != 0 || escape != 0) {
            DisableMenu();
            FindObjectOfType<PlayerInput>().UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
        }

        if(lastSelectorUpdateTime + 0.115f < Time.time) {
            Debug.Log(directional);
            if(directional > 0) {
                currentHoverIndex++;
                if(currentHoverIndex >= capeTiles.Length)
                    currentHoverIndex = capeTiles.Length - 1;    
            }
            else if(directional < 0) {
                currentHoverIndex--;
                if(currentHoverIndex < 0)
                    currentHoverIndex = 0;
            }

            lastSelectorUpdateTime = Time.time;
        }
        UpdateSelectorPosition();
    }

    void UpdateInventory() {
        Inventory currentInventory = dataManager.GetInventory();

        for(int i = 0; i < capeTiles.Length; i++) {
            capeTiles[i].GetNewItem(currentInventory.capeItems[i]);
            capeTiles[i].UpdateAppearance();
        }

        for(int j = 0; j < collectableTiles.Length; j++) {
            float tileQuantity = currentInventory.consumableItems[j].itemQuantity;
            collectableTiles[j].UpdateText(tileQuantity);
        }
    }

    private void UpdateSelectorPosition() {
        Vector3 newPosition = capeTiles[currentHoverIndex].transform.position;
        selectorObject.transform.position = newPosition;
    }
}
