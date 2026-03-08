using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    public int maxHits = 3;
    public int currentHits = 0;

    private Renderer rend;
    private Color startColor;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        if (rend != null)
        {
            startColor = rend.material.color;
        }
    }

    public void HitBlock()
    {
        currentHits++;

        UpdateVisual();

        if (currentHits >= maxHits)
        {
            DestroyBlock();
        }
    }

    void UpdateVisual()
    {
        if (rend == null) return;

        float damagePercent = (float)currentHits / maxHits;
        rend.material.color = Color.Lerp(startColor, Color.black, damagePercent * 0.5f);
    }

    void DestroyBlock()
    {
        SpawnDrop();
        Destroy(gameObject);
    }

    void SpawnDrop()
    {
        GameObject drop = GameObject.CreatePrimitive(PrimitiveType.Cube);
        drop.transform.position = transform.position + Vector3.up * 0.5f;
        drop.transform.localScale = Vector3.one * 0.4f;
        drop.name = GetDropName();

        Collider col = drop.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        Rigidbody rb = drop.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        BlockDrop blockDrop = drop.AddComponent<BlockDrop>();
        blockDrop.dropName = GetDropName();

        Renderer dropRenderer = drop.GetComponent<Renderer>();
        if (dropRenderer != null)
        {
            dropRenderer.material.color = GetDropColor();
        }
    }

    string GetDropName()
    {
        switch (blockType)
        {
            case BlockType.Grass:
                return "GrassDrop";
            case BlockType.Dirt:
                return "DirtDrop";
            case BlockType.Stone:
                return "StoneDrop";
            case BlockType.Air:
                return "Nothing";
            default:
                return "UnknownDrop";
        }
    }

    Color GetDropColor()
    {
        switch (blockType)
        {
            case BlockType.Grass:
                return Color.green;
            case BlockType.Dirt:
                return new Color(0.55f, 0.27f, 0.07f);
            case BlockType.Stone:
                return Color.gray;
            default:
                return Color.white;
        }
    }
}