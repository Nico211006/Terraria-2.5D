using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;
    public TextMeshProUGUI amountText;

    [Header("Slot Info")]
    public bool isHotbar;
    public int slotIndex;

    private static bool isDragging = false;
    private static bool draggedFromHotbar;
    private static int draggedFromIndex;

    private void Start()
    {
        ClearSlot();
    }

    public void SetSlot(ItemData item, int amount)
    {
        if (item != null && amount > 0)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
                itemIcon.enabled = true;
                itemIcon.color = Color.white;
            }

            if (amountText != null)
            {
                amountText.text = amount > 1 ? amount.ToString() : "";
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        if (amountText != null)
        {
            amountText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null)
            return;

        if (!isDragging)
        {
            InventorySlotData clickedSlot = InventoryManager.Instance.GetSlot(isHotbar, slotIndex);

            if (clickedSlot == null || clickedSlot.IsEmpty())
                return;

            isDragging = true;
            draggedFromHotbar = isHotbar;
            draggedFromIndex = slotIndex;
            return;
        }

        if (draggedFromHotbar == isHotbar && draggedFromIndex == slotIndex)
        {
            isDragging = false;
            return;
        }

        InventoryManager.Instance.MoveSlot(draggedFromHotbar, draggedFromIndex, isHotbar, slotIndex);
        isDragging = false;
    }
}