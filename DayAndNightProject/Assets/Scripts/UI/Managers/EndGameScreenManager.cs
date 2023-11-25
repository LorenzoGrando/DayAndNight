using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreenManager : MonoBehaviour
{
    public GameObject canvasEnabler;
    public Animator screenAnimator;
    public float holdDelay;
    private const string startAnim = "TriggerStart";
    private const string endAnim = "TriggerEnd";

    public void StartEndCutscene() {
        canvasEnabler.SetActive(true);
        screenAnimator.SetTrigger(startAnim);
        Invoke(nameof(EndEndCutscene), holdDelay);
    }

    public void EndEndCutscene(){
        screenAnimator.SetTrigger(endAnim);
        Invoke(nameof(CallReloadScene),4.5f);
    }

    private void CallReloadScene() {
        SceneManager.LoadScene(0);
    }
}
