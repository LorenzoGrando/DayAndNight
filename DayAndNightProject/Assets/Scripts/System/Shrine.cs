using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shrine : MonoBehaviour
{
    public event Action<ShrineTypeStatus> OnShrineComplete;
    public event Action<ShrineTypeStatus> OnShrineLoad;

    public enum ShrineTypeStatus {Uncomplete, Sun, Moon}
    public ShrineTypeStatus thisShrineStatus;
    [SerializeField]
    private Animator thisShrineCutsceneCam;
    [SerializeField]
    private ShrineCloudManager shrineClouds;
    [SerializeField]
    private ShrineBackgroundManager shrineBackgrounds;
    private CameraTransitionManager cameraTransitioner;
    public float animDurationDelay;
    public bool isLastShrine = false;

    void OnEnable()
    {
        cameraTransitioner = FindObjectOfType<CameraTransitionManager>();
    }

    public void ActivateShrine(ShrineTypeStatus activationStatus) {
        if(thisShrineStatus == ShrineTypeStatus.Uncomplete) {
            thisShrineStatus = activationStatus;
            switch(thisShrineStatus) {
                case ShrineTypeStatus.Sun:
                    shrineBackgrounds.ChangeBackgroundType(GlowEffectManager.GlowType.Sun);
                break;
                case ShrineTypeStatus.Moon:
                    shrineBackgrounds.ChangeBackgroundType(GlowEffectManager.GlowType.Moon);
                break;
            }
            FindObjectOfType<ShrineLoader>().UpdateSceneShrineData();
            OnShrineComplete?.Invoke(thisShrineStatus);
            Debug.Log("Invoked Shrine");
            BeginActivationCutscene();
        }
    }

    public void LoadShrine(ShrineTypeStatus shrineData) {
        Debug.Log("Tried to load shrine: " + gameObject.name);
        thisShrineStatus = shrineData;
        switch(thisShrineStatus) {
                case ShrineTypeStatus.Sun:
                    shrineBackgrounds.ChangeBackgroundType(GlowEffectManager.GlowType.Sun);
                    shrineClouds.FadeOutClouds();
                break;
                case ShrineTypeStatus.Moon:
                    shrineBackgrounds.ChangeBackgroundType(GlowEffectManager.GlowType.Moon);
                    shrineClouds.FadeOutClouds();
                break;
        }
        CelestialTree[] trees = FindObjectsOfType<CelestialTree>();
        foreach(CelestialTree treeInstance in trees) {
            if(treeInstance.shrineToReact == this) {
                treeInstance.ActivateTree(shrineData);
            }
        }
        OnShrineLoad?.Invoke(shrineData);
    }

    private void BeginActivationCutscene() {
        cameraTransitioner.ChangePlayerCamStatus(false);
        if(thisShrineCutsceneCam != null)
            thisShrineCutsceneCam.gameObject.SetActive(true);
        shrineClouds.FadeOutClouds();
        thisShrineCutsceneCam.SetTrigger("FirstAnim");
        FindObjectOfType<InGameSoundManager>().PlayShrine();
        FindObjectOfType<InGameSoundManager>().HalfFadeOutMusic();
        StartCoroutine(routine:DelaySecondAnimation());
    }

    private void BeginFinishActivationCutcene() {
        thisShrineCutsceneCam.SetTrigger("LastAnim");
        FindObjectOfType<InGameSoundManager>().EndShrine();
        FindObjectOfType<InGameSoundManager>().FadeInMusic();
        StartCoroutine(DelayRecallSpawnPos(animDurationDelay/2));

        if(isLastShrine) {
            FindObjectOfType<EndGameScreenManager>().StartEndCutscene(thisShrineStatus);
        }
    }

    IEnumerator DelaySecondAnimation() {
        yield return new WaitForSeconds(animDurationDelay);
        BeginFinishActivationCutcene();
        yield break;
    }

    IEnumerator DelayRecallSpawnPos(float delay) {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<PlayerSpawnManager>().ReturnPlayerToSpawn();
        yield return new WaitForSeconds(delay);
        if(thisShrineCutsceneCam != null) 
            thisShrineCutsceneCam.gameObject.SetActive(false);
        cameraTransitioner.ChangePlayerCamStatus(true);
        yield break;
    }
}
