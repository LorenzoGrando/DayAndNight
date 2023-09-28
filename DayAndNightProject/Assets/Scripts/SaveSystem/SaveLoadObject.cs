using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            SaveData currentSaveData = SaveLoadSystem.GetCurrentSave();
            if(currentSaveData.LastCheckpointReached != this) {
                SaveGame();
            }
        }
    }
    private void SaveGame(){
        SaveData newSaveData = new SaveData
        {
            LastCheckpointReached = this,
            TotalSaveIndex = SaveLoadSystem.GetCurrentSave().TotalSaveIndex + 1,
            GlobalPlayerPosition = gameObject.transform.position
        };

        SaveLoadSystem.Save(newSaveData);
        Debug.Log("Saved on new Checkpoint");
    }
}
