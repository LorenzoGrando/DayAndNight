using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class TextureCameraRender : MonoBehaviour
{
    [SerializeField]
    private Camera textureCamera;
    private RenderTexture currentRenderTexture;

    #if UNITY_EDITOR
    void OnEnable()
    {
        if(textureCamera.targetTexture == null) {
            currentRenderTexture = CreateRenderTexture();
            textureCamera.targetTexture = currentRenderTexture;
        }
    }
    #endif

    void Start()
    {
        if(currentRenderTexture != null) {
            Destroy(currentRenderTexture);
            textureCamera.targetTexture = null;
        }

        if(textureCamera.targetTexture == null) {
            currentRenderTexture = CreateRenderTexture();
        }
    }

    private RenderTexture CreateRenderTexture()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        RenderTexture newText = new(screenWidth, screenHeight, 32);

        return newText;
    }

    public RenderTexture GetRenderTexture() {
        return currentRenderTexture;
    }
}