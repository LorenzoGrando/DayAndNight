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
            colliderGlowType = other.GetComponentInParent<GlowEffectManager>().GetGlowType();
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
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GlowEffectManager.GlowType colliderGlowType;
        if(other.gameObject.CompareTag("GlowEffect")) {
            colliderGlowType = other.GetComponentInParent<GlowEffectManager>().GetGlowType();
            if(colliderGlowType == GlowEffectManager.GlowType.Sun && 
                    thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Sun) {
                UpdatePlatformCollider(true);
                platformObject.layer = LayerMask.NameToLayer("Active Platforms");
            }
            if(colliderGlowType == GlowEffectManager.GlowType.Moon && 
                    thisPlatformData.thisPlatformInteractionType == PlatformData.PlatformInteractionBehaviour.Moon) {
                UpdatePlatformStatus(false);
                platformObject.layer = LayerMask.NameToLayer("Disabled Platforms");
            }
        }
    }
    public void UpdatePlatformStatus(bool newStatus) {
        platformObject.SetActive(newStatus);
    }

    public void UpdatePlatformCollider(bool newStatus) {
        platformObject.GetComponent<BoxCollider2D>().enabled = newStatus;

    }
}

[System.Serializable]
public class PlatformData {
    public enum PlatformInteractionBehaviour {Null, Sun, Moon, Twilight};
    public PlatformInteractionBehaviour thisPlatformInteractionType;
}
