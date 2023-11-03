using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineCloudManager : MonoBehaviour
{
    private Animator animator;
    public bool isFaded = false;

    void OnEnable()
    {
        ResetAnimator();
    }

    public void FadeOutClouds() {
        ResetAnimator();
        animator.SetTrigger("PlayFade");
        isFaded = true;
    }

    public void ResetAnimator() {
        animator = GetComponent<Animator>();
    }
}
