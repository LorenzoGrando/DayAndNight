using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player _player;
    void Start()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float jumpInput = Input.GetAxisRaw("Jump");

        _player.SetDirectionalInput(inputX);
        _player.SetJumpInput(jumpInput);
    }
}
