using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class CrystalSphere : MonoBehaviour, IInteractable {
    private Controller2D _controller;
    private Collider2D _collider;
    [SerializeField]
    private GameObject spriteObject;
    private PlayerInteractor _lastPlayerInteractor;
    private IInteractable interactable;

    public enum SphereStatus {Default, Uninteractable, Hidden}
    public SphereStatus currentSphereStatus {get; private set;}
    private Vector2 velocity = new Vector2(0, -2f);
    private bool canMove = true;

    void Awake()
    {
        _controller = GetComponent<Controller2D>();
        _collider = GetComponent<Collider2D>();
        interactable = this;
    }

    void LateUpdate()
    {
        MoveSphere(Mathf.Sign(velocity.x), Mathf.Abs(velocity.x));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorEnter(other.gameObject);
        }
        if(other.gameObject.CompareTag("CrystalLizard")) {
            Debug.Log("Collided with Lizard");
            CollideWithLizard(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorExit(other.gameObject);
        }
        if(other.gameObject.CompareTag("CrystalLizard")) {
            velocity.x = 0;
        }
    }


    void IInteractable.OnInteractorEnter(GameObject newInteractor) {
        _lastPlayerInteractor = newInteractor.GetComponent<PlayerInteractor>();
        _lastPlayerInteractor.UpdateLastInteractable(interactable);
    }

    void IInteractable.OnInteractorExit(GameObject oldInteractor) {
        if(_lastPlayerInteractor != null) {
            if(_lastPlayerInteractor.GetLastInteractable() == interactable) {
                _lastPlayerInteractor.UpdateLastInteractable(null);
                _lastPlayerInteractor = null;
            }
        }
    }

    void IInteractable.OnInteractorActivate() {
        _lastPlayerInteractor.GetSphere(this);
    }

    void IInteractable.OnInteractorGlowActivate(GlowEffectManager.GlowType glowType) {
    }

    void MoveSphere(float xDirection, float moveSpeed) {
        if(canMove) {
            Debug.Log(xDirection);
            velocity.x = xDirection * (moveSpeed);
            _controller.Move(velocity * Time.deltaTime);
        }
    }

    void CollideWithLizard(GameObject lizardObject) {
        CrystalLizard lizardReference = lizardObject.GetComponent<CrystalLizard>();
        float lizardXVelocity = lizardReference.GetVelocity().x;
        MoveSphere(Mathf.Sign(lizardXVelocity), Mathf.Abs(lizardXVelocity) + .2f);
    }

    public void UpdateSphereStatus(SphereStatus newStatus) {
        switch (newStatus) {
            case SphereStatus.Default:
                spriteObject.SetActive(true);
                _collider.enabled = true;
                _controller.enabled = true;
                canMove = true;
            break;
            case SphereStatus.Uninteractable:
                spriteObject.SetActive(true);
                _collider.enabled = false;
                _controller.enabled = false;
                canMove = false;
            break;
            case SphereStatus.Hidden:
                spriteObject.SetActive(false);
                _collider.enabled = false;
                _controller.enabled = false;
                canMove = false;
            break;
        }
    }
}