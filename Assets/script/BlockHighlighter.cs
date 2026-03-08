using UnityEngine;

public class BlockHighlighter : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject blockHighlight;
    public float highlightDistance = 100f;

    void Start()
    {
        if (blockHighlight != null)
        {
            blockHighlight.SetActive(false);
        }
    }

    void Update()
    {
        HighlightBlock();
    }

    void HighlightBlock()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, highlightDistance))
        {
            Block block = hit.collider.GetComponent<Block>();

            if (block != null)
            {
                blockHighlight.SetActive(true);
                blockHighlight.transform.position = hit.collider.transform.position;
                return;
            }
        }

        blockHighlight.SetActive(false);
    }
}