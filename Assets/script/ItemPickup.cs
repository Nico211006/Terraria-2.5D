using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public void Collect(Collider other)
    {
        if (other == null)
            return;

        if (!other.CompareTag("Player"))
            return;

        BlockItem blockItem = GetComponent<BlockItem>();

        if (blockItem == null)
            return;

        if (blockItem.itemData == null)
            return;

        if (InventoryManager.Instance == null)
            return;

        InventoryManager.Instance.AddItem(blockItem.itemData, 1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect(other);
    }
}