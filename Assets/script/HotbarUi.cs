using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;
    public TextMeshProUGUI slot4Text;
    public TextMeshProUGUI slot5Text;

    [Header("Icons")]
    public Image slot1Icon;
    public Image slot2Icon;
    public Image slot3Icon;
    public Image slot4Icon;
    public Image slot5Icon;

    [Header("Item Data")]
    public ItemData earthItem;
    public ItemData stoneItem;
    public ItemData grassItem;

    private void Update()
    {
        if (InventoryManager.Instance == null)
            return;

        int earthCount = InventoryManager.Instance.GetItemCount("Erde");
        int stoneCount = InventoryManager.Instance.GetItemCount("Stein");
        int grassCount = InventoryManager.Instance.GetItemCount("Gras");

        UpdateSlot(slot1Text, slot1Icon, earthItem, earthCount);
        UpdateSlot(slot2Text, slot2Icon, stoneItem, stoneCount);
        UpdateSlot(slot3Text, slot3Icon, grassItem, grassCount);
        UpdateEmptySlot(slot4Text, slot4Icon);
        UpdateEmptySlot(slot5Text, slot5Icon);
    }

    private void UpdateSlot(TextMeshProUGUI slotText, Image slotIcon, ItemData item, int count)
    {
        if (count > 0)
        {
            if (slotText != null)
                slotText.text = count.ToString();

            if (slotIcon != null)
            {
                slotIcon.enabled = true;
                slotIcon.sprite = item != null ? item.icon : null;
            }
        }
        else
        {
            if (slotText != null)
                slotText.text = "Leer";

            if (slotIcon != null)
            {
                slotIcon.sprite = null;
                slotIcon.enabled = false;
            }
        }
    }

    private void UpdateEmptySlot(TextMeshProUGUI slotText, Image slotIcon)
    {
        if (slotText != null)
            slotText.text = "Leer";

        if (slotIcon != null)
        {
            slotIcon.sprite = null;
            slotIcon.enabled = false;
        }
    }
}