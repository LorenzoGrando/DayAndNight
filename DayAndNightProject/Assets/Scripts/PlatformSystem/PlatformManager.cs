using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private bool printDebugStatus;
    [SerializeField]
    private GameObject platformObject;
    [SerializeField]
    private PlatformData thisPlatformData;
    private GameObject sunSpriteObject;
    private Collider2D sunSpriteObjectCollider;
    public List<GlowEffectManager> totemGlows;

    private float _timeSinceLastCollisionWithGlow;
    private float _intervalToWait = 5;
    [SerializeField]
    private bool _isActivelyColliding;

    private GlowEffectManager _lastGlowEffectInteracted;
    private GlowEffectManager _lastPlayerGlowEffectInteracted = null;

    void Start()
    {
        ResetPlatformsToDefaultState();
        totemGlows = new List<GlowEffectManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GlowEffectManager.GlowType colliderGlowType;
        if(other.gameObject.CompareTag("GlowEffect")) {
            _isActivelyColliding = true;
            _timeSinceLastCollisionWithGlow = Time.time;
            if(other.transform.parent.gameObject.CompareTag("Player")) {
                _lastPlayerGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                colliderGlowType = _lastPlayerGlowEffectInteracted.GetGlowType();
                OnEnterGlowBehaviour(colliderGlowType);
            }
            else {
                _lastGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                if(!totemGlows.Contains(_lastGlowEffectInteracted)) {
                    totemGlows.Add(_lastGlowEffectInteracted);
                }
                    colliderGlowType = _lastGlowEffectInteracted.GetGlowType();
                    OnEnterGlowBehaviour(colliderGlowType);
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("GlowEffect")) {
            GlowEffectManager glow = null;
            if(other.transform.parent.gameObject.CompareTag("Player")) {
                _lastPlayerGlowEffectInteracted = null;
            }
            else {
                _lastGlowEffectInteracted = null;
                glow = other.GetComponentInParent<GlowEffectManager>(); 
            }           
            if(_lastPlayerGlowEffectInteracted == null && _lastGlowEffectInteracted != null) {
                if((int)_lastGlowEffectInteracted.GetGlowType() != (int)thisPlatformData.thisPlatformInteractionType) {
                    OnExitGlowBehaviour(glow);
                }
            }
            else {
                OnExitGlowBehaviour(glow);
            }
        }
    }
    public void UpdatePlatformStatus(bool newStatus) {
        platformObject.SetActive(newStatus);
        if(newStatus) {
            platformObject.layer = LayerMask.NameToLayer("Active Platforms");
        }
        else {
            platformObject.layer = LayerMask.NameToLayer("Disabled Platforms");
        }
    }

    public void UpdatePlatformCollider(bool newStatus) {
        platformObject.GetComponent<BoxCollider2D>().enabled = newStatus;
    }

    private void OnEnterGlowBehaviour(GlowEffectManager.GlowType colliderGlowType) {
        if(colliderGlowType == GlowEffectManager.GlowType.Sun && 
            thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
            UpdatePlatformCollider(false);
            sunSpriteObject.SetActive(false);
            platformObject.layer = LayerMask.NameToLayer("Disabled Platforms");
        }
        if(colliderGlowType == GlowEffectManager.GlowType.Moon && 
            thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Moon) {
            UpdatePlatformStatus(true);
            
        }
    }

    private void OnExitGlowBehaviour(GlowEffectManager glow) {
        Debug.Log("Call tracking debug");
        _isActivelyColliding = false;
        if(glow != null) {
            if(totemGlows.Contains(glow)) {
                totemGlows.Remove(glow);
                if(totemGlows.Count > 0) {
                    return;
                }
            }
        }
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
            UpdatePlatformCollider(true);
            sunSpriteObject.SetActive(true);
            platformObject.layer = LayerMask.NameToLayer("Active Platforms");
        }
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Moon) {
            UpdatePlatformStatus(false);
        }
    }

    private void ResetPlatformsToDefaultState() {
        if(printDebugStatus) {
            Debug.Log("Reset Platform to Default");
        }
        if(thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
            UpdatePlatformCollider(true);
            platformObject.layer = LayerMask.NameToLayer("Active Platforms");
            sunSpriteObject = platformObject.transform.GetChild(0).gameObject;
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
