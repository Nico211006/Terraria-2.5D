using UnityEngine;

public class InventoryUIUpdater : MonoBehaviour
{
    public InventorySlotUI[] inventorySlots;
    public InventorySlotUI[] hotbarSlots;

    public void RefreshUI()
    {
        if (InventoryManager.Instance == null)
            return;

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i] == null)
                continue;

            InventorySlotData slotData = InventoryManager.Instance.GetSlot(true, i);

            if (slotData != null && !slotData.IsEmpty())
                hotbarSlots[i].SetSlot(slotData.item, slotData.amount);
            else
                hotbarSlots[i].ClearSlot();
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null)
                continue;

            InventorySlotData slotData = InventoryManager.Instance.GetSlot(false, i);

            if (slotData != null && !slotData.IsEmpty())
                inventorySlots[i].SetSlot(slotData.item, slotData.amount);
            else
                inventorySlots[i].ClearSlot();
        }

        if (HotbarSelector.Instance != null)
            HotbarSelector.Instance.RefreshHotbarVisual();
    }
}