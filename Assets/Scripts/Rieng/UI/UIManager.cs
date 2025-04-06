using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   PlayerManager playerManager;
   public EquipmentWindowUI equipmentWindowUI;
   private QuickSlotsUI quickSlotsUI;

   [Header("HUD")]
   public Text shadesCount;

   [Header("UI Window")]
   public GameObject hudWindow;
   public GameObject selectWindow;
   public GameObject equipmentScreenWindow;
   public GameObject weaponInventoryWindow;
   public GameObject levelUpWindow;

   [Header("Equipment Window Slot Selected")]
   public bool rightHandSlot01Selected;
   public bool rightHandSlot02Selected;
   public bool leftHandSlot01Selected;
   public bool leftHandSlot02Selected;

   [Header("Weapon Inventory")]
   public GameObject weaponInventorySlotPrefab;
   public Transform weaponInventorySlotsParent;
   WeaponInventorySlot[] weaponInventorySlots;

   public void Awake()
   {
      quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
      playerManager = FindObjectOfType<PlayerManager>();
   }

   private void Start()
   {
      weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
      equipmentWindowUI.LoadWeaponsOnEquimentScreen(playerManager.playerInventoryManager);
      quickSlotsUI.UpdateCurrentConsumableIcon(playerManager.playerInventoryManager.currentConsumable);
      quickSlotsUI.UpdateCurrentSpellIcon(playerManager.playerInventoryManager.currentSpell);
      shadesCount.text = playerManager.playerStatsManager.currentShadesCount.ToString();
   }
   public void UpdateUI()
   {
      #region Weapon Inventory Slots
      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
         if (i < playerManager.playerInventoryManager.weaponsInventory.Count)
         {
            if (weaponInventorySlots.Length < playerManager.playerInventoryManager.weaponsInventory.Count)
            {
               Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
               weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            }
            weaponInventorySlots[i].AddItem(playerManager.playerInventoryManager.weaponsInventory[i]);
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
      ResetAllSelectedItems();
      weaponInventoryWindow.SetActive(false);
      equipmentScreenWindow.SetActive(false);
   }

   public void ResetAllSelectedItems()
   {
      rightHandSlot01Selected = false;
      rightHandSlot02Selected = false;
      leftHandSlot01Selected = false;
      leftHandSlot02Selected = false;
   }

}
