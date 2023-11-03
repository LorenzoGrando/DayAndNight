using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConsumableLoader : MonoBehaviour
{
    public Consumable[] sceneConsumables;
    public ConsumableData data;

    void  OnEnable()
    {
        data ??= new ConsumableData();
    }

    public ConsumableData GetData() {
        data ??= new ConsumableData();
        UpdateConsumableData();
        return data;
    }
    public void UpdateConsumableData() {
        data.activeData = new bool[sceneConsumables.Length];
        for(int i = 0; i < sceneConsumables.Length; i++) {
            data.activeData[i] = sceneConsumables[i].gameObject.activeSelf;
        }
    }

    public void OnGameLoad(SaveData loadData) {
        if(loadData.consumableData != null) {
            for(int a = 0; a < sceneConsumables.Length; a++) {
                if(!loadData.consumableData.activeData[a]) {
                    sceneConsumables[a].CollectItem();
                }
            }
            UpdateConsumableData();
        }
    }
}

[Serializable]
public class ConsumableData {
    public bool[] activeData;
}
