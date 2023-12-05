using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialTree : MonoBehaviour
{
    [SerializeField]
    public Shrine shrineToReact;
    [SerializeField]
    private GameObject glowWorldObject, realWorldObject;
    [SerializeField]
    private SpriteRenderer[] foliages;
    [SerializeField]
    private Sprite[] foliageSprites;

    private bool activatedByShrine = false;

    void OnEnable()
    {
        shrineToReact.OnShrineComplete += ActivateTree;
    }

    void OnDisable()
    {
        shrineToReact.OnShrineComplete -= ActivateTree;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!activatedByShrine) {
            if(other.gameObject.CompareTag("GlowEffect")) {
                GlowEffectManager _lastPlayerGlowEffectInteracted = other.GetComponentInParent<GlowEffectManager>();
                GlowEffectManager.GlowType colliderGlowType = _lastPlayerGlowEffectInteracted.GetGlowType();
                OnEnterGlowBehaviour(colliderGlowType);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!activatedByShrine) {
            if(other.gameObject.CompareTag("GlowEffect")) {
                OnExitGlowBehaviour();
            }
        }
    }

    private void OnEnterGlowBehaviour(GlowEffectManager.GlowType glowType) {
        ChangeFoliageAppearance(glowType);
        UpdateTreeVisibility(isReal: false, changeRenderers: false);
    }

    private void OnExitGlowBehaviour() {
        UpdateTreeVisibility(isReal: true, changeRenderers: false);
    }

    public void ActivateTree(Shrine.ShrineTypeStatus status) {
        GlowEffectManager.GlowType glowType = GlowEffectManager.GlowType.Null;
        switch(status) {
            case Shrine.ShrineTypeStatus.Sun:
                glowType = GlowEffectManager.GlowType.Sun;
                activatedByShrine = true;
            break;
            case Shrine.ShrineTypeStatus.Moon:
                glowType = GlowEffectManager.GlowType.Moon;
                activatedByShrine = true;
            break;
            case Shrine.ShrineTypeStatus.Uncomplete:
                glowType = GlowEffectManager.GlowType.Null;
            break;
        }
        ChangeFoliageAppearance(glowType);
        UpdateTreeVisibility(isReal: true, changeRenderers: activatedByShrine);
    }

    private void ChangeFoliageAppearance(GlowEffectManager.GlowType glowType) {
        int spriteIndex = glowType == GlowEffectManager.GlowType.Sun ? 0 : 1;
        spriteIndex = glowType == GlowEffectManager.GlowType.Null ? 2 : spriteIndex;
        Sprite foliageSprite = null;
        if(spriteIndex  <= 1)
           foliageSprite = foliageSprites[spriteIndex];
        int i = 0;
        foreach(SpriteRenderer spRenderer in foliages) {
            if(spriteIndex > 1) {
                if(i == 0) {
                    spRenderer.enabled = false;
                }
                continue;
            }
            else {
                if(i == 0) {
                    spRenderer.enabled = true;
                }
                spRenderer.sprite = foliageSprite;
            }
            i++;
        }
    }

    private void UpdateTreeVisibility(bool isReal, bool changeRenderers) {
        if(isReal) {
            glowWorldObject.SetActive(false);
            realWorldObject.SetActive(true);
            if(changeRenderers) {
                foliages[1].enabled = true;
                foliages[0].enabled = false;
            }
        }

        else {
            glowWorldObject.SetActive(true);
            realWorldObject.SetActive(false);
        }
        
    }
}
