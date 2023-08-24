using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player _player;
    private GlowEffectManager _glowEffectManager;
    void Start()
    {
        _player = GetComponent<Player>();
        if(transform.childCount > 0) {
            _glowEffectManager = GetComponentInChildren<GlowEffectManager>();
        }
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float jumpInput = Input.GetAxisRaw("Jump");
        float auraEffectInput = Input.GetAxisRaw("Ability");

        _player.SetDirectionalInput(inputX);
        _player.SetJumpInput(jumpInput);

        if(auraEffectInput > 0 && _glowEffectManager != null) {
            _glowEffectManager.StartGlow();
        }
    }
}
