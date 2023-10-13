using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private GameObject platformObject;
    [SerializeField]
    private PlatformData thisPlatformData;

    private GlowEffectManager _lastGlowEffectInteracted;
    private GlowEffectManager _lastPlayerGlowEffectInteracted = null;

    void Start()
    {
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
            UpdatePlatformCollider(true);
            platformObject.layer = LayerMask.NameToLayer("Active Platforms");
        }
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Moon) {
            UpdatePlatformStatus(false);
            platformObject.layer = LayerMask.NameToLayer("Disabled Platforms");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GlowEffectManager.GlowType colliderGlowType;
        if(other.gameObject.CompareTag("GlowEffect")) {
            if(other.transform.parent.gameObject.CompareTag("Player")) {
                _lastPlayerGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                colliderGlowType = _lastPlayerGlowEffectInteracted.GetGlowType();
                OnEnterGlowBehaviour(colliderGlowType);
            }
            else {
                _lastGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                if(_lastPlayerGlowEffectInteracted == null) {
                    colliderGlowType = _lastGlowEffectInteracted.GetGlowType();
                    OnEnterGlowBehaviour(colliderGlowType);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("GlowEffect")) {
            GlowEffectManager.GlowType _playerGlowType = GlowEffectManager.GlowType.Null;
            GlowEffectManager.GlowType _objectGlowType = GlowEffectManager.GlowType.Null;

            if(other.transform.parent.gameObject.CompareTag("Player")) {
                _playerGlowType = _lastPlayerGlowEffectInteracted.GetGlowType();
                _lastPlayerGlowEffectInteracted = null;
            }
            else {
                _objectGlowType = _lastGlowEffectInteracted.GetGlowType();
                _lastGlowEffectInteracted = null;
            }

            
            if(_lastPlayerGlowEffectInteracted == null && _lastGlowEffectInteracted != null) {
                if((int)_lastGlowEffectInteracted.GetGlowType() != (int)thisPlatformData.thisPlatformInteractionType) {
                    OnExitGlowBehaviour();
                }
            }
            else {
                OnExitGlowBehaviour();
            }
        }
    }
    public void UpdatePlatformStatus(bool newStatus) {
        platformObject.SetActive(newStatus);
    }

    public void UpdatePlatformCollider(bool newStatus) {
        platformObject.GetComponent<BoxCollider2D>().enabled = newStatus;

    }

    private void OnEnterGlowBehaviour(GlowEffectManager.GlowType colliderGlowType) {
        if(colliderGlowType == GlowEffectManager.GlowType.Sun && 
            thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
            UpdatePlatformCollider(false);
            platformObject.layer = LayerMask.NameToLayer("Disabled Platforms");
        }
        if(colliderGlowType == GlowEffectManager.GlowType.Moon && 
            thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Moon) {
            UpdatePlatformStatus(true);
            platformObject.layer = LayerMask.NameToLayer("Active Platforms");
        }
    }

    private void OnExitGlowBehaviour() {
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
            UpdatePlatformCollider(true);
            platformObject.layer = LayerMask.NameToLayer("Active Platforms");
        }
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Moon) {
            UpdatePlatformStatus(false);
            platformObject.layer = LayerMask.NameToLayer("Disabled Platforms");
        }
    }
}

[System.Serializable]
public class PlatformData {
    public enum PlatformInteractionBehaviour {Null, Sun, Moon, Twilight};
    public PlatformInteractionBehaviour thisPlatformInteractionType;
}
