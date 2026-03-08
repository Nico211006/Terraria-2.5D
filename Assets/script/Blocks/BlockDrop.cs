using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    public string dropName;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("Kein InventoryManager gefunden!");
            return;
        }

        InventoryManager.Instance.AddItem(dropName, 1);
        Destroy(gameObject);
    }
}