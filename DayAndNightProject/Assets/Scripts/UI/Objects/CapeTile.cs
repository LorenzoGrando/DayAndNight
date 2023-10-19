using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeTile : MonoBehaviour
{
    [SerializeField]
    private GameObject lockGameObject;

    private Item thisTileItem;

    public void GetNewItem(Item newItem) {
        thisTileItem = newItem;
    }

    public void UpdateAppearance() {
        lockGameObject.SetActive(!thisTileItem.isUnlocked);
    }
}
