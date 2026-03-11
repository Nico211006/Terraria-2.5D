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
            startColor = rend.material.color;
    }

    public void HitBlock(int damage = 1)
    {
        if (damage <= 0)
            damage = 1;

        currentHits += damage;

        if (currentHits > maxHits)
            currentHits = maxHits;

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
        if (GridWorldBuilder.instance != null)
        {
            GridWorldBuilder.instance.RemoveBlockFromData(gridX, gridY);
        }

        SpawnDrop();
        Destroy(gameObject);
    }

    private void SpawnDrop()
    {
        string dropName = GetDropName();

        if (dropName == "Unbekannt")
            return;

        GameObject drop = GameObject.CreatePrimitive(PrimitiveType.Cube);
        drop.transform.position = transform.position + Vector3.up * 0.5f;
        drop.transform.localScale = Vector3.one * 0.4f;
        drop.name = dropName + "_Drop";

        Rigidbody rb = drop.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = true;

        drop.AddComponent<ItemPickup>();

        BlockItem blockItem = drop.AddComponent<BlockItem>();
        blockItem.itemData = GetDropItemData();
        blockItem.amount = 1;

        Renderer dropRenderer = drop.GetComponent<Renderer>();
        if (dropRenderer != null)
        {
            dropRenderer.material.color = GetDropColor();
        }
    }

    private ItemData GetDropItemData()
    {
        if (InventoryManager.Instance == null)
            return null;

        return InventoryManager.Instance.GetItemByName(GetDropName());
    }

    private string GetDropName()
    {
        switch (blockType)
        {
            case BlockType.Grass: return "Gras";
            case BlockType.Dirt: return "Erde";
            case BlockType.Stone: return "Stein";
            case BlockType.CoalOre: return "Kohle";
            case BlockType.CopperOre: return "Kupfererz";
            case BlockType.IronOre: return "Eisenerz";
            case BlockType.Wood: return "Holz";
            case BlockType.Leaves: return "Blätter";
            case BlockType.WoodPlank: return "Holzplanken";
            case BlockType.Workbench: return "Werkbank";
            case BlockType.Furnace: return "Ofen";
            default: return "Unbekannt";
        }
    }

    private Color GetDropColor()
    {
        switch (blockType)
        {
            case BlockType.Grass: return Color.green;
            case BlockType.Dirt: return new Color(0.55f, 0.27f, 0.07f);
            case BlockType.Stone: return Color.gray;
            case BlockType.CoalOre: return new Color(0.15f, 0.15f, 0.15f);
            case BlockType.CopperOre: return new Color(0.72f, 0.45f, 0.2f);
            case BlockType.IronOre: return new Color(0.75f, 0.75f, 0.8f);
            case BlockType.Wood: return new Color(0.45f, 0.25f, 0.1f);
            case BlockType.Leaves: return new Color(0.2f, 0.7f, 0.2f);
            case BlockType.WoodPlank: return new Color(0.72f, 0.52f, 0.28f);
            case BlockType.Workbench: return new Color(0.50f, 0.30f, 0.12f);
            case BlockType.Furnace: return new Color(0.35f, 0.35f, 0.38f);
            default: return Color.white;
        }
    }
}