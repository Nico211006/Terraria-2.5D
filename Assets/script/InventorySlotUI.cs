using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;
    public TextMeshProUGUI amountText;

    [Header("Test Item")]
    public ItemData startItem;
    public int startAmount = 1;

    [HideInInspector] public ItemData currentItem;
    [HideInInspector] public int currentAmount;

    private static ItemData draggedItem;
    private static int draggedAmount;

    private void Start()
    {
        if (startItem != null && startAmount > 0)
        {
            SetSlot(startItem, startAmount);
        }
        else
        {
            ClearSlot();
        }
    }

    public void SetSlot(ItemData item, int amount)
    {
        currentItem = item;
        currentAmount = amount;

        if (currentItem != null && currentAmount > 0)
        {
            if (itemIcon != null)
            {
                itemIcon.enabled = true;
                itemIcon.sprite = currentItem.icon;
            }

            if (amountText != null)
            {
                amountText.text = currentAmount > 1 ? currentAmount.ToString() : "";
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentAmount = 0;

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
        if (draggedItem == null)
        {
            if (currentItem == null)
                return;

            draggedItem = currentItem;
            draggedAmount = currentAmount;

            ClearSlot();
        }
        else
        {
            ItemData tempItem = currentItem;
            int tempAmount = currentAmount;

            SetSlot(draggedItem, draggedAmount);

            draggedItem = tempItem;
            draggedAmount = tempAmount;

            if (draggedItem == null)
            {
                draggedAmount = 0;
            }
        }
    }
}