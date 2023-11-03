using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Player player;
    public SaveLoadObject spawnObject;

    void OnEnable()
    {
        player = FindObjectOfType<Player>();
        SaveLoadSystem.OnLoadGame += EnableSpawnCheckpoint;
    }
    
    void OnDisable()
    {
        SaveLoadSystem.OnLoadGame -= EnableSpawnCheckpoint;
    }

    public void ReturnPlayerToSpawn() {
        player.transform.position = transform.position;
    }

    public void EnableSpawnCheckpoint(SaveData data) {
        spawnObject.gameObject.SetActive(true);
    }
}
