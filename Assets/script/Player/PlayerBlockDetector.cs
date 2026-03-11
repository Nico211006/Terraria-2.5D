using UnityEngine;

public class PlayerBlockDetector : MonoBehaviour
{
    public Transform blockCheckPoint;
    public float blockCheckRadius = 0.7f;
    public float breakInterval = 0.5f;

    private float breakTimer = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            breakTimer += Time.deltaTime;

            if (breakTimer >= breakInterval)
            {
                TryBreakBlock();
                breakTimer = 0f;
            }
        }
        else
        {
            breakTimer = 0f;
        }
    }

    void TryBreakBlock()
    {
        if (blockCheckPoint == null)
        {
            Debug.Log("BlockCheckPoint fehlt");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(blockCheckPoint.position, blockCheckRadius);

        foreach (Collider hit in hits)
        {
            BlockHealth blockHealth = hit.GetComponent<BlockHealth>();

            if (blockHealth != null)
            {
                blockHealth.TakeDamage(1);
                Debug.Log("Block getroffen: " + hit.name);
                return;
            }
        }

        Debug.Log("Kein Block getroffen");
    }

    void OnDrawGizmosSelected()
    {
        if (blockCheckPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(blockCheckPoint.position, blockCheckRadius);
    }
}