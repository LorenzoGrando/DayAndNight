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
    private GameObject mainSelectorObject;
    private int currentHoverIndex = 0;
    private float lastSelectorUpdateTime;
    private float lastSelectorInteractTime;
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
                    if(currentHoverIndex >= buttonTiles.Length)
                        currentHoverIndex = buttonTiles.Length - 1;    
                }
                else if(directional < 0) {
                    currentHoverIndex--;
                    if(currentHoverIndex < 0)
                        currentHoverIndex = 0;
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
        }
    }

    private void OnInteract() {
        switch(currentHoverIndex) {
            case 0:
                //Options
            break;
            case 1:
                //Quit to menu
                fadeOutScreen.SetTrigger("PlayFadeIn");
                MethodToCallback Callback = CallReloadScene;
                StartCoroutine(routine:DelayCallbackBySeconds(2.75f, Callback));
            break;
        }
    }

    private void UpdateSelectorPosition() {
        Vector3 newPosition = mainSelectorObject.transform.position;
        newPosition.x = buttonTiles[currentHoverIndex].transform.position.x;
        mainSelectorObject.transform.position = newPosition;
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
