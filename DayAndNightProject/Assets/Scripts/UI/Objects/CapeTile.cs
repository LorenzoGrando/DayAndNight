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
    public Color[] textColors;
    public void GetNewItem(Item newItem, float currentMoonAmount, float currentSunAmount) {
        thisTileItem = newItem;
        for(int i = 0; i < 2; i++) {
            TextMeshProUGUI text = lockGameObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if(text != null) {
                text.text = newItem.buyRequirements[i].ToString();
                Color color =  textColors[0];
                if(i == 0) {
                    if(newItem.buyRequirements[i] <= currentMoonAmount) {
                        color = textColors[1];
                    }
                }
                else {
                    if(newItem.buyRequirements[i] <= currentMoonAmount) {
                        color = textColors[1];
                    }
                }

                text.color = color;
            }
        }
    }

    public void UpdateAppearance(bool isActive) {
        lockGameObject.SetActive(!thisTileItem.isUnlocked);

        if(isActive) {
            Color color = myImage.color;
            color.a = 1;
            myImage.color = color;
        }
        else {
            Color color = myImage.color;
            color.a = 0;
            myImage.color = color;
        }
    }
}
