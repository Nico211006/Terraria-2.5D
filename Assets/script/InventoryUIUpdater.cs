using System.Collections.Generic;
using UnityEngine;

public class InventoryUIUpdater : MonoBehaviour
{
    public InventorySlotUI[] inventorySlots;
    public InventorySlotUI[] hotbarSlots;

    private void Update()
    {
        UpdateAllSlots();
    }

    private void UpdateAllSlots()
    {
        if (InventoryManager.Instance == null)
            return;

        Dictionary<ItemData, int> items = InventoryManager.Instance.GetItems();
        List<KeyValuePair<ItemData, int>> itemList = new List<KeyValuePair<ItemData, int>>(items);

        int index = 0;

        if (hotbarSlots != null)
        {
            for (int i = 0; i < hotbarSlots.Length; i++)
            {
                if (hotbarSlots[i] == null)
                    continue;

                if (index < itemList.Count)
                {
                    hotbarSlots[i].SetSlot(itemList[index].Key, itemList[index].Value);
                    index++;
                }
                else
                {
                    hotbarSlots[i].ClearSlot();
                }
            }
        }

        if (inventorySlots != null)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i] == null)
                    continue;

                if (index < itemList.Count)
                {
                    inventorySlots[i].SetSlot(itemList[index].Key, itemList[index].Value);
                    index++;
                }
                else
                {
                    inventorySlots[i].ClearSlot();
                }
            }
        }
    }
}