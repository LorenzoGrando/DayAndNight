using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineInteractable : MonoBehaviour, IInteractable
{
    private Collider2D _collider;
    private PlayerInteractor _lastPlayerInteractor;
    private IInteractable interactable;
    [SerializeField]
    private Shrine thisActivatorShine;
    public Shrine.ShrineTypeStatus ThisActivatorType;
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        interactable = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorEnter(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorExit(other.gameObject);
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
        if(thisActivatorShine != null) {
            thisActivatorShine.ActivateShrine(ThisActivatorType);
            FindObjectOfType<TotemManager>().DisableAllGlows();
        }
    }

    void IInteractable.OnInteractorGlowActivate(GlowEffectManager.GlowType glowType) {
    }
}
