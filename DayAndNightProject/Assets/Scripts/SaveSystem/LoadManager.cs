using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    Player playerRef;
    ShrineLoader shrineLoader;
    PlayerDataManager dataManager;
    ConsumableLoader consumableLoader;

    void OnEnable()
    {
        playerRef ??= FindObjectOfType<Player>();
        shrineLoader ??= FindObjectOfType<ShrineLoader>();
        dataManager ??= FindObjectOfType<PlayerDataManager>();
        consumableLoader ??= FindObjectOfType<ConsumableLoader>(); 
        SaveLoadSystem.OnLoadGame += PerformLoadGame;
        SaveLoadSystem.OnSaveGame += OnNewGameLoad;
    }

    void OnDisable()
    {
        SaveLoadSystem.OnLoadGame -= PerformLoadGame;
        SaveLoadSystem.OnSaveGame -= OnNewGameLoad;
    }

    void PerformLoadGame(SaveData loadData) {
        Debug.Log("Called Load Game");
        playerRef ??= FindObjectOfType<Player>();
        shrineLoader ??= FindObjectOfType<ShrineLoader>();
        dataManager ??= FindObjectOfType<PlayerDataManager>();
        consumableLoader ??= FindObjectOfType<ConsumableLoader>(); 


        dataManager.UpdatePlayerData(loadData.savedPlayerData);
        shrineLoader.OnLoad(loadData.savedShrineData);
        consumableLoader.OnGameLoad(loadData);

        UpdatePlayerToLoad(loadData);
    }

    void OnNewGameLoad(SaveData loadData) {
        if(loadData.isDefault) {
            playerRef ??= FindObjectOfType<Player>();
            shrineLoader ??= FindObjectOfType<ShrineLoader>();
            dataManager ??= FindObjectOfType<PlayerDataManager>();
            consumableLoader ??= FindObjectOfType<ConsumableLoader>(); 


            dataManager.ResetInventory();
            FindObjectOfType<PlayerSpawnManager>().EnableSpawnCheckpoint(loadData);
        }
    }
    void UpdatePlayerToLoad(SaveData loadData) {
        playerRef.transform.position = loadData.GlobalPlayerPosition;
    }
}
