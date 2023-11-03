using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleReactors : MonoBehaviour
{
    public Shrine thisReactorShrine;
    public bool defaultStatus;
    public GameObject objectToReact;

    void OnEnable()
    {
        thisReactorShrine.OnShrineComplete += OnShrineActivation;
        thisReactorShrine.OnShrineLoad += OnShrineLoad;
    }

    void OnDisable()
    {
        thisReactorShrine.OnShrineComplete -= OnShrineActivation;
        thisReactorShrine.OnShrineLoad -= OnShrineLoad;
    }

    public void OnShrineActivation(Shrine.ShrineTypeStatus status) {
        objectToReact.SetActive(!defaultStatus);
    }

    public void OnShrineLoad(Shrine.ShrineTypeStatus status) {
        
        if(status == Shrine.ShrineTypeStatus.Uncomplete) {
            objectToReact.SetActive(defaultStatus);
        }
        else {
            objectToReact.SetActive(!defaultStatus);
        }
        
    }
}
