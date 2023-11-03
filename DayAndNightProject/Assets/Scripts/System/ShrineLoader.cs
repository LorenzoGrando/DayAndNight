using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineLoader : MonoBehaviour
{
    public ShrineSaveData shrineSaveData;
    public Shrine[] shrines;
    public void OnLoad(ShrineSaveData data) {
        for(int i = 0; i < shrines.Length; i++) {
            Debug.Log("Called load shrine of index " + i);
            shrineSaveData.sceneShrineData[i].status = data.sceneShrineData[i].status;
            shrines[i].LoadShrine(data.sceneShrineData[i].status);
        }
    }

    public void ResetSave() {
        for(int j = 0;j < shrineSaveData.sceneShrineData.Length; j++) {
            shrineSaveData.sceneShrineData[j].status = Shrine.ShrineTypeStatus.Uncomplete;
        }
    }

    public ShrineSaveData ReturnSaveData() {
        return shrineSaveData;
    }

    public void UpdateSceneShrineData() {
        for(int j = 0; j < shrines.Length; j++) {
            Debug.Log("Shrine of index: " + j + " has status of: " + shrines[j].thisShrineStatus);
            shrineSaveData.sceneShrineData[j].status = shrines[j].thisShrineStatus;
        }
    }
}

[Serializable]
public class ShrineSaveData {
    [SerializeField]
    public ShrineData[] sceneShrineData;
}
[Serializable]
public class ShrineData {
    public Shrine shrine;
    public Shrine.ShrineTypeStatus status;
}