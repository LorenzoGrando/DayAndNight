using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            SaveData currentSaveData = SaveLoadSystem.GetCurrentSave(out bool isDefault);
            if (currentSaveData.LastCheckpointReached != this) {
                SaveGame();
            }
        }
    }
    private void SaveGame(){
        SaveData newSaveData = new SaveData(gameObject.transform.position, this, FindObjectOfType<PlayerDataManager>().currentPlayerData,
                            FindObjectOfType<ShrineLoader>().shrineSaveData, FindObjectOfType<ConsumableLoader>().GetData(), false);

        SaveLoadSystem.Save(false, newSaveData);
        Debug.Log("Saved on new Checkpoint");
    }
}
