using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TutorialObject : MonoBehaviour
{
    [SerializeField]
    private int screenIndex;
    [SerializeField]
    private bool isConsumable;
    [SerializeField]
    private TutorialScreenManager _tutManager;


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            _tutManager.TriggerTutorial(screenIndex, isConsumable);
            gameObject.SetActive(false);
        }
    }
}
