using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum SlotType {Weapon,Shield}
    public enum ItemQuality {Normal, Rare, Epic}

    [Header("# Main Info")]
    public SlotType slotType;
    public ItemQuality quality;
    public int itemId;
    public string itemName;
    public Sprite itemIcon;

    [Header("# Stats")]
    public float baseAttack;
    public float baseDefense;   


}
