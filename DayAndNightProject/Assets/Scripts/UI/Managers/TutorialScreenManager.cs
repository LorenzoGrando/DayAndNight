using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorialStarters;
    [SerializeField]
    private PlayerInput _playerInput;
    private bool isActive = false;
    private int lastActiveIndex = 0;

    void OnEnable()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    public void UpdatePlayerInput(float interact, float  escape) {
        if(isActive) {
            if(interact > 0 || escape > 0) {
                DisableTutorial();
            }
        }
    }

    public void TriggerTutorial (int screenIndex) {
        _playerInput.UpdateActiveActionMap(PlayerInput.InputMaps.Tutorial);
        lastActiveIndex = screenIndex;
        tutorialStarters[screenIndex].SetActive(true);
        tutorialStarters[screenIndex].GetComponentInChildren<Animator>().SetTrigger("TriggerStart");
        isActive = true;
    }

    private void DisableTutorial() {
        _playerInput.UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);

        tutorialStarters[lastActiveIndex].GetComponentInChildren<Animator>().SetTrigger("TriggerEnd");
        Invoke(nameof(DisableScreen), 2.0f);
        isActive = false;
    }

    private void DisableScreen() {
        tutorialStarters[lastActiveIndex].SetActive(false);
    }
}
