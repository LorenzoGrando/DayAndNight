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
    private CircleCollider2D _areaTriggerCollider2D;

    [SerializeField]
    [Tooltip("Sun Mask Data")]
    public MaskData sunMaskData;
    [SerializeField]
    [Tooltip("Moon Mask Data")]
    public MaskData moonMaskData;


    public enum GlowType {Null, Sun, Moon, Twilight};
    public GlowType thisGlowType;
    public float animDuration;
    public float delayBeforeFadeOut;
    private float currentTargetScale;
    Sequence currentTweenSequence;
    private bool isActive = false;

    [System.Serializable]
    public class MaskData {
        public float targetScaleSize;
        public float maskSubtractionValue;
        public float maskMultiplicationValue;
        public float maskLerpMin;
        public float maskLerpMax;
        public float thresholdValue;
        public float waveSpeed;
        [Range(0,100)]
        public int waveAmount;
        public float outlineWidth;
        public Color temporaryOverlayColor;
    }
    void Start()
    {
        if(effectMat == null || textureCamera == null) {
            Debug.LogWarning("Assign all references while in Editor");
            return;
        }
        if(_areaTriggerCollider2D == null) {
            _areaTriggerCollider2D = GetComponentInChildren<CircleCollider2D>();
        }
    }
    void Update()
    {
        effectMat.SetTexture("_ScreenRenderTexture", textureCamera.GetRenderTexture());

        if(transform.localScale.x <= 0.12f) {
            _areaTriggerCollider2D.enabled = false;
        }
        else {
            _areaTriggerCollider2D.enabled = true;
        }
    }
    public void AssignMaterialProperties(MaskData currentMaskData) {
        currentTargetScale = currentMaskData.targetScaleSize;

        effectMat.SetFloat("_MaskSubtractionValue", currentMaskData.maskSubtractionValue);
        effectMat.SetFloat("_MaskMultiplicationValue", currentMaskData.maskMultiplicationValue);
        effectMat.SetFloat("_MaskLerpMin", currentMaskData.maskLerpMin);
        effectMat.SetFloat("_MaskLerpMax", currentMaskData.maskLerpMax);
        effectMat.SetFloat("_ThresholdValue", currentMaskData.thresholdValue);
        effectMat.SetFloat("_WaveSpeed", currentMaskData.waveSpeed);
        effectMat.SetInt("_WaveAmount", currentMaskData.waveAmount);
        effectMat.SetFloat("_OutlineWidth", currentMaskData.outlineWidth);
        effectMat.SetColor("_TemporaryColor", currentMaskData.temporaryOverlayColor);
    }

    public void StartGlow(GlowType glowType) {
        thisGlowType = glowType;
        if(!isActive) {
            if(glowType == GlowType.Sun) {
                AssignMaterialProperties(sunMaskData);
            }
            else if (glowType == GlowType.Moon) {
                AssignMaterialProperties(moonMaskData);
            }
            isActive = true;

            currentTweenSequence = DOTween.Sequence();
            currentTweenSequence.Append(transform.DOScale(Vector3.one * currentTargetScale, animDuration));
            currentTweenSequence.AppendInterval(delayBeforeFadeOut);
            currentTweenSequence.Append(transform.DOScale(Vector3.zero, animDuration));
            currentTweenSequence.OnComplete(() => EndGlowEffect());
        }
    }

    private void EndGlowEffect() {
        isActive = false;
    }

    public GlowType GetGlowType() {
        return thisGlowType;
    }
}
