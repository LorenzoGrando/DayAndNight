using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public Vector3 GlobalPlayerPosition = Vector3.zero;
    public SaveLoadObject LastCheckpointReached = null;
    public PlayerData savedPlayerData = null;
    public ShrineSaveData savedShrineData = null;
    public ConsumableData consumableData = null;
    public bool isDefault = true;

    public SaveData(Vector3 playerPos, SaveLoadObject lastCheckpoint, PlayerData playerData, ShrineSaveData shrineData, ConsumableData consumableData, bool isDefault) {
        GlobalPlayerPosition = playerPos;
        LastCheckpointReached = lastCheckpoint;
        savedPlayerData = playerData;
        savedShrineData = shrineData;
        this.consumableData = consumableData;
        this.isDefault = isDefault;
    }

    public SaveData() {
        GlobalPlayerPosition = Vector3.zero;
        LastCheckpointReached = null;
        savedPlayerData = new PlayerData();
        savedShrineData = new ShrineSaveData();
        consumableData = new ConsumableData();
        isDefault = true;
    }
}