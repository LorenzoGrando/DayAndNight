using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public event Action<float> OnPlayerDirectionalInput;
    private PlayerAnimationManager _animManager;

    public bool isWalking = false;
    public bool isJumping = false;


    #region Component References
    private Controller2D _controller;

    #endregion

    #region Physics Values
    private float _gravity;
    private float _maxJumpVelocity;
    private float _minJumpVelocity;
    private Vector2 _velocity;
    private float _smoothXVelocity;
    private float _lastJumpInputTime = Mathf.NegativeInfinity;
    private float _directionalInput;
    private float _jumpInput;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxJumpHeight;
    [SerializeField]
    private float minJumpHeight;
    [SerializeField]
    private float timeToJumpApex;
    [SerializeField]
    private float groundAcceleration;
    [SerializeField]
    private float airborneAcceleration;
    [SerializeField]
    private float jumpBufferTime;
    [SerializeField]
    private float coyoteTimeThreshold;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        if(_controller == null) {
            _controller = GetComponent<Controller2D>();
        }
        _animManager = FindObjectOfType<PlayerAnimationManager>();
        _gravity = -(2 * maxJumpHeight/ Mathf.Pow(timeToJumpApex,2));
        _maxJumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
        _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravity) * minJumpHeight);
    }

    private void Update()
    {
        if(_controller.Collisions.above || _controller.Collisions.below) {
            _velocity.y = 0;
        }

        bool bufferJump = Time.time < _lastJumpInputTime + jumpBufferTime;
        bool applyCoyoteTime = Time.time < _controller.Collisions.lastTimeOnGround + coyoteTimeThreshold;

        if(_jumpInput != 0) {
            _lastJumpInputTime = Time.time;
        }

        if(bufferJump && applyCoyoteTime) {
            Jump();
        }

        if(_jumpInput == 0 && !_controller.Collisions.below) {
            if(_velocity.y > _minJumpVelocity) {
                _velocity.y = _minJumpVelocity;
            }
        }

        PlayerMovement();
    }

    #endregion

    #region Movement Methods

    public void SetDirectionalInput(float input) {
        _directionalInput = input;
        OnPlayerDirectionalInput?.Invoke(_directionalInput);
    }
    public void SetJumpInput(float input) {
        _jumpInput = input;
    }
    private void PlayerMovement() {
        float targetXVelocity = _directionalInput * speed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetXVelocity, ref _smoothXVelocity, _controller.Collisions.below ? groundAcceleration : airborneAcceleration);
        _velocity.y += _gravity * Time.deltaTime;

        bool walking = _directionalInput != 0;
        int grounded = _controller.Collisions.below ? 1 : 0;
        _animManager.UpdateAnimatorParameters(walking, grounded);
        

        _controller.Move(_velocity * Time.deltaTime);

        float absVelocityX = Mathf.Abs(_velocity.x);

        if(absVelocityX < 0.01 || !_controller.Collisions.below) {
            FindObjectOfType<PlayerSoundManager>().StopWalkingSound();
        }

        if(absVelocityX > 0 && _controller.Collisions.below) {
            FindObjectOfType<PlayerSoundManager>().StartMovementSound();
        }
    }

    private void Jump() {
        FindObjectOfType<PlayerSoundManager>().JumpSound();
        _animManager.TriggerJump();
        _velocity.y = _maxJumpVelocity;
    }

    #endregion
}
