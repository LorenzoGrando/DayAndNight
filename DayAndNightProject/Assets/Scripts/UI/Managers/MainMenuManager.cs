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
    private GameObject[] optionsOptions;
    private GameObject[] conflictOptions;
    [SerializeField]
    private GameObject[] extraWindowTiles;
    [SerializeField]
    private GameObject mainSelectorObject;
    [SerializeField]
    private GameObject optionsSelectorObject;
    [SerializeField]
    private GameObject conflictSelectorObject;
    [SerializeField] 
    private  GameObject logoManager;
    private GameObject activeSelectorObject;
    private int currentHoverIndex = 0;
    private float lastSelectorUpdateTime;
    private float lastSelectorInteractTime;

    private enum ActiveSelector {
        Main,
        Options,
        Conflict
    };

    private enum ConflictType {
        Default,
        Sprite
    };
    private ActiveSelector activeSelector;
    private ConflictType activeConflictType;



    void OnEnable()
    {
        dataManager = FindObjectOfType<PlayerDataManager>();
        activeSelectorObject = mainSelectorObject;
        SaveLoadSystem.CreateDefaultSave(false);

        bool isDefaultSave;
        SaveLoadSystem.GetCurrentSave(out isDefaultSave);

        FindObjectOfType<MenuSoundManager>().FadeInMenuSong();
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
        fadeOutScreen.SetTrigger("PlayFadeOut");
        FindObjectOfType<InGameSoundManager>().FadeInMusic();
    }

    private void ChangeDefaultMenuVisibility(bool isVisible) {
        foreach(GameObject menuObject in buttonTiles) {
            menuObject.SetActive(isVisible);
        }
        logoManager.SetActive(isVisible);
        mainSelectorObject.SetActive(isVisible);
    }

    public void InputUpdate(float directional, float interact, float escape) {
        if(canvasEnablerObject.activeSelf == true) {
            if(escape != 0) {
                CloseAllExtraWindows();
            }

            if(lastSelectorUpdateTime + 0.115f < Time.time) {
                if(directional > 0) {
                    bool playSound = true;
                    currentHoverIndex++;
                    int maxInput = 1;
                    switch (activeSelector) {
                        case ActiveSelector.Main:
                            maxInput = buttonTiles.Length;
                        break;
                        case ActiveSelector.Options:
                            maxInput = optionsOptions.Length;
                        break;
                        case ActiveSelector.Conflict:
                            maxInput = conflictOptions.Length;
                        break;
                    }
                    if(currentHoverIndex >= maxInput) {
                        currentHoverIndex = maxInput - 1;
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


            if(activeSelector == ActiveSelector.Main) {
                UpdateSelectorPosition(buttonTiles);
            }
            else if (activeSelector == ActiveSelector.Options) {
                UpdateSelectorPosition(optionsOptions);
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
            FindObjectOfType<MenuSoundManager>().PlaySelectionSound();
            switch(currentHoverIndex) {
                case 0:
                    bool isDefaultFile;
                    SaveData saveData = SaveLoadSystem.GetCurrentSave(out isDefaultFile);
                    if(isDefaultFile) {
                        OpenChooseSprite();
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
            FindObjectOfType<MenuSoundManager>().PlaySelectionSound();
            CloseAllExtraWindows();
        }
        else if (activeSelector == ActiveSelector.Conflict) {
            FindObjectOfType<MenuSoundManager>().PlaySelectionSound();
            switch(activeConflictType) {
                case ConflictType.Default:
                    if(currentHoverIndex == 0) {
                        CloseAllExtraWindows();
                        OpenChooseSprite();
                    }
                    else {
                        CloseAllExtraWindows();
                    }
                break;
                case ConflictType.Sprite:
                    if(currentHoverIndex == 0) {
                        OnNewGame(0);
                    }
                    else {
                        OnNewGame(1);
                    }
                break;
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
        FindObjectOfType<MenuSoundManager>().FadeOutMenuSong();
        FindObjectOfType<PlayerInput>().UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
        MethodToCallback callback = DisableMenu;
        StartCoroutine(routine:DelayCallbackBySeconds(2.75f, callback));
    }

    private void OnNewGame(int spriteChoice) {
        SaveLoadSystem.CreateDefaultSave(true);
        SaveData saveData = SaveLoadSystem.GetCurrentSave(out bool isDefault);
        saveData.thisGameSpriteChoice = (SaveData.PlayerSpriteChoice) spriteChoice;
        SaveLoadSystem.Save(false, saveData);
        StartGame();
    }

    private void OnLoadGame() {
        SaveLoadSystem.Load();
        StartGame();
    }

    private void OpenOptions() {
        ChangeDefaultMenuVisibility(false);
        extraWindowTiles[0].SetActive(true);
        currentHoverIndex = 0;
        optionsSelectorObject.gameObject.SetActive(true);
        activeSelectorObject = optionsSelectorObject;
        activeSelector = ActiveSelector.Options;

        if(optionsOptions == null) {
            GameObject[] options = new GameObject[1];
            options[0] = extraWindowTiles[0].transform.GetChild(0).gameObject;
            optionsOptions = options;
        }
        

        UpdateSelectorPosition(optionsOptions);
    }

    private void OpenChooseSprite() {
        ChangeDefaultMenuVisibility(false);
        extraWindowTiles[2].SetActive(true);
        currentHoverIndex = 0;
        conflictSelectorObject.SetActive(true);

        activeSelectorObject = conflictSelectorObject;
        activeSelector = ActiveSelector.Conflict;
        activeConflictType = ConflictType.Sprite;

        if(conflictOptions == null) {
            GameObject holderRef = extraWindowTiles[2].transform.GetChild(0).gameObject;
            GameObject[] options = new GameObject[holderRef.transform.childCount];
            for(int i = 0; i < holderRef.transform.childCount; i++) {
                options[i] = holderRef.transform.GetChild(i).gameObject;
            }
            conflictOptions = options;
        }
        UpdateSelectorPosition(conflictOptions);
    }

    private void OpenStartNewGameConflict() {
        ChangeDefaultMenuVisibility(false);
        extraWindowTiles[1].SetActive(true);
        currentHoverIndex = 0;
        conflictSelectorObject.SetActive(true);

        activeSelectorObject = conflictSelectorObject;
        activeSelector = ActiveSelector.Conflict;
        activeConflictType = ConflictType.Default;

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
        ChangeDefaultMenuVisibility(true);
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
