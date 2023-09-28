using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public Vector3 GlobalPlayerPosition = Vector3.zero;
    public float TotalSaveIndex = 0;
    public SaveLoadObject LastCheckpointReached = null;
}