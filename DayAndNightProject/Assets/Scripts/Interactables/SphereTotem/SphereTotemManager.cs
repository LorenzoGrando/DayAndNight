using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTotemManager : MonoBehaviour, IInteractable
{
    private GlowEffectManager _totemGlowEffect;
    private PlayerInteractor _lastPlayerInteractor;
    private IInteractable interactable;
    private Collider2D _collider;
    private bool _hasSphere = false;

    void OnEnable() {
        _totemGlowEffect = _totemGlowEffect != null ? _totemGlowEffect : GetComponentInChildren<GlowEffectManager>();
        _collider = GetComponent<Collider2D>();
        interactable = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorEnter(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorExit(other.gameObject);
        }
    }

    void IInteractable.OnInteractorEnter(GameObject interactorObject) {
        _lastPlayerInteractor = interactorObject.GetComponent<PlayerInteractor>();
        _lastPlayerInteractor.UpdateLastInteractable(interactable);
    }

    void IInteractable.OnInteractorExit(GameObject oldInteractorObject) {
        if(_lastPlayerInteractor != null) {
            if(Vector3.Distance(transform.position, _lastPlayerInteractor.transform.position) > 0.5f) {
                if(_lastPlayerInteractor.GetLastInteractable() == interactable) {
                    _lastPlayerInteractor.UpdateLastInteractable(null);
                    _lastPlayerInteractor = null;
                }
            }
        }
    }

    void IInteractable.OnInteractorActivate() {
        if(_hasSphere == true) {
            _hasSphere = false;
        }
        else {
            _totemGlowEffect.ForceFadeOutGlow();
            _hasSphere = true;
        }
    }

    void IInteractable.OnInteractorGlowActivate(GlowEffectManager.GlowType glowType) {
        if(_totemGlowEffect.GetGlowType() != glowType) {
            _totemGlowEffect.EndGlowEffect();
            _totemGlowEffect.StartGlow(glowType, false);
        }
    }
}
