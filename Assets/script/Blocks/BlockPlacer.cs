using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject stonePrefab;
    public GameObject grassPrefab;

    public Camera mainCamera;
    public float buildPlaneZ = 0f;
    public float gridSize = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryPlaceBlock();
        }
    }

    private void TryPlaceBlock()
    {
        if (HotbarSelector.Instance == null)
        {
            Debug.LogWarning("HotbarSelector fehlt!");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager fehlt!");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera fehlt im BlockPlacer!");
            return;
        }

        string selectedItem = HotbarSelector.Instance.GetSelectedItemName();
        Debug.Log("Ausgewähltes Item: " + selectedItem);

        if (string.IsNullOrEmpty(selectedItem))
        {
            Debug.Log("Kein platzierbares Item ausgewählt.");
            return;
        }

        if (!InventoryManager.Instance.HasItem(selectedItem, 1))
        {
            Debug.Log("Item nicht im Inventar: " + selectedItem);
            return;
        }

        Plane buildPlane = new Plane(Vector3.forward, new Vector3(0f, 0f, buildPlaneZ));
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!buildPlane.Raycast(ray, out float enter))
        {
            Debug.Log("Maus trifft Bauebene nicht.");
            return;
        }

        Vector3 worldPos = ray.GetPoint(enter);

        Vector3 placePos = new Vector3(
            Mathf.Round(worldPos.x / gridSize) * gridSize,
            Mathf.Round(worldPos.y / gridSize) * gridSize,
            buildPlaneZ
        );

        Collider[] overlaps = Physics.OverlapBox(placePos, Vector3.one * 0.4f);
        if (overlaps.Length > 0)
        {
            Debug.Log("Platz ist schon belegt.");
            return;
        }

        GameObject prefabToPlace = GetPrefabForItem(selectedItem);
        if (prefabToPlace == null)
        {
            Debug.LogWarning("Kein Prefab gefunden für: " + selectedItem);
            return;
        }

        Instantiate(prefabToPlace, placePos, Quaternion.identity);
        InventoryManager.Instance.RemoveItem(selectedItem, 1);

        Debug.Log("Block platziert: " + selectedItem + " bei " + placePos);
    }

    private GameObject GetPrefabForItem(string itemName)
    {
        switch (itemName)
        {
            case "Erde":
                return dirtPrefab;
            case "Stein":
                return stonePrefab;
            case "Gras":
                return grassPrefab;
            default:
                return null;
        }
    }
}