using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject craftingPanel;

    private void Start()
    {
        if (craftingPanel != null)
        {
            craftingPanel.SetActive(false);
        }
    }

    public void ToggleCraftingPanel()
    {
        if (craftingPanel == null)
            return;

        craftingPanel.SetActive(!craftingPanel.activeSelf);
    }

    public void OpenCraftingPanel()
    {
        if (craftingPanel == null)
            return;

        craftingPanel.SetActive(true);
    }

    public void CloseCraftingPanel()
    {
        if (craftingPanel == null)
            return;

        craftingPanel.SetActive(false);
    }
}