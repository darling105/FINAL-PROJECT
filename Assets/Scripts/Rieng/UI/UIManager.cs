using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public PlayerManager player;
   public ItemStatsWindowUI itemStatsWindowUI;
   public EquipmentWindowUI equipmentWindowUI;
   public QuickSlotsUI quickSlotsUI;

   [Header("HUD")]
   public Text shadesCount;

   [Header("UI Window")]
   public GameObject hudWindow;
   public GameObject selectWindow;
   public GameObject equipmentScreenWindow;
   public GameObject weaponInventoryWindow;
   public GameObject itemStatsWindow;
   public GameObject levelUpWindow;


   [Header("Equipment Window Slot Selected")]
   public bool rightHandSlot01Selected;
   public bool rightHandSlot02Selected;
   public bool leftHandSlot01Selected;
   public bool leftHandSlot02Selected;

   public bool headEquipmentSlotSelected;
   public bool bodyEquipmentSlotSelected;
   public bool legEquipmentSlotSelected;
   public bool handEquipmentSlotSelected;

   [Header("Weapon Inventory")]
   public GameObject weaponInventorySlotPrefab;
   public Transform weaponInventorySlotsParent;
   WeaponInventorySlot[] weaponInventorySlots;

   [Header("Head Equipment Inventory")]
   public GameObject headEquipmentInventorySlotPrefab;
   public Transform headEquipmentInventorySlotParent;
   HeadEquipmentInventorySlot[] headEquipmentInventorySlots;

   [Header("Body Equipment Inventory")]
   public GameObject bodyEquipmentInventorySlotPrefab;
   public Transform bodyEquipmentInventorySlotParent;
   BodyEquipmentInventorySlot[] bodyEquipmentInventorySlots;

   [Header("Leg Equipment Inventory")]
   public GameObject legEquipmentInventorySlotPrefab;
   public Transform legEquipmentInventorySlotParent;
   LegEquipmentInventorySlot[] legEquipmentInventorySlots;

   [Header("Hand Equipment Inventory")]
   public GameObject handEquipmentInventorySlotPrefab;
   public Transform handEquipmentInventorySlotParent;
   HandEquipmentInventorySlot[] handEquipmentInventorySlots;

   private void Awake()
   {
      player = FindObjectOfType<PlayerManager>();

      quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();

      weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
      headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
      bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
      legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
      handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
   }

   private void Start()
   {
      equipmentWindowUI.LoadWeaponsOnEquipmentScreen(player.playerInventoryManager);
      equipmentWindowUI.LoadArmorOnEquipmentScreen(player.playerInventoryManager);
      
      if (player.playerInventoryManager.currentConsumable != null)
      {
         quickSlotsUI.UpdateCurrentSpellIcon(player.playerInventoryManager.currentSpell);
      }

      if (player.playerInventoryManager.currentConsumable != null)
      {
         quickSlotsUI.UpdateCurrentConsumableIcon(player.playerInventoryManager.currentConsumable);
      }

      shadesCount.text = player.playerStatsManager.currentShadesCount.ToString();
   }

   public void UpdateUI()
   {
      //weapon inventory slots

      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
         if (i < player.playerInventoryManager.weaponsInventory.Count)
         {
            if (weaponInventorySlots.Length < player.playerInventoryManager.weaponsInventory.Count)
            {
               Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
               weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            }
            weaponInventorySlots[i].AddItem(player.playerInventoryManager.weaponsInventory[i]);
         }
         else
         {
            weaponInventorySlots[i].ClearInventorySlot();
         }
      }

      //head equipment inventory slot

      for (int i = 0; i < headEquipmentInventorySlots.Length; i++)
      {
         if (i < player.playerInventoryManager.headEquipmentInventory.Count)
         {
            if (headEquipmentInventorySlots.Length < player.playerInventoryManager.headEquipmentInventory.Count)
            {
               Instantiate(headEquipmentInventorySlotPrefab, headEquipmentInventorySlotParent);
               headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            }
            headEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.headEquipmentInventory[i]);
         }
         else
         {
            headEquipmentInventorySlots[i].ClearInventorySlot();
         }
      }

      //body equipment inventory slot

      for (int i = 0; i < bodyEquipmentInventorySlots.Length; i++)
      {
         if (i < player.playerInventoryManager.bodyEquipmentInventory.Count)
         {
            if (bodyEquipmentInventorySlots.Length < player.playerInventoryManager.bodyEquipmentInventory.Count)
            {
               Instantiate(bodyEquipmentInventorySlotPrefab, bodyEquipmentInventorySlotParent);
               bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            }
            bodyEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.bodyEquipmentInventory[i]);
         }
         else
         {
            bodyEquipmentInventorySlots[i].ClearInventorySlot();
         }
      }

      //leg equipment inventory slot

      for (int i = 0; i < legEquipmentInventorySlots.Length; i++)
      {
         if (i < player.playerInventoryManager.legEquipmentInventory.Count)
         {
            if (legEquipmentInventorySlots.Length < player.playerInventoryManager.legEquipmentInventory.Count)
            {
               Instantiate(legEquipmentInventorySlotPrefab, legEquipmentInventorySlotParent);
               legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            }
            legEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.legEquipmentInventory[i]);
         }
         else
         {
            legEquipmentInventorySlots[i].ClearInventorySlot();
         }
      }

      //hand equipment inventory slot

      for (int i = 0; i < handEquipmentInventorySlots.Length; i++)
      {
         if (i < player.playerInventoryManager.handEquipmentInventory.Count)
         {
            if (handEquipmentInventorySlots.Length < player.playerInventoryManager.handEquipmentInventory.Count)
            {
               Instantiate(handEquipmentInventorySlotPrefab, handEquipmentInventorySlotParent);
               handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
            }
            handEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.handEquipmentInventory[i]);
         }
         else
         {
            handEquipmentInventorySlots[i].ClearInventorySlot();
         }
      }

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
      itemStatsWindow.SetActive(false);
   }

   public void ResetAllSelectedItems()
   {
      rightHandSlot01Selected = false;
      rightHandSlot02Selected = false;
      leftHandSlot01Selected = false;
      leftHandSlot02Selected = false;

      headEquipmentSlotSelected = false;
      bodyEquipmentSlotSelected = false;
      legEquipmentSlotSelected = false;
      handEquipmentSlotSelected = false;
   }

}
