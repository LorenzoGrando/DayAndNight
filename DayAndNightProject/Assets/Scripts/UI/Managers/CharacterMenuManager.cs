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
    private float lastSelectorInteractTime;


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
        if(canvasEnablerObject.activeSelf == true) {
            if(characterMenuLeaver != 0 || escape != 0) {
                DisableMenu();
                FindObjectOfType<PlayerInput>().UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
            }

            if(lastSelectorUpdateTime + 0.115f < Time.time) {
                if(directional > 0) {
                    bool playSound = true;
                    currentHoverIndex++;
                    if(currentHoverIndex >= capeTiles.Length) {
                        currentHoverIndex = capeTiles.Length - 1;
                        playSound = false;
                    }

                    if(playSound) {
                        FindObjectOfType<MenuSoundManager>().PlayMovementSound();
                    }
                }
                else if(directional < 0) {
                    bool playSound = true;
                    currentHoverIndex--;
                    if(currentHoverIndex < 0) {
                        currentHoverIndex = 0;
                        playSound = false;
                    }

                    if(playSound) {
                        FindObjectOfType<MenuSoundManager>().PlayMovementSound();
                    }
                }

                lastSelectorUpdateTime = Time.time;
            }

            UpdateSelectorPosition();

            if(lastSelectorInteractTime + 0.115f < Time.time) {
                if(interact != 0) {
                    OnInteract();
                    lastSelectorInteractTime = Time.time;
                }
            }

            UpdateInventory();
        }
    }

    void UpdateInventory() {
        Inventory currentInventory = dataManager.GetInventory();

        for(int i = 0; i < capeTiles.Length; i++) {
            capeTiles[i].GetNewItem(currentInventory.capeItems[i]);
            bool isActive = currentInventory.capeItems[i] == currentInventory.activeCape;
            capeTiles[i].UpdateAppearance(isActive);
        }

        for(int j = 0; j < collectableTiles.Length; j++) {
            float tileQuantity = currentInventory.consumableItems[j].itemQuantity;
            collectableTiles[j].UpdateText(tileQuantity);
        }
    }

    private void UpdateSelectorPosition() {
        Vector3 newPosition = capeTiles[currentHoverIndex].transform.position;
        selectorObject.transform.position = newPosition;
        descriptionManager.UpdateTexts(dataManager.GetInventory().capeItems[currentHoverIndex]);
    }

    private void OnInteract() {
        Inventory currentInventory = dataManager.GetInventory();

        Item capeRef = currentInventory.capeItems[currentHoverIndex];

        FindObjectOfType<MenuSoundManager>().PlaySelectionSound();

        if(capeRef.isUnlocked) {
            bool applyEffect = dataManager.UpdateActiveCape(capeRef);
            if(applyEffect) {
                if(capeRef.capeGlowType == GlowEffectManager.GlowType.Null) {
                    dataManager.gameObject.GetComponentInChildren<GlowEffectManager>().ResetToDefaultValues();
                }
                else {
                    dataManager.gameObject.GetComponentInChildren<GlowEffectManager>().ApplyCapeEffect(capeRef.capeGlowType);
                }
            }
        }
        else if (!capeRef.isUnlocked){
            Item newCapeRef = TryBuyCape(capeRef, out currentInventory);
            currentInventory.capeItems[currentHoverIndex] = newCapeRef;
            dataManager.UpdateInventory(currentInventory);
        }
    }

    private Item TryBuyCape(Item capeRef, out Inventory currentInventory) {
        currentInventory = dataManager.GetInventory();
        for(int i = 0; i < capeRef.buyRequirements.Length; i++) {
            if(!(currentInventory.consumableItems[i].itemQuantity >= capeRef.buyRequirements[i])) {
                return capeRef;
            }
        }
        capeRef.isUnlocked = true;
        
        for(int j = 0; j < capeRef.buyRequirements.Length; j++) {
            currentInventory.consumableItems[j].itemQuantity -= capeRef.buyRequirements[j];
        }
        return capeRef;
    }
}
