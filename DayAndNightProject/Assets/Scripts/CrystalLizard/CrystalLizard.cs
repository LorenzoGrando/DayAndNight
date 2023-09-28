using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLizard : MonoBehaviour {
    public StateMachine behaviourStateMachine;
    private GlowEffectManager _lastGlowEffectInteracted;

    void  OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("GlowEffect")) {
            _lastGlowEffectInteracted = other.GetComponent<GlowEffectManager>();
            OnGlowContact(_lastGlowEffectInteracted);
        }
    }

    private void OnGlowContact(GlowEffectManager glowEffectReference) {

    }
}