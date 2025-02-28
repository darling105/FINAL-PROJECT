


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public ItemSaveManager SaveSystem;
        public bool Continue;
        [SerializeField] private SlotPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();
        public InventoryItem equipmentItem;

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;

        private void Start()
        {

            PrepareUI();
            PrepareInventoryData();
            PrepareEquipmentData();
            inventoryUI.ShowEquiptmentData += HandleShowEquiptmentUI;
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            if(Continue){
                initialItems = SaveSystem.LoadInventory(this);
            }

            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }

        }
        private void HandleShowEquiptmentUI()
        {
            foreach (var item in inventoryData.GetCurrentEquipmentState())
            {
                inventoryUI.UpdateEquipmentData(item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }
        private void PrepareEquipmentData()
        {
            inventoryData.OnEquipmentUpdated += UpdateEquipmentUI;
            inventoryData.AddItemToEquipment(equipmentItem);
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void UpdateEquipmentUI(Dictionary<int, InventoryItem> inventoryState)
        {
            //inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateEquipmentData(item.Key, item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
            inventoryUI.OnUnequipFromEquipment += HandleRemoveItemFromEquipment;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex, inventoryUI._inventoryPanelIsOpen);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject);
                //inventoryUI.ShowItemAction(itemIndex);
                //inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                //inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
                inventoryData.RemoveItem(itemIndex, 1);
            }

        }
        private void HandleUnequipFromEquipment()
        {

        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            // InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            // if (inventoryItem.IsEmpty)
            //     return;

            // IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            // if (destroyableItem != null)
            // {
            //     inventoryData.RemoveItem(itemIndex, 1);
            // }

            // IItemAction itemAction = inventoryItem.item as IItemAction;
            // if (itemAction != null)
            // {
            //     itemAction.PerformAction(gameObject, inventoryItem.itemState);
            //     audioSource.PlayOneShot(itemAction.actionSFX);
            //     if (inventoryData.GetItemAt(itemIndex).IsEmpty)
            //         inventoryUI.ResetSelection();
            // }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex, inventoryUI._inventoryPanelIsOpen);
            if (inventoryItem.IsEmpty)
            {
                Debug.Log("itemIndex : " + itemIndex);
                return;
            }

            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex, bool isEnableInventory)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex, isEnableInventory);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            //string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
                item.name, item.Description);
        }

        private void HandleRemoveItemFromEquipment(int itemIndex = 0)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex, false);
            if (inventoryItem.IsEmpty)
            {
                //inventoryUI.ResetSelection();
                return;
            }
            inventoryData.UnEquipFromEquipment(itemIndex);


        }

        // private string PrepareDescription(InventoryItem inventoryItem)
        // {
        //     StringBuilder sb = new StringBuilder();
        //     sb.Append(inventoryItem.item.Description);
        //     sb.AppendLine();
        //     for (int i = 0; i < inventoryItem.itemState.Count; i++)
        //     {
        //         sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
        //             $": {inventoryItem.itemState[i].value} / " +
        //             $"{inventoryItem.item.DefaultParametersList[i].value}");
        //         sb.AppendLine();
        //     }
        //     return sb.ToString();
        // }

        public void Update()
        {

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {

                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                    }
                }
                else
                {

                    inventoryUI.Hide();
                }

            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SaveSystem.SaveItems(inventoryData.InventoryItems);
            }
        }
    }
}
