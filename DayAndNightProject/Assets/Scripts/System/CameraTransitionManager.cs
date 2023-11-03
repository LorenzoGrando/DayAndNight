using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionManager : MonoBehaviour
{
    public Camera[] playerCams;
    private PlayerInput playerInput;

    void OnEnable() {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    public void ChangePlayerCamStatus(bool newStatus) {
        if(playerCams != null) {
            foreach(Camera cam in playerCams) {
                cam.enabled = newStatus;
            }
        }

        if(newStatus) {
            playerInput.UpdateActiveActionMap(PlayerInput.InputMaps.Gameplay);
        }
        else {
            playerInput.UpdateActiveActionMap(PlayerInput.InputMaps.UI);
        }
    }
}
