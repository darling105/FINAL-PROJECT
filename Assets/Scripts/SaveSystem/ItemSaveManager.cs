using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
	[SerializeField] ItemDatabase itemDatabase;

	private const string InventoryFileName = "Inventory";
	private const string EquipmentFileName = "Equipment";

	public List<InventoryItem> LoadInventory(InventoryController inventoryController)
	{
		List<InventoryItem> continueItems = new List<InventoryItem>();
		ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(InventoryFileName);
		// if (savedSlots == null) return;
		//inventoryController.initialItems.Clear();
		for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
		{

			//InventoryItem itemSlot = inventoryController.initialItems[i];
			ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];
			InventoryItem itemSlot;

			if (savedSlot == null)
			{
				Debug.Log("index i:"+ i);
				itemSlot.item = null;
				itemSlot.quantity = 0;
			}
			else
			{
				itemSlot.item = itemDatabase.GetItemCopy(savedSlot.ItemName);
				itemSlot.quantity = savedSlot.Amount;
			}
			continueItems.Add(itemSlot);
		}
		return continueItems;
	}

	// public void LoadEquipment(Character character)
	// {
	// 	ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(EquipmentFileName);
	// 	if (savedSlots == null) return;

	// 	foreach (ItemSlotSaveData savedSlot in savedSlots.SavedSlots)
	// 	{
	// 		if (savedSlot == null) {
	// 			continue;
	// 		}

	// 		Item item = itemDatabase.GetItemCopy(savedSlot.ItemID);
	// 		character.Inventory.AddItem(item);
	// 		character.Equip((EquippableItem)item);
	// 	}
	// }

	// public void SaveInventory(IList<InventoryItem> itemSlots)
	// {
	// 	SaveItems(itemSlots, InventoryFileName);
	// }

	// public void SaveEquipment(InventoryController inventoryController)
	// {
	// 	SaveItems(inventoryController.equipmentItem, EquipmentFileName);
	// }

	public void SaveItems(IList<InventoryItem> itemSlots, string fileName = InventoryFileName)
	{
		var saveData = new ItemContainerSaveData(itemSlots.Count);

		for (int i = 0; i < saveData.SavedSlots.Length; i++)
		{
			InventoryItem itemSlot = itemSlots[i];

			if (itemSlot.item == null)
			{
				saveData.SavedSlots[i] = null;
			}
			else
			{
				saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.item.Name, itemSlot.quantity);
			}
		}

		ItemSaveIO.SaveItems(saveData, fileName);
	}
}

