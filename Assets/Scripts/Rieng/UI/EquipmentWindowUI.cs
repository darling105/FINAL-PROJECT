using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    public HandEquipmentSlotUI[] handEquimentSlotUI;

    private void Start()
    {
        handEquimentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
    }

    public void LoadWeaponsOnEquimentScreen(PlayerInventoryManager playerInventory)
    {
        for (int i = 0; i < handEquimentSlotUI.Length; i++)
        {
            if (handEquimentSlotUI[i].rightHandSlot01)
            {
                handEquimentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
            }
            else if (handEquimentSlotUI[i].rightHandSlot02)
            {
                handEquimentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
            }
            else if (handEquimentSlotUI[i].leftHandSlot01)
            {
                handEquimentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
            }
            else
            {
                handEquimentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }
    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }
    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }
    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Selected = true;
    }
}
