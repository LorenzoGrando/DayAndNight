using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TutorialObject : MonoBehaviour
{
    [SerializeField]
    private int screenIndex;
    [SerializeField]
    private TutorialScreenManager _tutManager;


    void OnTriggerEnter2D(Collider2D other)
    {
        _tutManager.TriggerTutorial(screenIndex);
        gameObject.SetActive(false);
    }
}
