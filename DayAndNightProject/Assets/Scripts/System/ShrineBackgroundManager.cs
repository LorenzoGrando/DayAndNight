using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBackgroundManager : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer backgroundArea;
    [SerializeField]
    MeshRenderer leftTransitionArea;
    [SerializeField]
    MeshRenderer rightTransitionArea;
    public GlowEffectManager.GlowType thisBackgroundType;
    [SerializeField]
    private Sprite[] backgroundSprites;
    [SerializeField]
    private Texture2D[] transitionTextures;

    public void EnableBackground() {
        backgroundArea.gameObject.SetActive(true);
        backgroundArea.gameObject.SetActive(true);
    }

    public void ChangeBackgroundType(GlowEffectManager.GlowType newType) {
        thisBackgroundType = newType;
        UpdateBackgrounds();
    }

    private void UpdateBackgrounds() {
        switch(thisBackgroundType) {
            case GlowEffectManager.GlowType.Sun:
                backgroundArea.sprite = backgroundSprites[0];

            break;
            case GlowEffectManager.GlowType.Moon:
                backgroundArea.sprite = backgroundSprites[1];
            break;
        }

        UpdateTransitionAreaTextures();
    }

    private void UpdateTransitionAreaTextures() {
        int index = thisBackgroundType == GlowEffectManager.GlowType.Sun ? 0 : 1;

        if(leftTransitionArea != null) {
            Material material = leftTransitionArea.material;
            material.SetTexture("RightTex", transitionTextures[index]);
        }

        if(rightTransitionArea != null) {
            Material material = leftTransitionArea.material;
            material.SetTexture("LeftTex", transitionTextures[index]);
        }
    }
}
