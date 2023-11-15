using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceDisabler : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            gameObject.SetActive(false);
        }
    }
}
