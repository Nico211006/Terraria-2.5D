using UnityEngine;
using UnityEngine.UI;

public class HotbarSelector : MonoBehaviour
{
    public static HotbarSelector Instance;

    public Image[] slots;
    public int selectedSlot = 0;

    public Color normalColor = new Color(0.7f, 0.7f, 0.7f, 0.8f);
    public Color selectedColor = new Color(1f, 1f, 1f, 1f);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateHotbarVisual();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        selectedSlot = index;
        UpdateHotbarVisual();
    }

    private void UpdateHotbarVisual()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            slots[i].color = (i == selectedSlot) ? selectedColor : normalColor;
        }
    }

    public string GetSelectedItemName()
    {
        switch (selectedSlot)
        {
            case 0:
                return "Erde";
            case 1:
                return "Stein";
            case 2:
                return "Gras";
            default:
                return "";
        }
    }
}