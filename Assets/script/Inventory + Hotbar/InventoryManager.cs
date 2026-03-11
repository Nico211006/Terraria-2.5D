using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Database")]
    public List<ItemData> itemDatabase = new List<ItemData>();

    [Header("Sizes")]
    public int hotbarSize = 6;
    public int inventorySize = 8;

    [Header("UI")]
    public InventoryUIUpdater uiUpdater;

    [HideInInspector] public InventorySlotData[] hotbarSlots;
    [HideInInspector] public InventorySlotData[] inventorySlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        hotbarSlots = new InventorySlotData[hotbarSize];
        inventorySlots = new InventorySlotData[inventorySize];

        for (int i = 0; i < hotbarSlots.Length; i++)
            hotbarSlots[i] = new InventorySlotData();

        for (int i = 0; i < inventorySlots.Length; i++)
            inventorySlots[i] = new InventorySlotData();
    }

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (uiUpdater != null)
            uiUpdater.RefreshUI();
    }

    private string NormalizeItemName(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return "";

        return itemName.Trim().ToLower();
    }

    public ItemData GetItemByName(string itemName)
    {
        string searchName = NormalizeItemName(itemName);

        if (string.IsNullOrEmpty(searchName))
            return null;

        for (int i = 0; i < itemDatabase.Count; i++)
        {
            if (itemDatabase[i] == null)
                continue;

            if (NormalizeItemName(itemDatabase[i].itemName) == searchName)
                return itemDatabase[i];
        }

        Debug.LogWarning("Kein ItemData gefunden für: " + itemName);
        return null;
    }

    public InventorySlotData GetSlot(bool isHotbar, int index)
    {
        if (isHotbar)
        {
            if (index >= 0 && index < hotbarSlots.Length)
                return hotbarSlots[index];
        }
        else
        {
            if (index >= 0 && index < inventorySlots.Length)
                return inventorySlots[index];
        }

        return null;
    }

    public InventorySlotData GetHotbarSlot(int index)
    {
        if (index >= 0 && index < hotbarSlots.Length)
            return hotbarSlots[index];

        return null;
    }

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            Debug.LogWarning("AddItem abgebrochen: itemData null oder amount <= 0");
            return;
        }

        Debug.Log("AddItem aufgerufen mit: " + itemData.itemName + " x" + amount);

        int remaining = amount;

        remaining = AddToExistingStacks(hotbarSlots, itemData, remaining);
        remaining = AddToExistingStacks(inventorySlots, itemData, remaining);

        remaining = AddToEmptySlot(hotbarSlots, itemData, remaining);
        remaining = AddToEmptySlot(inventorySlots, itemData, remaining);

        if (remaining > 0)
        {
            Debug.LogWarning("Inventar voll: " + itemData.itemName);
        }

        RefreshUI();
        PrintInventoryState();
    }

    public void AddItem(string itemName, int amount = 1)
    {
        ItemData itemData = GetItemByName(itemName);
        AddItem(itemData, amount);
    }

    private int AddToExistingStacks(InventorySlotData[] slots, ItemData itemData, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].item == itemData)
            {
                slots[i].amount += amount;
                Debug.Log("Stack erweitert in Slot " + i + ": " + itemData.itemName + " x" + slots[i].amount);
                return 0;
            }
        }

        return amount;
    }

    private int AddToEmptySlot(InventorySlotData[] slots, ItemData itemData, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].Set(itemData, amount);
                Debug.Log("Neues Item in leerem Slot " + i + ": " + itemData.itemName + " x" + amount);
                return 0;
            }
        }

        return amount;
    }

    public void MoveSlot(bool fromHotbar, int fromIndex, bool toHotbar, int toIndex)
    {
        InventorySlotData fromSlot = GetSlot(fromHotbar, fromIndex);
        InventorySlotData toSlot = GetSlot(toHotbar, toIndex);

        if (fromSlot == null || toSlot == null)
            return;

        if (fromSlot.IsEmpty())
            return;

        if (fromHotbar == toHotbar && fromIndex == toIndex)
            return;

        if (toSlot.IsEmpty())
        {
            toSlot.Set(fromSlot.item, fromSlot.amount);
            fromSlot.Clear();
        }
        else if (toSlot.item == fromSlot.item)
        {
            toSlot.amount += fromSlot.amount;
            fromSlot.Clear();
        }
        else
        {
            ItemData tempItem = toSlot.item;
            int tempAmount = toSlot.amount;

            toSlot.Set(fromSlot.item, fromSlot.amount);
            fromSlot.Set(tempItem, tempAmount);
        }

        RefreshUI();
        PrintInventoryState();
    }

    public int GetItemCount(ItemData itemData)
    {
        if (itemData == null)
            return 0;

        int total = 0;

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (!hotbarSlots[i].IsEmpty() && hotbarSlots[i].item == itemData)
                total += hotbarSlots[i].amount;
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (!inventorySlots[i].IsEmpty() && inventorySlots[i].item == itemData)
                total += inventorySlots[i].amount;
        }

        return total;
    }

    public int GetItemCount(string itemName)
    {
        ItemData itemData = GetItemByName(itemName);
        return GetItemCount(itemData);
    }

    public bool HasItem(ItemData itemData, int amount = 1)
    {
        return GetItemCount(itemData) >= amount;
    }

    public bool HasItem(string itemName, int amount = 1)
    {
        ItemData itemData = GetItemByName(itemName);
        return HasItem(itemData, amount);
    }

    public bool RemoveItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
            return false;

        if (!HasItem(itemData, amount))
            return false;

        int remaining = amount;

        remaining = RemoveFromSlots(hotbarSlots, itemData, remaining);
        remaining = RemoveFromSlots(inventorySlots, itemData, remaining);

        RefreshUI();
        PrintInventoryState();
        return remaining <= 0;
    }

    public bool RemoveItem(string itemName, int amount = 1)
    {
        ItemData itemData = GetItemByName(itemName);
        return RemoveItem(itemData, amount);
    }

    private int RemoveFromSlots(InventorySlotData[] slots, ItemData itemData, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
                continue;

            if (slots[i].item != itemData)
                continue;

            if (slots[i].amount > amount)
            {
                slots[i].amount -= amount;
                return 0;
            }
            else
            {
                amount -= slots[i].amount;
                slots[i].Clear();

                if (amount <= 0)
                    return 0;
            }
        }

        return amount;
    }

    public bool RemoveOneFromHotbarSlot(int hotbarIndex)
    {
        InventorySlotData slot = GetHotbarSlot(hotbarIndex);

        if (slot == null || slot.IsEmpty())
            return false;

        slot.amount--;

        if (slot.amount <= 0)
            slot.Clear();

        RefreshUI();
        PrintInventoryState();
        return true;
    }

    private void PrintInventoryState()
    {
        string hotbarText = "Hotbar: ";
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i].IsEmpty())
                hotbarText += "[" + i + ": leer] ";
            else
                hotbarText += "[" + i + ": " + hotbarSlots[i].item.itemName + " x" + hotbarSlots[i].amount + "] ";
        }

        string inventoryText = "Inventar: ";
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].IsEmpty())
                inventoryText += "[" + i + ": leer] ";
            else
                inventoryText += "[" + i + ": " + inventorySlots[i].item.itemName + " x" + inventorySlots[i].amount + "] ";
        }

        Debug.Log(hotbarText);
        Debug.Log(inventoryText);
    }
}