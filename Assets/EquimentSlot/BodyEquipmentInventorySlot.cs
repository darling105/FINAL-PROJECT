using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyEquipmentInventorySlot : MonoBehaviour
{
   UIManager uiManager;

    public Image icon;
    BodyEquipment item;

    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public void AddItem(BodyEquipment newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void EquipThisItem()
    {
        if (uiManager.bodyEquipmentSlotSelected)
        {
            if (uiManager.player.playerInventoryManager.currentBodyEquipment != null)
            {
                uiManager.player.playerInventoryManager.bodyEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentBodyEquipment);
            }
            uiManager.player.playerInventoryManager.currentBodyEquipment = item;
            uiManager.player.playerInventoryManager.bodyEquipmentInventory.Remove(item);
            uiManager.player.playerEquipmentManager.EquipAllEquipmentModelsOnStart();
        }
        else
        {
            return;
        }
        
        uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uiManager.player.playerInventoryManager);
        uiManager.ResetAllSelectedItems();
    }
}
