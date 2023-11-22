using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBackgroundManager : MonoBehaviour
{
    [SerializeField]
    MeshRenderer[] backgroundArea;
    [SerializeField]
    MeshRenderer leftTransitionArea;
    [SerializeField]
    MeshRenderer rightTransitionArea;
    public GlowEffectManager.GlowType thisBackgroundType;
    [SerializeField]
    private Sprite[] backgroundSprites;
    [SerializeField]
    private Texture2D[] transitionTextures;

    public void OnEnable() {
        foreach(MeshRenderer bg in backgroundArea) {
            Material newInstancedMaterial = Instantiate(bg.material);
            bg.material = newInstancedMaterial;
        }
        
        Material leftMat = Instantiate(leftTransitionArea.material);
        leftTransitionArea.material = leftMat;

        Material rightMat = Instantiate(rightTransitionArea.material);
        rightTransitionArea.material = rightMat;
    }

    public void ChangeBackgroundType(GlowEffectManager.GlowType newType) {
        thisBackgroundType = newType;
        UpdateBackgrounds();
    }

    private void UpdateBackgrounds() {
        Debug.Log("CalledUpdate");
        switch(thisBackgroundType) {
            case GlowEffectManager.GlowType.Sun:
                foreach(MeshRenderer bg in backgroundArea) {
                    Material materialtest = bg.material;
                    materialtest.SetTexture("_RightTexture", transitionTextures[0]);
                    materialtest.SetTexture("_LeftTexture", transitionTextures[0]);
                }
                

            break;
            case GlowEffectManager.GlowType.Moon:
                foreach(MeshRenderer bg2 in backgroundArea) {
                    Material material2 = bg2.material;
                    material2.SetTexture("_RightTexture", transitionTextures[1]);
                    material2.SetTexture("_LeftTexture", transitionTextures[1]);
                }
            break;
        }

        UpdateTransitionAreaTextures();
    }

    private void UpdateTransitionAreaTextures() {
        int index = thisBackgroundType == GlowEffectManager.GlowType.Sun ? 0 : 1;

        if(leftTransitionArea != null) {
            Material material = leftTransitionArea.material;
            material.SetTexture("_RightTexture", transitionTextures[index]);
        }

        if(rightTransitionArea != null) {
            Material material = rightTransitionArea.material;
            material.SetTexture("_LeftTexture", transitionTextures[index]);
        }
    }
}
