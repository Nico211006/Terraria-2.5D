using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    public string dropName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Aufgehoben: " + dropName);
            Destroy(gameObject);
        }
    }
}