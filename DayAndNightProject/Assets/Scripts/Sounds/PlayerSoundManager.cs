using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource walkSource, jumpSource, sphereSound;

    public void StartMovementSound() {
        if(!walkSource.isPlaying)
            walkSource.Play();
    }

    public void PlaySphereSound() {
        sphereSound.Play();
    }

    public void JumpSound() {
        if(!jumpSource.isPlaying) {
            StopAllPlayerSounds();
            jumpSource.Play();
        }
    }

    public void StopAllPlayerSounds() {
        walkSource.Stop();
        jumpSource.Stop();
    }

    public void StopWalkingSound() {
        walkSource.Stop();
    }
}
