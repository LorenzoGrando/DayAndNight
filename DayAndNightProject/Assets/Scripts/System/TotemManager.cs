using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    [SerializeField]
    private GlowEffectManager[] glowEffects;

    void OnEnable()
    {
        glowEffects = FindObjectsOfType<GlowEffectManager>();
    }    

    public void DisableAllGlows() {
        foreach(GlowEffectManager glow in glowEffects) {
            if(glow != null) {
                glow.ForceFadeOutGlow();
            }
        }
    }
}
