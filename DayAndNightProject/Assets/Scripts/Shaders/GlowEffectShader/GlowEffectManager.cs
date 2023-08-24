using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlowEffectManager : MonoBehaviour
{
    public Material effectMat;
    public TextureCameraRender textureCamera;
    
    void Update()
    {
        if(effectMat == null || textureCamera == null) {
            Debug.LogWarning("Assign all references while in Editor");
            return;
        }
        AssignMaterialProperties();
    }
    public void AssignMaterialProperties() {
        if(textureCamera != null) {
            effectMat.SetTexture("_ScreenRenderTexture", textureCamera.GetRenderTexture());
        }
    }
}
