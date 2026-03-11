using UnityEngine;

public class BlockMiner : MonoBehaviour
{
    public Camera mainCamera;
    public float mineDistance = 100f;

    [Header("Mining")]
    public float mineRate = 0.2f; // Zeit zwischen Schlägen

    private float nextMineTime;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (InventoryToggle.inventoryOpen)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextMineTime)
        {
            nextMineTime = Time.time + mineRate;
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
        Debug.DrawRay(ray.origin, ray.direction * mineDistance, Color.red, 0.2f);

        RaycastHit[] hits = Physics.RaycastAll(ray, mineDistance);

        if (hits.Length == 0)
        {
            Debug.Log("Raycast hat nichts getroffen");
            return;
        }

        foreach (RaycastHit hit in hits)
        {
            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                int damage = GetMiningDamage(block);
                block.HitBlock(damage);
                return;
            }
        }

        Debug.Log("Es wurde etwas getroffen, aber kein Block");
    }

    int GetMiningDamage(Block block)
    {
        if (block == null)
            return 1;

        bool hasWoodPickaxeEquipped = HasItemEquipped("Holzspitzhacke");
        bool hasWoodAxeEquipped = HasItemEquipped("Holzaxt");

        switch (block.blockType)
        {
            case BlockType.Grass:
            case BlockType.Dirt:
                return 1;

            case BlockType.Stone:
            case BlockType.CoalOre:
            case BlockType.CopperOre:
            case BlockType.IronOre:
                return hasWoodPickaxeEquipped ? 2 : 1;

            case BlockType.Wood:
            case BlockType.Leaves:
                return hasWoodAxeEquipped ? 2 : 1;

            default:
                return 1;
        }
    }

    bool HasItemEquipped(string itemName)
    {
        if (HotbarSelector.Instance == null)
            return false;

        ItemData selectedItem = HotbarSelector.Instance.GetSelectedItemData();

        if (selectedItem == null)
            return false;

        return selectedItem.itemName == itemName;
    }
}