using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public enum ItemStorageType {Quantity, Unlockable};
    public enum ItemType {Consumable, Cape};
    public enum ConsumableTypes {Sun, Moon};
    public ItemStorageType itemStorageType;
    public ItemType itemType;
    public ConsumableTypes consumableType;
    public float itemQuantity;
    public bool isUnlocked;
    public string itemName;
    public string itemThemeDescription;
    public string itemFunctionalitDescription;
    public Sprite itemSprite;

    public GlowEffectManager.GlowType capeGlowType;
    public float[] buyRequirements;
}