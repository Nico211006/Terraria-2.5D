using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    public int gridX;
    public int gridY;
    public int maxHits = 3;
    public int currentHits = 0;

    private Renderer rend;
    private Color startColor;

    private void Awake()
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

        if (BlockHitEffect.Instance != null)
        {
            BlockHitEffect.Instance.PlayHitEffect(transform.position, GetHitParticleColor());
        }

        UpdateVisual();

        if (currentHits >= maxHits)
        {
            DestroyBlock();
        }
    }

    private void UpdateVisual()
    {
        if (rend == null)
            return;

        float damagePercent = (float)currentHits / maxHits;
        rend.material.color = Color.Lerp(startColor, Color.black, damagePercent * 0.5f);
    }

    private void DestroyBlock()
    {
        SpawnDrop();
        Debug.Log("Block zerstört bei Grid-Position: X=" + gridX + " Y=" + gridY);
        Destroy(gameObject);
    }

    private void SpawnDrop()
    {
        GameObject drop = GameObject.CreatePrimitive(PrimitiveType.Cube);
        drop.transform.position = transform.position + Vector3.up * 0.5f;
        drop.transform.localScale = Vector3.one * 0.4f;
        drop.name = GetDropName();

        Collider col = drop.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }

        Rigidbody rb = drop.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.freezeRotation = true;

        BlockDrop blockDrop = drop.AddComponent<BlockDrop>();
        blockDrop.dropName = GetDropName();

        Renderer dropRenderer = drop.GetComponent<Renderer>();
        if (dropRenderer != null)
        {
            dropRenderer.material.color = GetDropColor();
        }
    }

    private string GetDropName()
    {
        switch (blockType)
        {
            case BlockType.Grass:
                return "Gras";
            case BlockType.Dirt:
                return "Erde";
            case BlockType.Stone:
                return "Stein";
            default:
                return "Unbekannt";
        }
    }

    private Color GetDropColor()
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

    private Color GetHitParticleColor()
    {
        switch (blockType)
        {
            case BlockType.Grass:
                return new Color(0.3f, 0.8f, 0.3f);
            case BlockType.Dirt:
                return new Color(0.55f, 0.27f, 0.07f);
            case BlockType.Stone:
                return Color.gray;
            default:
                return Color.white;
        }
    }
}