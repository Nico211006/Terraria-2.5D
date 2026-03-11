using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;
    public static bool inventoryOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryPanel == null)
            {
                Debug.LogWarning("InventoryPanel fehlt!");
                return;
            }

            inventoryOpen = !inventoryOpen;
            inventoryPanel.SetActive(inventoryOpen);
        }
    }
}