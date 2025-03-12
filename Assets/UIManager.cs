using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public PlayerInventory playerInventory;
   EquipmentWindowUI equipmentWindowUI;

   [Header("UI Window")]
   public GameObject hudWindow;
   public GameObject selectWindow;
   public GameObject weaponInventoryWindow;

   [Header("Weapon Inventory")]
   public GameObject weaponInventorySlotPrefab;
   public Transform weaponInventorySlotsParent;
   WeaponInventorySlot[] weaponInventorySlots;

   public void Awake()
   {
      equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
   }

   private void Start()
   {
      weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
      equipmentWindowUI.LoadWeaponsOnEquimentScreen(playerInventory);
   }
   public void UpdateUI()
   {
      #region Weapon Inventory Slots
      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
         if (i < playerInventory.weaponsInventory.Count)
         {
            if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
            {
               Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
               weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            }
            weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
         }
         else
         {
            weaponInventorySlots[i].ClearInventortSlot();
         }
      }
      #endregion
   }


   public void OpenSelectWindow()
   {
      selectWindow.SetActive(true);
   }

   public void CloseSelectWindow()
   {
      selectWindow.SetActive(false);
   }

   public void CloseAllInventoryWindows()
   {
      weaponInventoryWindow.SetActive(false);
   }

}
