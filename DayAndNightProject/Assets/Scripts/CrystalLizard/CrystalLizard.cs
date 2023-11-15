using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class CrystalLizard : MonoBehaviour {
    private Controller2D _controller;
    private Animator _animator;
    private SpriteRenderer _renderer;
    [SerializeField]
    [Tooltip("State Machine")]
    public StateMachine behaviourStateMachine = null;
    private GlowEffectManager _lastGlowEffectInteracted;
    private GlowEffectManager _lastPlayerGlowEffectInteracted = null;
    private Vector2 _velocity = new Vector2(0,-3f);
    [SerializeField]
    private float speed;
    private float _lastTimeActive;
    private float _sleepDelay = 2.5f;

    #region Unity Methods

    void OnEnable()
    {
        behaviourStateMachine ??= new StateMachine();
        _controller = _controller != null ? _controller : GetComponent<Controller2D>();
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        ExecuteBehaviour();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("GlowEffect")) {
            if(other.transform.parent.gameObject.CompareTag("Player")) {
                _lastPlayerGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                OnGlowContact(_lastPlayerGlowEffectInteracted);
            }
            else {
                _lastGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                if(_lastPlayerGlowEffectInteracted == null) {
                    OnGlowContact(_lastGlowEffectInteracted);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("GlowEffect")) {
            if(other.transform.parent.gameObject.CompareTag("Player")) {
                _lastPlayerGlowEffectInteracted = null;
            }
            else {
                _lastGlowEffectInteracted = null;
            }
            OnGlowExit();
        }
    }

    #endregion

    #region Glow Effect Methods
    private void OnGlowContact(GlowEffectManager glowEffectReference) {
        GlowEffectManager.GlowType incomingGlowType = glowEffectReference.GetGlowType();
        UpdateStateMachine(incomingGlowType);
    }

    private void OnGlowExit() {
        if(_lastPlayerGlowEffectInteracted == null && _lastGlowEffectInteracted != null) {
            OnGlowContact(_lastGlowEffectInteracted);
        }
        else {
            _lastTimeActive = Time.time;
            behaviourStateMachine.DirectlyChangeState(StateMachine.State.Idle);
        }
    }

    private void UpdateStateMachine(GlowEffectManager.GlowType newGlowBehaviour) {
        behaviourStateMachine.ChangeStateBasedOnGlow(newGlowBehaviour);
    }

    private void ExecuteBehaviour() {
        switch (behaviourStateMachine.CurrentState) {
            case StateMachine.State.Idle:
                IdleBehaviour();
            break;
            case StateMachine.State.Escaping:
                EscapingBehaviour();
            break;
            case StateMachine.State.Approaching:
                ApproachingBehaviour();
            break;
            case StateMachine.State.Sleeping:
                SleepingBehaviour();
            break;
        }
    }

    private void EscapingBehaviour() {
        bool isPlayerGlow = _lastPlayerGlowEffectInteracted == null ? false : true;
        GlowEffectManager trackedGlow = isPlayerGlow ? _lastPlayerGlowEffectInteracted : _lastGlowEffectInteracted;
        if((!isPlayerGlow && _lastGlowEffectInteracted != null) || isPlayerGlow) {
            Vector3 glowWorldPosition = trackedGlow.gameObject.transform.position;
            Vector2 spaceTowardsGlowCenter = glowWorldPosition - transform.position;
            Vector2 directionTowardsGlowCenter = spaceTowardsGlowCenter.normalized;
            _animator.SetBool("Walking", true);
            MoveLizard(-directionTowardsGlowCenter.x);
        }
        else {
            behaviourStateMachine.ChangeStateBasedOnGlow(GlowEffectManager.GlowType.Null);
        }
    }

    private void ApproachingBehaviour() {
        bool isPlayerGlow = _lastPlayerGlowEffectInteracted == null ? false : true;
        GlowEffectManager trackedGlow = isPlayerGlow ? _lastPlayerGlowEffectInteracted : _lastGlowEffectInteracted;
        if((!isPlayerGlow && _lastGlowEffectInteracted != null) || isPlayerGlow) {
            Vector3 glowWorldPosition = trackedGlow.gameObject.transform.position;

            if(transform.position != glowWorldPosition) {
                Vector2 spaceTowardsGlowCenter = glowWorldPosition - transform.position;
                Vector2 directionTowardsGlowCenter = spaceTowardsGlowCenter.normalized;


                if((Mathf.Sign(_velocity.x) == directionTowardsGlowCenter.x) && 
                    (Mathf.Abs(_velocity.x) > Mathf.Abs(spaceTowardsGlowCenter.x))) {

                    float optionalSpeed = _velocity.x - spaceTowardsGlowCenter.x;
                    MoveLizard(directionTowardsGlowCenter.x, optionalSpeed, true);
                    return;
                }
                _animator.SetBool("Walking", true);
                MoveLizard(directionTowardsGlowCenter.x);
            }

        }
        else {
            behaviourStateMachine.ChangeStateBasedOnGlow(GlowEffectManager.GlowType.Null);
        }
    }

    private void IdleBehaviour() {
        UpdateDirection(0);
        MoveLizard(0);
        _animator.SetBool("Walking", false);
        if(Time.time > _lastTimeActive + _sleepDelay) {
            behaviourStateMachine.DirectlyChangeState(StateMachine.State.Sleeping);
        }
    }

    private void SleepingBehaviour() {
        UpdateDirection(0);
        MoveLizard(0);
    }

    #endregion

    #region Movement Methods

    void MoveLizard(float xDirection, float optionalSpeed = 0, bool useOptionalSpeed = false) {
        float finalSpeed = useOptionalSpeed ? optionalSpeed : speed;
        _velocity.x = xDirection * finalSpeed;
        UpdateDirection(xDirection);
        _controller.Move(_velocity * Time.deltaTime);
    }

    public Vector2 GetVelocity() {
        return _velocity;
    }

    private void UpdateDirection(float direction) {
        bool isFaceLeft = direction < 0;
        bool isMaintainSide = direction == 0;
        if(!isMaintainSide) {
            _renderer.flipX = isFaceLeft;
        }
    }

    #endregion
}