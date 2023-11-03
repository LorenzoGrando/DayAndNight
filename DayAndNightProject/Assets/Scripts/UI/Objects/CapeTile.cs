using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CapeTile : MonoBehaviour
{
    [SerializeField]
    private GameObject lockGameObject;

    private Item thisTileItem;
    public Image myImage;
    public void GetNewItem(Item newItem) {
        thisTileItem = newItem;
        for(int i = 0; i < 2; i++) {
            TextMeshProUGUI text = lockGameObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            text.text = newItem.buyRequirements[i].ToString();
        }
    }

    public void UpdateAppearance(bool isActive) {
        lockGameObject.SetActive(!thisTileItem.isUnlocked);

        if(isActive) {
            myImage.color = Color.cyan;
        }
        else {
            myImage.color = Color.white;
        }
    }
}
