using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    #region Component References

    private Controller2D _controller;

    #endregion

    #region Physics Values
    private float _gravity;
    private float _jumpVelocity;
    private Vector3 _velocity;
    private float _smoothXVelocity;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float timeToJumpApex;
    [SerializeField]
    private float groundAcceleration;
    [SerializeField]
    private float airbornedAcceleration;
    #endregion

    #region Unity Methods
    private void Start()
    {
        if(_controller == null) {
            _controller = GetComponent<Controller2D>();
        }
        _gravity = -(2 * jumpHeight/ Mathf.Pow(timeToJumpApex,2));
        _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
    }

    private void FixedUpdate()
    {
        if(_controller.Collisions.above || _controller.Collisions.below) {
            _velocity.y = 0;
        }
        if(Input.GetAxisRaw("Jump") != 0 && _controller.Collisions.below) {
            Jump();
        }
        PlayerMovement();
    }

    #endregion

    #region Movement Methods

    private void PlayerMovement() {
        float inputX = Input.GetAxis("Horizontal");

        float targetXVelocity = inputX * speed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetXVelocity, ref _smoothXVelocity, _controller.Collisions.below ? groundAcceleration : airbornedAcceleration);
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Jump() {
        _velocity.y = _jumpVelocity;
    }

    #endregion
}
