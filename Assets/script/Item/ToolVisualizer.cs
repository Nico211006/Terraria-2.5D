using UnityEngine;

public class ToolVisualizer : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer toolRenderer;

    [Header("Tool Sprites")]
    public Sprite woodPickaxeSprite;
    public Sprite woodAxeSprite;

    private bool firstSelectionHappened = false;
    private int lastSelectedSlot = -1;

    private void Awake()
    {
        if (toolRenderer == null)
            toolRenderer = GetComponent<SpriteRenderer>();

        if (toolRenderer != null)
            toolRenderer.sprite = null;
    }

    private void Update()
    {
        UpdateToolVisual();
    }

    public void UpdateToolVisual()
    {
        if (toolRenderer == null)
            return;

        if (HotbarSelector.Instance == null)
        {
            toolRenderer.sprite = null;
            return;
        }

        int currentSlot = HotbarSelector.Instance.GetSelectedSlotIndex();

        if (lastSelectedSlot == -1)
        {
            lastSelectedSlot = currentSlot;
            toolRenderer.sprite = null;
            return;
        }

        if (currentSlot != lastSelectedSlot)
        {
            firstSelectionHappened = true;
            lastSelectedSlot = currentSlot;
        }

        if (!firstSelectionHappened)
        {
            toolRenderer.sprite = null;
            return;
        }

        ItemData selectedItem = HotbarSelector.Instance.GetSelectedItemData();

        if (selectedItem == null)
        {
            toolRenderer.sprite = null;
            return;
        }

        if (selectedItem.itemName == "Holzspitzhacke")
        {
            toolRenderer.sprite = woodPickaxeSprite;
        }
        else if (selectedItem.itemName == "Holzaxt")
        {
            toolRenderer.sprite = woodAxeSprite;
        }
        else
        {
            toolRenderer.sprite = null;
        }
    }
}