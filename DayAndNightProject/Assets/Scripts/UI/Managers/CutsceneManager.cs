using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private Image imageobject;
    [SerializeField]
    private GameObject bgObject;
    [SerializeField]
    private GameObject canvasEnabler;
    [SerializeField]
    private GameObject environmentSpritesObject, cloudSpritesObject;
    [SerializeField]
    private SpriteRenderer[] playerSpriteRenderers;
    [SerializeField]
    private Material cloudMat;
    [SerializeField]
    private GameObject fakeObject;
    [SerializeField]
    private float coverageAnimDuration;


    [SerializeField]
    private bool beginAnim = false;
    [SerializeField]
    private bool finishAnim = false;
    [SerializeField]
    private bool hideCanvas = false;

    private Tween cloudCoverageTween;

    void OnEnable()
    {
        _animator = GetComponent<Animator>();
        cloudMat = bgObject.GetComponent<MeshRenderer>().material;
        ChangeSceneStatus(false, false);
        fakeObject.transform.localPosition = Vector3.zero;
        cloudMat.SetFloat("_intensity", 0);
    }

    void OnDisable()
    {
        if(cloudCoverageTween != null) {
            cloudCoverageTween.Kill();
            cloudCoverageTween = null;
        }
    }

    void Update()
    {
        cloudMat.SetFloat("_intensity", fakeObject.transform.localPosition.x);

        if(beginAnim) {
            StartCloudCoverage();
            beginAnim = false;
        }

        if(finishAnim) {
            EndMenuCutscene(true);
            finishAnim = false;
        }

        if(hideCanvas) {
            HideCanvas();
            hideCanvas = false;
        }
    }

    public void StartMenuCutscene() {
        _playerInput.UpdateActiveActionMap(PlayerInput.InputMaps.Cutscene);
        ChangeSceneStatus(false, false);
        canvasEnabler.SetActive(true);
        bgObject.SetActive(true);
        _animator.SetTrigger("PlayCutscene");
    }

    public void EndMenuCutscene(bool cloudCoverage) {
        ChangeSceneStatus(true, cloudCoverage);
        bgObject.SetActive(false);
    }

    public void HideCanvas() {
        canvasEnabler.SetActive(false);
        gameObject.SetActive(false);
        _playerInput.UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
    }

    public void ChangeSceneStatus(bool status, bool cloudCoverage) {
        foreach(SpriteRenderer render in playerSpriteRenderers) {
            render.enabled = status;
        }
        environmentSpritesObject.SetActive(status);
        cloudSpritesObject.SetActive(cloudCoverage);
    }
    
    public void StartCloudCoverage() {
        if(cloudCoverageTween != null) {
            cloudCoverageTween.Kill();
            cloudCoverageTween = null;
        }

        cloudCoverageTween = fakeObject.transform.DOLocalMove(Vector3.one, coverageAnimDuration);
    }
}