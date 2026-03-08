using UnityEngine;

public class BlockMiner : MonoBehaviour
{
    public Camera mainCamera;
    public float mineDistance = 100f;

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
            Debug.Log("Main Camera fehlt im BlockMiner!");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * mineDistance, Color.red, 2f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, mineDistance))
        {
            Debug.Log("Getroffen: " + hit.collider.name);

            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                block.HitBlock();
            }
        }
        else
        {
            Debug.Log("Raycast hat nichts getroffen");
        }
    }
}