using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableTile : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI quantityText;

    public void UpdateText(float newValue) {
        quantityText.text = newValue.ToString();
    }
}
