using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    Player playerRef;
    ShrineLoader shrineLoader;
    PlayerDataManager dataManager;
    ConsumableLoader consumableLoader;
    PlayerAnimationManager animManager;

    void OnEnable()
    {
        playerRef ??= FindObjectOfType<Player>();
        shrineLoader ??= FindObjectOfType<ShrineLoader>();
        dataManager ??= FindObjectOfType<PlayerDataManager>();
        consumableLoader ??= FindObjectOfType<ConsumableLoader>();
        animManager ??= FindObjectOfType<PlayerAnimationManager>();
        SaveLoadSystem.OnLoadGame += PerformLoadGame;
        SaveLoadSystem.OnSaveGame += OnNewGameLoad;
        
        Cursor.visible = false;
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
        animManager ??= FindObjectOfType<PlayerAnimationManager>();


        dataManager.UpdatePlayerData(loadData.savedPlayerData);
        shrineLoader.OnLoad(loadData.savedShrineData);
        consumableLoader.OnGameLoad(loadData);
        animManager.UpdateActiveSprite(loadData);
        animManager.UpdateCape(loadData.savedPlayerData.currentInventory.ReturnCurrentInventoryData().activeCapeIndexValue);

        UpdatePlayerToLoad(loadData);
    }

    void OnNewGameLoad(SaveData loadData) {
        if(loadData.isDefault) {
            playerRef ??= FindObjectOfType<Player>();
            shrineLoader ??= FindObjectOfType<ShrineLoader>();
            dataManager ??= FindObjectOfType<PlayerDataManager>();
            consumableLoader ??= FindObjectOfType<ConsumableLoader>(); 
            animManager ??= FindObjectOfType<PlayerAnimationManager>();

            dataManager.ResetInventory();
            FindObjectOfType<PlayerSpawnManager>().EnableSpawnCheckpoint(loadData);
            animManager.UpdateActiveSprite(loadData);
        }
    }
    void UpdatePlayerToLoad(SaveData loadData) {
        playerRef.transform.position = loadData.GlobalPlayerPosition;
    }
}
