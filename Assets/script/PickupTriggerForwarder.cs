using UnityEngine;

public class PickupTriggerForwarder : MonoBehaviour
{
    public ItemPickup itemPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (itemPickup != null)
        {
            itemPickup.Collect(other);
        }
    }
}