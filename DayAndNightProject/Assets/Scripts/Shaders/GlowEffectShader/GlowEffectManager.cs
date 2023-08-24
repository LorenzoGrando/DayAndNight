using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[ExecuteInEditMode]
public class GlowEffectManager : MonoBehaviour
{
    public Material effectMat;
    public TextureCameraRender textureCamera;
    public float maskSubtractionValue;
    public float maskMultiplicationValue;
    public float maskLerpMin;
    public float maskLerpMax;
    public float thresholdValue;
    public float waveSpeed;
    [Range(0,100)]
    public int waveAmount;

    public float outlineWidth;
    
    public float animDuration;
    public float delayBeforeFadeOut;
    Sequence currentTweenSequence;
    void Start()
    {
        if(effectMat == null || textureCamera == null) {
            Debug.LogWarning("Assign all references while in Editor");
            return;
        }
        effectMat.SetFloat("_MaskSubtractionValue", maskSubtractionValue);
        effectMat.SetFloat("_MaskMultiplicationValue", maskMultiplicationValue);
        effectMat.SetFloat("_MaskLerpMin", maskLerpMin);
        effectMat.SetFloat("_MaskLerpMax", maskLerpMax);
        effectMat.SetFloat("_ThresholdValue", thresholdValue);
        effectMat.SetFloat("_WaveSpeed", waveSpeed);
        effectMat.SetInt("_WaveAmount", waveAmount);
        effectMat.SetFloat("_OutlineWidth", outlineWidth);
    }
    void Update()
    {
        AssignMaterialProperties();
    }
    public void AssignMaterialProperties() {
        effectMat.SetTexture("_ScreenRenderTexture", textureCamera.GetRenderTexture());
    }

    public void StartGlow() {
        currentTweenSequence = DOTween.Sequence();
        currentTweenSequence.Append(transform.DOScale(Vector3.one, animDuration));
        currentTweenSequence.AppendInterval(delayBeforeFadeOut);
        currentTweenSequence.Append(transform.DOScale(Vector3.zero, animDuration));
    }
}
