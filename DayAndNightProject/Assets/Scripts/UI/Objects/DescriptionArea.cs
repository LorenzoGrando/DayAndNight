using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionArea : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI themeText;

    public void UpdateTexts(Item currentItem) {
        descriptionText.text = currentItem.itemFunctionalitDescription;
        themeText.text = currentItem.itemThemeDescription;
    }
}
