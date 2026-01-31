using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public ItemData currentWeapon;
    public ItemData currentShield;

    [Header("HUD Elements")] 
    public Image weaponIconImage;
    public Image shieldIconImage;

    private void Awake()
    {
        PlayerStats stats = GameManager.instance.playerStats;

        if (currentWeapon != null)
        {
            stats.AttackPow += CalculateAttack(currentWeapon);

            if (weaponIconImage != null)
            {
                weaponIconImage.sprite = currentWeapon.itemIcon;
                weaponIconImage.enabled = true;
            }
        }
        else
        {
            if (weaponIconImage != null)
            {
                weaponIconImage.enabled = false;
            }
        }

        if (currentShield != null)
        {
            stats.DefensePow += CalculateDefense(currentShield);

            if (shieldIconImage != null)
            {
                shieldIconImage.sprite = currentShield.itemIcon;
                shieldIconImage.enabled = true;
            }
        }
        else
        {
            if (shieldIconImage != null)
            {
                shieldIconImage.enabled = false;
            }
        }
    }


    public float CalculateAttack(ItemData item)
    {
        return item.baseAttack;
    }

    public float CalculateDefense(ItemData item)
    {
        return item.baseDefense;
    }

    private void Unequip(ItemData.SlotType slot)
    {
        PlayerStats stats = GameManager.instance.playerStats;

        if (slot == ItemData.SlotType.Weapon && currentWeapon != null)
        {
            stats.AttackPow -= CalculateAttack(currentWeapon);
            currentWeapon = null;

            
            if (weaponIconImage != null)
            {
                weaponIconImage.sprite = null;
                weaponIconImage.enabled = false;
            }
        }

        if (slot == ItemData.SlotType.Shield && currentShield != null)
        {
            stats.DefensePow -= CalculateDefense(currentShield);
            currentShield = null;

            
            if (shieldIconImage != null)
            {
                shieldIconImage.sprite = null;
                shieldIconImage.enabled = false;
            }
        }
    }

    public void Equip(ItemData newItem)
    {
        Unequip(newItem.slotType);

        PlayerStats stats = GameManager.instance.playerStats;

        if (newItem.slotType == ItemData.SlotType.Weapon)
        {
            currentWeapon = newItem;
            stats.AttackPow += CalculateAttack(newItem);

            if (weaponIconImage != null)
            {
                weaponIconImage.sprite = newItem.itemIcon;
                weaponIconImage.enabled = true;
            }
        }
        else if (newItem.slotType == ItemData.SlotType.Shield)
        {
            currentShield = newItem;
            stats.DefensePow += CalculateDefense(newItem);

            if (shieldIconImage != null)
            {
                shieldIconImage.sprite = newItem.itemIcon;
                shieldIconImage.enabled = true;
            }
        }
    }
}
