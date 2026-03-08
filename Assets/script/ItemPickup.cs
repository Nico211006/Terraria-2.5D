using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public void Collect(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Item eingesammelt: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect(other);
    }
}