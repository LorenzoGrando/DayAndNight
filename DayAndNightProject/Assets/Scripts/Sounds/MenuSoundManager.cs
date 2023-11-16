using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _selectionSource, _movementSource;

    public void PlayMovementSound() {
        _movementSource.Play();
    }

    public void PlaySelectionSound() {
        _selectionSource.Play();
    }
}
