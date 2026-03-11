using UnityEngine;

public class PlacementHighlight : MonoBehaviour
{
    public Camera mainCamera;
    public float placeDistance = 100f;

    private GameObject highlightBlock;
    private Renderer highlightRenderer;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        CreateHighlightBlock();
    }

    private void Update()
    {
        UpdateHighlight();
    }

    private void CreateHighlightBlock()
    {
        highlightBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        highlightBlock.name = "PlacementHighlight";

        Destroy(highlightBlock.GetComponent<Collider>());

        highlightBlock.transform.localScale = Vector3.one * 1.02f;

        highlightRenderer = highlightBlock.GetComponent<Renderer>();
        if (highlightRenderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            mat.color = Color.green;
            highlightRenderer.material = mat;
        }

        highlightBlock.SetActive(false);
    }

    private void UpdateHighlight()
    {
        if (mainCamera == null || GridWorldBuilder.instance == null)
        {
            if (highlightBlock != null)
                highlightBlock.SetActive(false);
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, placeDistance))
        {
            highlightBlock.SetActive(false);
            return;
        }

        Block hitBlock = hit.collider.GetComponent<Block>();
        if (hitBlock == null)
        {
            highlightBlock.SetActive(false);
            return;
        }

        int targetGridX = hitBlock.gridX;
        int targetGridY = hitBlock.gridY + 1;

        Vector3 targetPosition = new Vector3(targetGridX, targetGridY, 0);
        highlightBlock.transform.position = targetPosition;

        bool isFree = GridWorldBuilder.instance.IsAirAt(targetGridX, targetGridY);

        if (highlightRenderer != null)
        {
            highlightRenderer.material.color = isFree ? Color.green : Color.red;
        }

        highlightBlock.SetActive(true);
    }
}