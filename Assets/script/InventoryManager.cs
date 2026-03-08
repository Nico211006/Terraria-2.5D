using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public TextMeshProUGUI inventoryText;
    public List<ItemData> itemDatabase = new List<ItemData>();

    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

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
    }

    private void Start()
    {
        UpdateUI();
    }

    private ItemData GetItemByName(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return null;

        for (int i = 0; i < itemDatabase.Count; i++)
        {
            if (itemDatabase[i] != null && itemDatabase[i].itemName == itemName)
                return itemDatabase[i];
        }

        Debug.LogWarning("Kein ItemData gefunden für: " + itemName);
        return null;
    }

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null)
            return;

        if (items.ContainsKey(itemData))
            items[itemData] += amount;
        else
            items[itemData] = amount;

        Debug.Log(itemData.itemName + " im Inventar: " + items[itemData]);
        UpdateUI();
    }

    public void AddItem(string itemName, int amount = 1)
    {
        ItemData itemData = GetItemByName(itemName);
        AddItem(itemData, amount);
    }

    public bool HasItem(ItemData itemData, int amount = 1)
    {
        return itemData != null && items.ContainsKey(itemData) && items[itemData] >= amount;
    }

    public bool HasItem(string itemName, int amount = 1)
    {
        ItemData itemData = GetItemByName(itemName);
        return HasItem(itemData, amount);
    }

    public bool RemoveItem(ItemData itemData, int amount = 1)
    {
        if (!HasItem(itemData, amount))
            return false;

        items[itemData] -= amount;

        if (items[itemData] <= 0)
            items.Remove(itemData);

        UpdateUI();
        return true;
    }

    public bool RemoveItem(string itemName, int amount = 1)
    {
        ItemData itemData = GetItemByName(itemName);
        return RemoveItem(itemData, amount);
    }

    public int GetItemCount(ItemData itemData)
    {
        if (itemData != null && items.ContainsKey(itemData))
            return items[itemData];

        return 0;
    }

    public int GetItemCount(string itemName)
    {
        ItemData itemData = GetItemByName(itemName);
        return GetItemCount(itemData);
    }

    public Dictionary<ItemData, int> GetItems()
    {
        return items;
    }

    private void UpdateUI()
    {
        if (inventoryText == null)
            return;

        if (items.Count == 0)
        {
            inventoryText.text = "Inventar leer";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Inventar:");

        foreach (KeyValuePair<ItemData, int> entry in items)
        {
            if (entry.Key != null)
                sb.AppendLine(entry.Key.itemName + ": " + entry.Value);
        }

        inventoryText.text = sb.ToString();
    }
}