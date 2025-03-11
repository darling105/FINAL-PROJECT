using System;

[Serializable]
public class ItemSlotSaveData
{
	public string ItemName;
	public int Amount;

	public ItemSlotSaveData(string name, int amount)
	{
		ItemName = name;
		Amount = amount;
	}
}

[Serializable]
public class ItemContainerSaveData
{
	public ItemSlotSaveData[] SavedSlots;

	public ItemContainerSaveData(int numItems)
	{
		SavedSlots = new ItemSlotSaveData[numItems];
	}
}

