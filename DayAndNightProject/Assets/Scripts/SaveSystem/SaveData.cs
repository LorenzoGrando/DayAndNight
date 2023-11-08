using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 GlobalPlayerPosition = Vector3.zero;
    public SaveLoadObject LastCheckpointReached = null;
    public PlayerData savedPlayerData = null;
    public ShrineSaveData savedShrineData = null;
    public ConsumableData consumableData = null;
    public enum PlayerSpriteChoice {
        Male, 
        Female
    };

    public PlayerSpriteChoice thisGameSpriteChoice = PlayerSpriteChoice.Male;
    public bool isDefault = true;

    public SaveData(Vector3 playerPos, SaveLoadObject lastCheckpoint, PlayerData playerData, 
                            ShrineSaveData shrineData, ConsumableData consumableData, PlayerSpriteChoice sprite, bool isDefault) {
        GlobalPlayerPosition = playerPos;
        LastCheckpointReached = lastCheckpoint;
        savedPlayerData = playerData;
        savedShrineData = shrineData;
        this.consumableData = consumableData;
        thisGameSpriteChoice = sprite;
        this.isDefault = isDefault;
    }

    public SaveData() {
        GlobalPlayerPosition = Vector3.zero;
        LastCheckpointReached = null;
        savedPlayerData = new PlayerData();
        savedShrineData = new ShrineSaveData();
        consumableData = new ConsumableData();
        thisGameSpriteChoice = PlayerSpriteChoice.Male;
        isDefault = true;
    }
}