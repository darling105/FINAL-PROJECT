using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public WeaponEquipmentSlotUI[] weaponEquimentSlotUI;
    public HeadEquipmentSlotUI headEquipmentSlotUI;
    public BodyEquipmentSlotUI bodyEquipmentSlotUI;
    public LegEquipmentSlotUI legEquipmentSlotUI;
    public HandEquipmentSlotUI handEquipmentSlotUI;


    public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventory)
    {
        for (int i = 0; i < weaponEquimentSlotUI.Length; i++)
        {
            if (weaponEquimentSlotUI[i].rightHandSlot01)
            {
                weaponEquimentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
            }
            else if (weaponEquimentSlotUI[i].rightHandSlot02)
            {
                weaponEquimentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
            }
            else if (weaponEquimentSlotUI[i].leftHandSlot01)
            {
                weaponEquimentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
            }
            else
            {
                weaponEquimentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventory)
    {
        if (playerInventory.currentHelmetEquipment != null)
        {
            headEquipmentSlotUI.AddItem(playerInventory.currentHelmetEquipment);
        }
        else
        {
            headEquipmentSlotUI.ClearItem();
        }

        if(playerInventory.currentBodyEquipment != null)
        {
            bodyEquipmentSlotUI.AddItem(playerInventory.currentBodyEquipment);
        }
        else
        {
            bodyEquipmentSlotUI.ClearItem();
        }

        if (playerInventory.currentLegEquipment != null)
        {
            legEquipmentSlotUI.AddItem(playerInventory.currentLegEquipment);
        }
        else
        {
            legEquipmentSlotUI.ClearItem();
        }

        if (playerInventory.currentHandEquipment != null)
        {
            handEquipmentSlotUI.AddItem(playerInventory.currentHandEquipment);
        }
        else
        {
            handEquipmentSlotUI.ClearItem();
        }
    }
}
