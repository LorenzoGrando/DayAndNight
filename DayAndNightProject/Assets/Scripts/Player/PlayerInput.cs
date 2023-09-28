using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player _player;
    private GlowEffectManager _glowEffectManager;
    [SerializeField]
    private ControlMap inputActions;
    void Start()
    {
        _player = GetComponent<Player>();
        if(transform.childCount > 0) {
            _glowEffectManager = GetComponentInChildren<GlowEffectManager>();
        }

        if(inputActions == null) {
            inputActions = new ControlMap();
            inputActions.Enable();
        }
    }

    void OnDisable()
    {
        if(inputActions != null) {
            inputActions.Disable();
        } 
    }

    void Update()
    {
        float inputX = inputActions.Gameplay.Move.ReadValue<Vector2>().x;
        float jumpInput = inputActions.Gameplay.Jump.ReadValue<float>();
        float sunAuraEffectInput = inputActions.Gameplay.SunGlowAction.ReadValue<float>();
        float moonAuraEffectInput = inputActions.Gameplay.MoonGlowAction.ReadValue<float>();


        _player.SetDirectionalInput(inputX);
        _player.SetJumpInput(jumpInput);

        if(sunAuraEffectInput > 0 && _glowEffectManager != null) {
            _glowEffectManager.StartGlow(GlowEffectManager.GlowType.Sun);
        }
        if(moonAuraEffectInput > 0 && _glowEffectManager != null) {
            _glowEffectManager.StartGlow(GlowEffectManager.GlowType.Moon);
        }
    }
}
