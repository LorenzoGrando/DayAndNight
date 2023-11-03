using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public static class SaveLoadSystem
{
    public static event Action<SaveData> OnSaveGame;
    public static event Action<SaveData> OnLoadGame;

    public static SaveData saveData = new SaveData();

    public const string saveFolderDirectory = "/Assets/Saves/";
    public const string defaultSaveFileName = "DefaultSaveFile.sav";
    public const string currentSaveFileName = "ActiveSaveFile.sav";

    public static void Save( bool isDefault, SaveData newSaveData = null) {
        saveData ??= new SaveData();
        newSaveData ??= saveData;

        var directory = Application.persistentDataPath + saveFolderDirectory;

        if(!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        string json = JsonUtility.ToJson(newSaveData, true);

        if(isDefault) {
            File.WriteAllText(directory + defaultSaveFileName, json);
        }
        else {
            File.WriteAllText(directory + currentSaveFileName, json);
            OnSaveGame?.Invoke(newSaveData);
        }
        saveData = newSaveData;
    }

    public static void Load() {
        string saveFilePath = Application.persistentDataPath + saveFolderDirectory + currentSaveFileName;
        SaveData loadData = new SaveData();

        if(File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);
            loadData = JsonUtility.FromJson<SaveData>(json);
        }

        saveData = loadData;
        OnLoadGame?.Invoke(saveData);
    }

    public static void CreateDefaultSave(bool forceNewSave) {
        var directory = Application.persistentDataPath + saveFolderDirectory;
        if(!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        var defaultSaveFile = directory + defaultSaveFileName;
        if(!File.Exists(defaultSaveFile) && forceNewSave == false) {
            SaveData defaultSave = new SaveData();
            Save(true, defaultSave);
        }
        else if(forceNewSave) {
            SaveData defaultSave = new SaveData();
            Save(false, defaultSave);
        }
    }

    public static SaveData GetCurrentSave(out bool isDefault) {
        saveData ??= new SaveData();
        isDefault = saveData.isDefault;
        return saveData;
    }

    public static void ClearLoadData() {
        Save(false, new SaveData());
    }
}