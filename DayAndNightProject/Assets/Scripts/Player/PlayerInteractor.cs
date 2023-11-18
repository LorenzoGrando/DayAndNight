using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public PlayerDataManager playerDataManager;
    public GameObject sphereHeldPosition;
    private float _lastActivateTime;
    [SerializeField]
    private float _activationDelay = 0.25f; 
    [SerializeField]
    public IInteractable _lastKnownInteractable {get; private set;} = null;

    public void UpdateLastInteractable(IInteractable newInteractable) {
        _lastKnownInteractable = newInteractable;
    }

    public IInteractable GetLastInteractable() {
        return _lastKnownInteractable;
    }

    public void OnActivateInputPressed() {
        if(_lastKnownInteractable == null) {
            LeaveSphere();
        }
        else {
            _lastKnownInteractable?.OnInteractorActivate();
        }
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

    public void GetSphere(CrystalSphere sphere) {
        if(playerDataManager.currentPlayerData.currentHeldSphere == null) {
            playerDataManager.currentPlayerData.currentHeldSphere = sphere;
            sphere.UpdateSphereStatus(CrystalSphere.SphereStatus.Uninteractable);
            sphere.transform.parent = sphereHeldPosition.transform;
            sphere.transform.localPosition = Vector3.zero;
            FindObjectOfType<PlayerSoundManager>().PlaySphereSound();
        }
    }

    public void LeaveSphere() {
        CrystalSphere sphereRef = playerDataManager.currentPlayerData.currentHeldSphere;
        if(sphereRef != null) {
            sphereRef.transform.parent = null;
            sphereRef.UpdateSphereStatus(CrystalSphere.SphereStatus.Default);
            playerDataManager.currentPlayerData.currentHeldSphere = null;
        }
    }
}
