using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player _player;
    private CharacterMenuManager _characterMenuManager;
    private MainMenuManager _mainMenuManager;
    private InGameMenuManager _inGameMenuManager;
    [SerializeField]
    private PlayerInteractor interactor;
    private TutorialScreenManager _tutorialManager;
    private GlowEffectManager _glowEffectManager;
    [SerializeField]
    private ControlMap inputActions;
    public enum InputMaps {Gameplay, UI, Tutorial, Cutscene};
    private InputMaps activeInputMap;
    void OnEnable()
    {
        _player = GetComponent<Player>();
        _characterMenuManager = FindObjectOfType<CharacterMenuManager>();
        _mainMenuManager = FindObjectOfType<MainMenuManager>();
        _inGameMenuManager = FindObjectOfType<InGameMenuManager>();
        _tutorialManager = FindObjectOfType<TutorialScreenManager>();
        if(transform.childCount > 0) {
            _glowEffectManager = GetComponentInChildren<GlowEffectManager>();
        }

        if(inputActions == null) {
            inputActions = new ControlMap();
            inputActions.Enable();
        }

        activeInputMap = InputMaps.UI;
    }

    void OnDisable()
    {
        if(inputActions != null) {
            inputActions.Disable();
        } 
    }

    void Update()
    {
        if(activeInputMap == InputMaps.Gameplay) {
            GameplayInputs();
        }
        else if(activeInputMap == InputMaps.UI) {
            UIInputs();
        }
        else if(activeInputMap == InputMaps.Tutorial) {
            TutorialInputs();
        }
    }

    private void GameplayInputs() {
        float inputX = inputActions.Gameplay.Move.ReadValue<Vector2>().x;
        float jumpInput = inputActions.Gameplay.Jump.ReadValue<float>();
        float interactInput = inputActions.Gameplay.Interact.triggered ? 1 : 0;
        float escapeInput = inputActions.Gameplay.InGameMenu.triggered ? 1 : 0;
        float sunAuraEffectInput = inputActions.Gameplay.SunGlowAction.ReadValue<float>();
        float moonAuraEffectInput = inputActions.Gameplay.MoonGlowAction.ReadValue<float>();
        float characterMenuInput = inputActions.Gameplay.CharacterMenu.triggered ? 1 : 0;

        if(characterMenuInput != 0) {
            _characterMenuManager.EnableMenu();
            UpdateActiveActionMap(InputMaps.UI);
            return;
        }

        if(escapeInput != 0) {
            _inGameMenuManager.EnableMenu();
            UpdateActiveActionMap(InputMaps.UI);
            return;
        }

        _player.SetDirectionalInput(inputX);
        _player.SetJumpInput(jumpInput);
        interactor.SetInteractionValue(interactInput);

        if(interactor._lastKnownInteractable != null) {
            if(interactor._lastKnownInteractable.GetType() == typeof(SphereTotemManager)) {
                SphereTotemManager totemRef = interactor._lastKnownInteractable as SphereTotemManager;
                if(totemRef.currentSphere != null) {
                    if(sunAuraEffectInput > 0) {
                        interactor.OnActiveGlowInputPressed(GlowEffectManager.GlowType.Sun);
                    }
                    if(moonAuraEffectInput > 0) {
                        interactor.OnActiveGlowInputPressed(GlowEffectManager.GlowType.Moon);
                    }
                    return;
                }
            }
            else {
                if(sunAuraEffectInput > 0 && _glowEffectManager != null) {
                    _glowEffectManager.StartGlow(GlowEffectManager.GlowType.Sun, true);
                }
                if(moonAuraEffectInput > 0 && _glowEffectManager != null) {
                    _glowEffectManager.StartGlow(GlowEffectManager.GlowType.Moon, true);
                }
            }
        }
        else {
            if(sunAuraEffectInput > 0 && _glowEffectManager != null) {
                _glowEffectManager.StartGlow(GlowEffectManager.GlowType.Sun, true);
            }
            if(moonAuraEffectInput > 0 && _glowEffectManager != null) {
                _glowEffectManager.StartGlow(GlowEffectManager.GlowType.Moon, true);
            }
        }
    }

    private void UIInputs() {
        float directionalInput = inputActions.UI.DirectionInput.ReadValue<Vector2>().x;
        float interactInput = inputActions.UI.Interact.triggered ? 1 : 0;
        float escapeInput = inputActions.UI.Escape.triggered ? 1 : 0;
        float characterMenuInput = inputActions.UI.CharacterMenu.triggered ? 1 : 0;

        _characterMenuManager.InputUpdate(directionalInput, interactInput, escapeInput, characterMenuInput);
        _mainMenuManager.InputUpdate(directionalInput, interactInput, escapeInput);
        _inGameMenuManager.InputUpdate(directionalInput, interactInput, escapeInput);
    }

    private void TutorialInputs() {
        float interactInput = inputActions.UI.Interact.triggered ? 1 : 0;
        float escapeInput = inputActions.UI.Escape.triggered ? 1 : 0;

        _tutorialManager.UpdatePlayerInput(interactInput, escapeInput);
    }

    public void UpdateActiveActionMap(InputMaps newActiveMap) {
        _player.SetDirectionalInput(0);
        _player.SetJumpInput(0);
        activeInputMap = newActiveMap;
    }
}
