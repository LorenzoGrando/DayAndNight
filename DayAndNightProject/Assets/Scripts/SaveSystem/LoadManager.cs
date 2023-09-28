using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    Player playerRef;

    void OnEnable()
    {
        SaveLoadSystem.OnLoadGame += UpdatePlayerToLoad;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace)) {
            SaveLoadSystem.Load();
        }
    }
    void UpdatePlayerToLoad(SaveData loadData) {
        playerRef ??= FindObjectOfType<Player>();
        playerRef.transform.position = loadData.GlobalPlayerPosition;
    }
}
