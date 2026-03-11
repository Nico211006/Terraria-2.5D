using UnityEngine;
using UnityEngine.UI;

public class HotbarSelector : MonoBehaviour
{
    public static HotbarSelector Instance;

    [Header("UI")]
    public Image[] slots;
    public int selectedSlot = 0;

    [Header("Colors")]
    public Color normalColor = new Color(0.35f, 0.35f, 0.35f, 0.9f);
    public Color selectedColor = new Color(1f, 0.85f, 0.2f, 1f);

    [Header("Scale")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1f);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshHotbarVisual();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
    }

    public void SelectSlot(int index)
    {
        if (slots == null || slots.Length == 0)
            return;

        if (index < 0 || index >= slots.Length)
            return;

        selectedSlot = index;
        RefreshHotbarVisual();
    }

    public void RefreshHotbarVisual()
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            bool isSelected = (i == selectedSlot);

            slots[i].color = isSelected ? selectedColor : normalColor;
            slots[i].rectTransform.localScale = isSelected ? selectedScale : normalScale;
        }
    }

    public int GetSelectedSlotIndex()
    {
        return selectedSlot;
    }

    public InventorySlotData GetSelectedSlotData()
    {
        if (InventoryManager.Instance == null)
            return null;

        return InventoryManager.Instance.GetHotbarSlot(selectedSlot);
    }

    public ItemData GetSelectedItemData()
    {
        InventorySlotData slot = GetSelectedSlotData();

        if (slot == null || slot.IsEmpty())
            return null;

        return slot.item;
    }
}