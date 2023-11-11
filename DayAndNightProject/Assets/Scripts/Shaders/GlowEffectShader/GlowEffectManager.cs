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
    [SerializeField]
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
    private float[] defaultMaskSize;
    private float defaultFadeOutDelay;

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
        public Texture2D glowTexture;
    }
    void Awake()
    {
        if(effectMat == null || textureCamera == null) {
            Debug.LogWarning("Assign all references while in Editor");
            return;
        }
        if(_areaTriggerCollider2D == null) {
            _areaTriggerCollider2D = GetComponentInChildren<CircleCollider2D>();
        }
        if(effectMat != null) {
            Material newInstancedMaterial = Instantiate(effectMat);
            effectMat = newInstancedMaterial;
            GetComponentInChildren<MeshRenderer>().material = effectMat;
        }
        thisGlowType = GlowType.Null;
        defaultMaskSize = new float[2];
        defaultMaskSize[0] = sunMaskData.targetScaleSize;
        defaultMaskSize[1] = moonMaskData.targetScaleSize;
        defaultFadeOutDelay = delayBeforeFadeOut;
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
        effectMat.SetTexture("_BaseMap", currentMaskData.glowTexture);
    }

    public void StartGlow(GlowType glowType, bool fadeOut) {
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
            if(fadeOut) {
                currentTweenSequence.Append(transform.DOScale(Vector3.zero, animDuration));
                currentTweenSequence.OnComplete(() => EndGlowEffect());
            }
        }
    }

    public void ForceFadeOutGlow() {
        if(isActive) {
            currentTweenSequence?.Kill();
            currentTweenSequence = DOTween.Sequence();
            currentTweenSequence.Append(transform.DOScale(Vector3.zero, animDuration));
            currentTweenSequence.OnComplete(() => EndGlowEffect());
        }
    }

    public void EndGlowEffect() {
        transform.localScale = Vector3.zero;
        currentTweenSequence = null;
        isActive = false;
    }

    public GlowType GetGlowType() {
        return thisGlowType;
    }

    public void ApplyCapeEffect(GlowType glow) {
        ResetToDefaultValues();
        Debug.Log(nameof(glow));

        switch(glow) {
            case GlowType.Sun:
                sunMaskData.targetScaleSize = defaultMaskSize[0] + 0.5f;
                moonMaskData.targetScaleSize = defaultMaskSize[1] + 0.5f;
                delayBeforeFadeOut = defaultFadeOutDelay;
            break;
            case GlowType.Moon:
                sunMaskData.targetScaleSize = defaultMaskSize[0];
                moonMaskData.targetScaleSize = defaultMaskSize[1];
                delayBeforeFadeOut = defaultFadeOutDelay + 2.5f;
            break;
            case GlowType.Twilight:
                sunMaskData.targetScaleSize = defaultMaskSize[0];
                moonMaskData.targetScaleSize = defaultMaskSize[1];
                delayBeforeFadeOut = defaultFadeOutDelay - 3f;
            break;
        }

    }

    public void ResetToDefaultValues() {
        Debug.Log("Reset Values");
        sunMaskData.targetScaleSize = defaultMaskSize[0];
        moonMaskData.targetScaleSize = defaultMaskSize[1];
        delayBeforeFadeOut = defaultFadeOutDelay;
    }
}
