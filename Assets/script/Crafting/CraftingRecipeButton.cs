using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CraftingRecipeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Recipe")]
    public ItemData requiredItem;
    public int requiredAmount = 1;

    public ItemData resultItem;
    public int resultAmount = 1;

    [Header("UI")]
    public Image recipeIcon;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    private void Start()
    {
        UpdateRecipeVisual();
        HideTooltip();
    }

    private void UpdateRecipeVisual()
    {
        if (recipeIcon != null && resultItem != null)
        {
            recipeIcon.sprite = resultItem.icon;
        }

        if (tooltipText != null && requiredItem != null)
        {
            tooltipText.text = requiredAmount + " " + requiredItem.itemName;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Craft();
    }

    private void Craft()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager fehlt.");
            return;
        }

        if (requiredItem == null || resultItem == null)
        {
            Debug.LogWarning("RequiredItem oder ResultItem fehlt.");
            return;
        }

        if (!InventoryManager.Instance.HasItem(requiredItem, requiredAmount))
        {
            Debug.Log("Nicht genug Material für: " + resultItem.itemName);
            return;
        }

        bool removed = InventoryManager.Instance.RemoveItem(requiredItem, requiredAmount);

        if (!removed)
        {
            Debug.LogWarning("Material konnte nicht entfernt werden.");
            return;
        }

        InventoryManager.Instance.AddItem(resultItem, resultAmount);
        Debug.Log("Crafting erfolgreich: " + resultItem.itemName);
    }

    private void ShowTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(true);
    }

    private void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }
}