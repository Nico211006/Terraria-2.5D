using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public Camera mainCamera;
    public float placeDistance = 100f;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

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
            Debug.Log("BlockPlacer: HotbarSelector fehlt.");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.Log("BlockPlacer: InventoryManager fehlt.");
            return;
        }

        if (GridWorldBuilder.instance == null)
        {
            Debug.Log("BlockPlacer: GridWorldBuilder fehlt.");
            return;
        }

        if (mainCamera == null)
        {
            Debug.Log("BlockPlacer: Kamera fehlt.");
            return;
        }

        ItemData selectedItem = HotbarSelector.Instance.GetSelectedItemData();
        if (selectedItem == null)
        {
            Debug.Log("BlockPlacer: Kein Item ausgewählt.");
            return;
        }

        BlockType blockTypeToPlace;
        if (!TryGetBlockTypeFromItem(selectedItem, out blockTypeToPlace))
        {
            Debug.Log("BlockPlacer: Item ist nicht platzierbar: " + selectedItem.itemName);
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, placeDistance);

        if (hits == null || hits.Length == 0)
        {
            Debug.Log("BlockPlacer: Raycast hat nichts getroffen.");
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Block hitBlock = null;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider == null)
                continue;

            Block block = hits[i].collider.GetComponent<Block>();
            if (block != null)
            {
                hitBlock = block;
                break;
            }
        }

        if (hitBlock == null)
        {
            Debug.Log("BlockPlacer: Kein Block im Raycast gefunden.");
            return;
        }

        int targetGridX = hitBlock.gridX;
        int targetGridY = hitBlock.gridY + 1;

        if (!GridWorldBuilder.instance.IsAirAt(targetGridX, targetGridY))
        {
            Debug.Log("BlockPlacer: Ziel ist nicht frei.");
            return;
        }

        GridWorldBuilder.instance.PlaceBlockAt(targetGridX, targetGridY, blockTypeToPlace);
        InventoryManager.Instance.RemoveOneFromHotbarSlot(HotbarSelector.Instance.GetSelectedSlotIndex());

        Debug.Log("Block platziert: " + blockTypeToPlace + " bei X=" + targetGridX + " Y=" + targetGridY);
    }

    private bool TryGetBlockTypeFromItem(ItemData itemData, out BlockType blockType)
    {
        blockType = BlockType.Air;

        if (itemData == null || string.IsNullOrWhiteSpace(itemData.itemName))
            return false;

        string itemName = itemData.itemName.Trim();

        switch (itemName)
        {
            case "Gras":
                blockType = BlockType.Grass;
                return true;

            case "Erde":
                blockType = BlockType.Dirt;
                return true;

            case "Stein":
                blockType = BlockType.Stone;
                return true;

            case "Kohle":
                blockType = BlockType.CoalOre;
                return true;

            case "Kupfererz":
                blockType = BlockType.CopperOre;
                return true;

            case "Eisenerz":
                blockType = BlockType.IronOre;
                return true;

            case "Holz":
                blockType = BlockType.Wood;
                return true;

            case "Blätter":
                blockType = BlockType.Leaves;
                return true;

            case "Holzplanken":
                blockType = BlockType.WoodPlank;
                return true;

            case "Werkbank":
                blockType = BlockType.Workbench;
                return true;

            case "Ofen":
                blockType = BlockType.Furnace;
                return true;

            default:
                Debug.Log("Dieses Item kann nicht als Block platziert werden: " + itemData.itemName);
                return false;
        }
    }
}