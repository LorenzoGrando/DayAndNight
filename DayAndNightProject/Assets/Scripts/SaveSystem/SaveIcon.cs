using UnityEngine;

public class SaveIcon : MonoBehaviour
{
    private Animator animator;

    void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void OnSave() {
        animator.SetTrigger("PlaySaveAnim");
    }
}