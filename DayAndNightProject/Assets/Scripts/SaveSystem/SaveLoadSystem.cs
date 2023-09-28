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
    public const string defaultSaveFileName = "NewSaveFile.sav";

    public static void Save(SaveData newSaveData = null) {
        saveData ??= new SaveData();
        newSaveData ??= saveData;

        var directory = Application.persistentDataPath + saveFolderDirectory;

        if(!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        string json = JsonUtility.ToJson(newSaveData, true);

        File.WriteAllText(directory + defaultSaveFileName, json);

        OnSaveGame?.Invoke(newSaveData);
    }

    public static void Load() {
        string saveFilePath = Application.persistentDataPath + saveFolderDirectory + defaultSaveFileName;
        SaveData loadData = new SaveData();

        if(File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);
            loadData = JsonUtility.FromJson<SaveData>(json);
        }

        saveData = loadData;
        OnLoadGame?.Invoke(saveData);
    }

    public static SaveData GetCurrentSave() {
        saveData ??= new SaveData();
        return saveData;
    }

    public static void ClearLoadData() {
        Save(new SaveData());
    }
}