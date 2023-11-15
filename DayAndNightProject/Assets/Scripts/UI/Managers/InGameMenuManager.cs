using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    private delegate void MethodToCallback();
    private PlayerDataManager dataManager;
    [SerializeField]
    private Animator fadeOutScreen;
    [SerializeField]
    private GameObject canvasEnablerObject;
    [SerializeField]
    private GameObject[] buttonTiles;
    [SerializeField]
    private GameObject[] optionsTiles;
    [SerializeField]
    private GameObject optionsActiveObject;
    [SerializeField]
    private GameObject mainActiveObject;
    [SerializeField]
    private GameObject mainSelectorObject;
    [SerializeField]
    private GameObject optionsSelectorObject;
    private GameObject activeSelectorObject;
    private int currentHoverIndex = 0;
    private float lastSelectorUpdateTime;
    private float lastSelectorInteractTime;

    private enum ActiveInGameSelector {
        Main,
        Options,
    };

    private ActiveInGameSelector activeSelector;
    void OnEnable()
    {
        dataManager = FindObjectOfType<PlayerDataManager>();
    }

    public void EnableMenu() {
        canvasEnablerObject.SetActive(true);
        currentHoverIndex = 0;
    }

    public void DisableMenu() {
        canvasEnablerObject.SetActive(false);
        FindObjectOfType<PlayerInput>().UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
    }

    public void InputUpdate(float directional, float interact, float escape) {
        if(canvasEnablerObject.activeSelf == true) {
            if(escape != 0) {
                DisableMenu();
            }

            if(lastSelectorUpdateTime + 0.115f < Time.time) {
                if(directional > 0) {
                    currentHoverIndex++;
                    int maxInput = 1;
                    switch (activeSelector) {
                        case ActiveInGameSelector.Main:
                            maxInput = buttonTiles.Length;
                        break;
                        case ActiveInGameSelector.Options:
                            maxInput = optionsTiles.Length;
                        break;
                    }
                    if(currentHoverIndex >= maxInput)
                        currentHoverIndex = maxInput - 1;    
                }
                else if(directional < 0) {
                    currentHoverIndex--;
                    if(currentHoverIndex < 0)
                        currentHoverIndex = 0;
                }

                lastSelectorUpdateTime = Time.time;
            }

            if(activeSelector == ActiveInGameSelector.Main) {
                UpdateSelectorPosition(buttonTiles);
            }
            else if(activeSelector == ActiveInGameSelector.Options) {
                UpdateSelectorPosition(optionsTiles);
            }

            

            if(lastSelectorInteractTime + 0.115f < Time.time) {
                if(interact != 0) {
                    OnInteract();
                    lastSelectorInteractTime = Time.time;
                }
            }
        }
    }

    private void OnInteract() {
        if(activeSelector == ActiveInGameSelector.Main) {
            switch(currentHoverIndex) {
                case 0:
                    //Options
                    ShowOptionsMenu();
                break;
                case 1:
                    //Quit to menu
                    fadeOutScreen.SetTrigger("PlayFadeIn");
                    MethodToCallback Callback = CallReloadScene;
                    StartCoroutine(routine:DelayCallbackBySeconds(2.75f, Callback));
                break;
            }
        }
        else if(activeSelector == ActiveInGameSelector.Options) {
            CloseExtraWindows();
        }
    }

    private void UpdateSelectorPosition(GameObject[] tiles) {
        Vector3 newPosition = mainSelectorObject.transform.position;
        newPosition.x = tiles[currentHoverIndex].transform.position.x;
        mainSelectorObject.transform.position = newPosition;
    }

    private void CloseExtraWindows() {
        optionsActiveObject.SetActive(false);
        ChangeDefaultMenuVisibility(true);
        activeSelectorObject = mainSelectorObject;
        activeSelector = ActiveInGameSelector.Main;
        currentHoverIndex = 0;
        UpdateSelectorPosition(buttonTiles);
    }

    private void ShowOptionsMenu() {
        ChangeDefaultMenuVisibility(false);
        optionsActiveObject.SetActive(true);
        currentHoverIndex = 0;
        activeSelectorObject = optionsSelectorObject;
        activeSelector = ActiveInGameSelector.Options;

        UpdateSelectorPosition(optionsTiles);
    }

    private void ChangeDefaultMenuVisibility(bool activeState) {
        mainActiveObject.SetActive(activeState);
    }

    private void CallReloadScene() {
        SceneManager.LoadScene(0);
    }

    private IEnumerator DelayCallbackBySeconds(float duration, MethodToCallback Callback) {
        yield return new WaitForSeconds(duration);
        
        Callback();

        yield break;
    }
}
