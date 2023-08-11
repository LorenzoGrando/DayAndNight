using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region Required Components

    private Rigidbody2D _playerRB;

    #endregion

    #region Physics Values

    [SerializeField]
    private float speed;
    [SerializeField] 
    private float jumpSpeed;

    private Vector2 _thisFrameDirection;

    [SerializeField]
    private LayerMask groundLayers;

    #endregion

    private void Start()
    {
        if(_playerRB == null) {
            _playerRB = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        ReadAxisInput();
        if(Input.GetAxisRaw("Jump") != 0) {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }
    private void ReadAxisInput() {
        _thisFrameDirection.x = Input.GetAxis("Horizontal");
    }
    private void ApplyMovement() {
        Vector2 velocity = new(_thisFrameDirection.x * speed, _thisFrameDirection.y);
        _playerRB.velocity = velocity;
    }

    private void CheckIfGrounded() {

    }

    private void Jump() {
        _playerRB.AddForce(Vector2.up * jumpSpeed);
    }
}
