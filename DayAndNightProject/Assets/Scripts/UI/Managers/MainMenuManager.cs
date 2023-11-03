using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private delegate void MethodToCallback();
    private PlayerDataManager dataManager;
    [SerializeField]
    private Animator fadeOutScreen;
    [SerializeField]
    private GameObject canvasEnablerObject;
    [SerializeField]
    private GameObject[] buttonTiles;
    private GameObject[] conflictOptions;
    [SerializeField]
    private GameObject[] extraWindowTiles;
    [SerializeField]
    private GameObject mainSelectorObject;
    [SerializeField]
    private GameObject optionsSelectorObject;
    [SerializeField]
    private GameObject conflictSelectorObject;
    private GameObject activeSelectorObject;
    private int currentHoverIndex = 0;
    private float lastSelectorUpdateTime;
    private float lastSelectorInteractTime;

    private enum ActiveSelector {
        Main,
        Options,
        Conflict
    };
    private ActiveSelector activeSelector;



    void OnEnable()
    {
        dataManager = FindObjectOfType<PlayerDataManager>();
        activeSelectorObject = mainSelectorObject;
        SaveLoadSystem.CreateDefaultSave(false);

        bool isDefaultSave;
        SaveLoadSystem.GetCurrentSave(out isDefaultSave);
        if(isDefaultSave) {
            buttonTiles[1].GetComponent<CanvasGroup>().alpha = 0.35f;
        }
        else {
            buttonTiles[1].GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public void EnableMenu() {
        canvasEnablerObject.SetActive(true);
        activeSelectorObject = mainSelectorObject;
        activeSelector = ActiveSelector.Main;
        currentHoverIndex = 0;

        fadeOutScreen.SetTrigger("PlayFadeOut");
    }

    public void DisableMenu() {
        canvasEnablerObject.SetActive(false);
        FindObjectOfType<PlayerInput>().UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
        fadeOutScreen.SetTrigger("PlayFadeOut");
    }

    public void InputUpdate(float directional, float interact, float escape) {
        if(canvasEnablerObject.activeSelf == true) {
            if(escape != 0) {
                CloseAllExtraWindows();
            }

            if(lastSelectorUpdateTime + 0.115f < Time.time) {
                if(directional > 0) {
                    currentHoverIndex++;
                    int maxInput = 1;
                    switch (activeSelector) {
                        case ActiveSelector.Main:
                            maxInput = buttonTiles.Length;
                        break;
                        case ActiveSelector.Options:
                            maxInput = buttonTiles.Length;
                        break;
                        case ActiveSelector.Conflict:
                            maxInput = conflictOptions.Length;
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


            if(activeSelector == ActiveSelector.Main) {
                UpdateSelectorPosition(buttonTiles);
            }
            else if (activeSelector == ActiveSelector.Options) {
                UpdateSelectorPosition(buttonTiles);
            }
            else if (activeSelector == ActiveSelector.Conflict) {
                UpdateSelectorPosition(conflictOptions);
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
        if(activeSelector == ActiveSelector.Main) {
            switch(currentHoverIndex) {
                case 0:
                    bool isDefaultFile;
                    SaveData saveData = SaveLoadSystem.GetCurrentSave(out isDefaultFile);
                    if(isDefaultFile) {
                        OnNewGame();
                    }
                    else {
                        OpenStartNewGameConflict();
                    }
                break;
                case 1:
                    bool isDefaultLoadFile;
                    SaveData loadSaveData = SaveLoadSystem.GetCurrentSave(out isDefaultLoadFile);
                    if(!isDefaultLoadFile) {
                        OnLoadGame();
                    }
                break;
                case 2:
                    OpenOptions();
                break;
                case 3:
                    Application.Quit();
                break;
            }
        }
        else if(activeSelector == ActiveSelector.Options) {

        }
        else if (activeSelector == ActiveSelector.Conflict) {
            if(currentHoverIndex == 0) {
                OnNewGame();
            }
            else {
                CloseAllExtraWindows();
            }
        }
    }

    private void UpdateSelectorPosition(GameObject[] buttons) {
        Vector3 newPosition = activeSelectorObject.transform.position;
        newPosition.x = buttons[currentHoverIndex].transform.position.x;
        activeSelectorObject.transform.position = newPosition;
    }

    private void StartGame() {
        fadeOutScreen.SetTrigger("PlayFadeIn");
        MethodToCallback callback = DisableMenu;
        StartCoroutine(routine:DelayCallbackBySeconds(2.75f, callback));
    }

    private void OnNewGame() {
        SaveLoadSystem.CreateDefaultSave(true);
        StartGame();
    }

    private void OnLoadGame() {
        SaveLoadSystem.Load();
        StartGame();
    }

    private void OpenOptions() {
        extraWindowTiles[0].SetActive(true);
        currentHoverIndex = 0;
        activeSelectorObject = optionsSelectorObject;
        activeSelector = ActiveSelector.Options;
        UpdateSelectorPosition(buttonTiles);
    }

    private void OpenLoadConflict() {

    }

    private void OpenStartNewGameConflict() {
        extraWindowTiles[1].SetActive(true);
        currentHoverIndex = 0;
        optionsSelectorObject.SetActive(true);

        activeSelectorObject = optionsSelectorObject;
        activeSelector = ActiveSelector.Conflict;

        if(conflictOptions == null) {
            GameObject holderRef = extraWindowTiles[1].transform.GetChild(0).gameObject;
            GameObject[] options = new GameObject[holderRef.transform.childCount];
            for(int i = 0; i < holderRef.transform.childCount; i++) {
                options[i] = holderRef.transform.GetChild(i).gameObject;
            }
            conflictOptions = options;
        }
        UpdateSelectorPosition(conflictOptions);
    }

    private void CloseAllExtraWindows() {
        foreach(GameObject gameObject in extraWindowTiles) {
            gameObject.SetActive(false);
        }
        optionsSelectorObject.SetActive(false);
        conflictSelectorObject.SetActive(false);
        activeSelectorObject = mainSelectorObject;
        activeSelector = ActiveSelector.Main;
        currentHoverIndex = 0;
        UpdateSelectorPosition(buttonTiles);
    }

    private IEnumerator DelayCallbackBySeconds(float duration, MethodToCallback Callback) {
        yield return new WaitForSeconds(duration);
        
        Callback();

        yield break;
    }
}
