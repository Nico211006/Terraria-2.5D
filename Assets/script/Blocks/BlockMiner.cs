using UnityEngine;

public class BlockMiner : MonoBehaviour
{
    public Camera mainCamera;
    public float mineDistance = 100f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryMineBlock();
        }
    }

    void TryMineBlock()
    {
        if (mainCamera == null)
        {
            Debug.Log("Keine Kamera gefunden");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * mineDistance, Color.red, 2f);

        RaycastHit[] hits = Physics.RaycastAll(ray, mineDistance);

        if (hits.Length == 0)
        {
            Debug.Log("Raycast hat nichts getroffen");
            return;
        }

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Getroffen: " + hit.collider.name);

            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                Debug.Log("Block gefunden: " + hit.collider.name);
                block.HitBlock();
                return;
            }
        }

        Debug.Log("Es wurde etwas getroffen, aber kein Block");
    }
}