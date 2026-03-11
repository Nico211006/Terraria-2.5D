using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool pickedUp = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (pickedUp)
            return;

        if (collision == null || collision.gameObject == null)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        BlockItem blockItem = GetComponent<BlockItem>();

        if (blockItem == null)
            blockItem = GetComponentInParent<BlockItem>();

        if (blockItem == null)
        {
            Debug.LogWarning("Kein BlockItem am Drop gefunden: " + gameObject.name);
            return;
        }

        if (blockItem.itemData == null)
        {
            Debug.LogWarning("BlockItem hat kein itemData: " + gameObject.name);
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("Kein InventoryManager gefunden.");
            return;
        }

        pickedUp = true;
        InventoryManager.Instance.AddItem(blockItem.itemData, blockItem.amount);
        Destroy(gameObject);
    }
}