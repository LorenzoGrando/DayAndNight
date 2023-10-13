using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private float _lastActivateTime;
    [SerializeField]
    private float _activationDelay = 0.25f; 
    [SerializeField]
    public IInteractable _lastKnownInteractable {get; private set;} = null;
    private GlowEffectManager.GlowType _lastGlowTypeInput;

    public void UpdateLastInteractable(IInteractable newInteractable) {
        _lastKnownInteractable = newInteractable;
    }

    public IInteractable GetLastInteractable() {
        return _lastKnownInteractable;
    }

    public void OnActivateInputPressed() {
        _lastKnownInteractable?.OnInteractorActivate();
    }

    public void OnActiveGlowInputPressed(GlowEffectManager.GlowType glowInput) {
        _lastKnownInteractable?.OnInteractorGlowActivate(glowInput);
    }

    public void SetInteractionValue(float interactValue) {
        if(interactValue > 0) {
            if(_lastActivateTime + _activationDelay < Time.time) {
                _lastActivateTime = Time.time;
                OnActivateInputPressed();
            }
        }
    }
}
