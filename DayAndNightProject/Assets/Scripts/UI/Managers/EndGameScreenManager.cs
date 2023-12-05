using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreenManager : MonoBehaviour
{
    public GameObject startCanvasEnabler;
    public GameObject midCanvasEnabler;
    public Animator startAnimator;
    public Animator midAnimator;
    public UnityEngine.UI.Image image;
    private Material BGMaterial;
    public GameObject[] playerHairs;
    public GameObject[] thrones;
    public float holdDelay;
    public Texture2D[] bgTextures;
    private const string startAnim = "TriggerStart";
    private const string endAnim = "TriggerEnd";

    public void StartEndCutscene(Shrine.ShrineTypeStatus shrineGlowType) {
        UpdateMaterial(shrineGlowType);
        UpdateEndScene(shrineGlowType);
        startCanvasEnabler.SetActive(true);
        startAnimator.SetTrigger(startAnim);
        Invoke(nameof(EndEndCutscene), 18.55f);
    }

    public void StartMidEndCutscene() {
        startCanvasEnabler.SetActive(false);
        midAnimator.SetTrigger(startAnim);
        Invoke(nameof(EndEndCutscene), holdDelay * 2);
    }

    public void EndEndCutscene(){
        midAnimator.SetTrigger(endAnim);
        Invoke(nameof(CallReloadScene),4.5f);
    }

    private void CallReloadScene() {
        SceneManager.LoadScene(0);
    }

    private void UpdateMaterial(Shrine.ShrineTypeStatus glowType) {
        BGMaterial = image.material;
        Texture2D targetTex = glowType == Shrine.ShrineTypeStatus.Sun ? bgTextures[0] : bgTextures[1];

        BGMaterial.SetTexture("_LeftTexture", targetTex);
        BGMaterial.SetTexture("_RightTexture", targetTex);
    }

    private void UpdateEndScene(Shrine.ShrineTypeStatus shrineGlowType) {
        int celestialIndex = 0;
        if(shrineGlowType == Shrine.ShrineTypeStatus.Moon) {
            celestialIndex = 1;
        }

        for(int t = 0; t < thrones.Length; t++) {
            if(t == celestialIndex) {
                thrones[t].gameObject.SetActive(true);
            }
            else {
                thrones[t].gameObject.SetActive(false);
            }
        }

        int playerIndex = FindObjectOfType<PlayerAnimationManager>().activePlayerSprite;

        for(int s = 0; s < playerHairs.Length; s++) {
            if(s == playerIndex) {
                playerHairs[s].gameObject.SetActive(true);
            }
            else {
                playerHairs[s].gameObject.SetActive(false);
            }
        }
    }
}
