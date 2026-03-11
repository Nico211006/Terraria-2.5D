using UnityEngine;

public class GridWorldBuilder : MonoBehaviour
{
    public static GridWorldBuilder instance;

    [Header("World Size")]
    public int width = 60;
    public int depth = 12;
    public int skyHeight = 8;
    public float blockSize = 1f;

    [Header("Materials")]
    public Material grassMaterial;
    public Material dirtMaterial;
    public Material stoneMaterial;
    public Material coalOreMaterial;
    public Material copperOreMaterial;
    public Material ironOreMaterial;
    public Material woodMaterial;
    public Material leavesMaterial;
    public Material woodPlankMaterial;
    public Material workbenchMaterial;
    public Material furnaceMaterial;

    [Header("Layer")]
    public string groundLayerName = "Ground";

    private BlockType[,] worldData;
    private int[] surfaceHeights;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BuildWorld();
    }

    private void BuildWorld()
    {
        worldData = new BlockType[width, depth + skyHeight + 1];
        surfaceHeights = new int[width];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < worldData.GetLength(1); y++)
            {
                worldData[x, y] = BlockType.Air;
            }
        }

        GenerateSurfaceProfile();

        for (int x = 0; x < width; x++)
        {
            int surfaceY = surfaceHeights[x];

            for (int gridY = surfaceY; gridY >= -depth; gridY--)
            {
                BlockType type = GetBaseBlockTypeForPosition(surfaceY, gridY);
                int arrayY = GridYToArrayY(gridY);

                Material mat = GetMaterialForBlockType(type);
                int hits = GetHitsForBlockType(type);

                CreateBlock(x, gridY, type, hits, mat);
                worldData[x, arrayY] = type;
            }
        }

        GenerateOreVeins(BlockType.CoalOre, 12, 2, 4, -4, -8);
        GenerateOreVeins(BlockType.CopperOre, 8, 2, 3, -6, -10);
        GenerateOreVeins(BlockType.IronOre, 6, 2, 3, -8, -12);

        GenerateTrees();
    }

    private void GenerateSurfaceProfile()
    {
        int currentSurfaceY = 0;

        for (int x = 0; x < width; x++)
        {
            int stepChance = Random.Range(0, 100);

            if (stepChance < 20)
                currentSurfaceY += 1;
            else if (stepChance < 40)
                currentSurfaceY -= 1;

            currentSurfaceY = Mathf.Clamp(currentSurfaceY, -2, 0);
            surfaceHeights[x] = currentSurfaceY;
        }
    }

    private BlockType GetBaseBlockTypeForPosition(int surfaceY, int gridY)
    {
        if (gridY == surfaceY)
            return BlockType.Grass;

        int depthBelowSurface = surfaceY - gridY;

        if (depthBelowSurface <= 3)
            return BlockType.Dirt;

        if (depthBelowSurface <= 6)
        {
            float roll = Random.value;
            return roll < 0.35f ? BlockType.Dirt : BlockType.Stone;
        }

        if (depthBelowSurface <= 9)
        {
            float roll = Random.value;
            return roll < 0.15f ? BlockType.Dirt : BlockType.Stone;
        }

        return BlockType.Stone;
    }

    private void GenerateTrees()
    {
        for (int x = 4; x < width - 4; x++)
        {
            if (Random.value > 0.14f)
                continue;

            int groundY = surfaceHeights[x];

            if (!IsBlockTypeAt(x, groundY, BlockType.Grass))
                continue;

            int trunkHeight = Random.Range(4, 6);

            for (int t = 1; t <= trunkHeight; t++)
            {
                ForcePlaceBlockAt(x, groundY + t, BlockType.Wood);
            }

            int topY = groundY + trunkHeight;

            ForcePlaceLeavesAt(x,     topY + 2);

            ForcePlaceLeavesAt(x - 1, topY + 1);
            ForcePlaceLeavesAt(x,     topY + 1);
            ForcePlaceLeavesAt(x + 1, topY + 1);

            ForcePlaceLeavesAt(x - 2, topY);
            ForcePlaceLeavesAt(x - 1, topY);
            ForcePlaceLeavesAt(x,     topY);
            ForcePlaceLeavesAt(x + 1, topY);
            ForcePlaceLeavesAt(x + 2, topY);

            ForcePlaceLeavesAt(x - 1, topY - 1);
            ForcePlaceLeavesAt(x,     topY - 1);
            ForcePlaceLeavesAt(x + 1, topY - 1);

            x += 4;
        }
    }

    private void GenerateOreVeins(BlockType oreType, int veinCount, int minLength, int maxLength, int minDepth, int maxDepth)
    {
        for (int i = 0; i < veinCount; i++)
        {
            int startX = Random.Range(0, width);
            int startY = Random.Range(maxDepth, minDepth + 1);
            int veinLength = Random.Range(minLength, maxLength + 1);

            int currentX = startX;
            int currentY = startY;

            for (int j = 0; j < veinLength; j++)
            {
                ReplaceBlockWithOre(currentX, currentY, oreType);

                currentX += Random.Range(-1, 2);
                currentY += Random.Range(-1, 2);

                currentX = Mathf.Clamp(currentX, 0, width - 1);
                currentY = Mathf.Clamp(currentY, -depth, 0);
            }
        }
    }

    private void ReplaceBlockWithOre(int gridX, int gridY, BlockType oreType)
    {
        int arrayY = GridYToArrayY(gridY);

        if (!IsValidArrayPos(gridX, arrayY))
            return;

        if (worldData[gridX, arrayY] == BlockType.Air)
            return;

        Transform oldBlock = FindBlockAt(gridX, gridY);
        if (oldBlock != null)
            Destroy(oldBlock.gameObject);

        Material mat = GetMaterialForBlockType(oreType);
        int hits = GetHitsForBlockType(oreType);

        CreateBlock(gridX, gridY, oreType, hits, mat);
        worldData[gridX, arrayY] = oreType;
    }

    private Transform FindBlockAt(int gridX, int gridY)
    {
        foreach (Transform child in transform)
        {
            Block block = child.GetComponent<Block>();
            if (block != null && block.gridX == gridX && block.gridY == gridY)
                return child;
        }

        return null;
    }

    private bool IsBlockTypeAt(int gridX, int gridY, BlockType type)
    {
        int arrayY = GridYToArrayY(gridY);

        if (!IsValidArrayPos(gridX, arrayY))
            return false;

        return worldData[gridX, arrayY] == type;
    }

    private void ForcePlaceBlockAt(int gridX, int gridY, BlockType type)
    {
        int arrayY = GridYToArrayY(gridY);

        if (!IsValidArrayPos(gridX, arrayY))
            return;

        Transform oldBlock = FindBlockAt(gridX, gridY);
        if (oldBlock != null)
            Destroy(oldBlock.gameObject);

        Material mat = GetMaterialForBlockType(type);
        int hits = GetHitsForBlockType(type);

        CreateBlock(gridX, gridY, type, hits, mat);
        worldData[gridX, arrayY] = type;
    }

    private void ForcePlaceLeavesAt(int gridX, int gridY)
    {
        ForcePlaceBlockAt(gridX, gridY, BlockType.Leaves);
    }

    private void CreateBlock(int gridX, int gridY, BlockType type, int hits, Material mat)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);

        block.name = type + "_" + gridX + "_" + gridY;

        float zPos = 0f;
        if (type == BlockType.Wood || type == BlockType.Leaves)
        {
            zPos = 0.2f;
        }
        else if (type == BlockType.Workbench || type == BlockType.Furnace)
        {
            zPos = 0.1f;
        }

        block.transform.position = new Vector3(
            gridX * blockSize,
            gridY * blockSize,
            zPos
        );

        block.transform.localScale = Vector3.one * blockSize;
        block.transform.parent = transform;

        int groundLayer = LayerMask.NameToLayer(groundLayerName);
        if (groundLayer != -1)
        {
            block.layer = groundLayer;
        }

        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer != null && mat != null)
        {
            renderer.material = mat;
        }

        BoxCollider collider = block.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = block.AddComponent<BoxCollider>();
        }

        Block blockComponent = block.GetComponent<Block>();
        if (blockComponent == null)
        {
            blockComponent = block.AddComponent<Block>();
        }

        blockComponent.blockType = type;
        blockComponent.gridX = gridX;
        blockComponent.gridY = gridY;
        blockComponent.maxHits = hits;
        blockComponent.currentHits = 0;
    }

    public void PlaceBlockAt(int gridX, int gridY, BlockType type)
    {
        int arrayY = GridYToArrayY(gridY);

        if (!IsValidArrayPos(gridX, arrayY))
        {
            Debug.Log("Ungültige Position zum Platzieren.");
            return;
        }

        if (worldData[gridX, arrayY] != BlockType.Air)
        {
            Debug.Log("Dort ist bereits ein Block.");
            return;
        }

        Material mat = GetMaterialForBlockType(type);
        int hits = GetHitsForBlockType(type);

        CreateBlock(gridX, gridY, type, hits, mat);
        worldData[gridX, arrayY] = type;
    }

    public void RemoveBlockFromData(int gridX, int gridY)
    {
        int arrayY = GridYToArrayY(gridY);

        if (!IsValidArrayPos(gridX, arrayY))
            return;

        worldData[gridX, arrayY] = BlockType.Air;
    }

    public bool IsAirAt(int gridX, int gridY)
    {
        int arrayY = GridYToArrayY(gridY);

        if (!IsValidArrayPos(gridX, arrayY))
            return false;

        return worldData[gridX, arrayY] == BlockType.Air;
    }

    private int GridYToArrayY(int gridY)
    {
        return gridY + depth;
    }

    private bool IsValidArrayPos(int x, int arrayY)
    {
        if (worldData == null)
            return false;

        return x >= 0 &&
               x < worldData.GetLength(0) &&
               arrayY >= 0 &&
               arrayY < worldData.GetLength(1);
    }

    private Material GetMaterialForBlockType(BlockType type)
    {
        switch (type)
        {
            case BlockType.Grass:
                return grassMaterial;

            case BlockType.Dirt:
                return dirtMaterial;

            case BlockType.Stone:
                return stoneMaterial;

            case BlockType.CoalOre:
                return coalOreMaterial;

            case BlockType.CopperOre:
                return copperOreMaterial;

            case BlockType.IronOre:
                return ironOreMaterial;

            case BlockType.Wood:
                return woodMaterial;

            case BlockType.Leaves:
                return leavesMaterial;

            case BlockType.WoodPlank:
                return woodPlankMaterial != null ? woodPlankMaterial : woodMaterial;

            case BlockType.Workbench:
                return workbenchMaterial != null ? workbenchMaterial : woodMaterial;

            case BlockType.Furnace:
                return furnaceMaterial != null ? furnaceMaterial : stoneMaterial;

            default:
                return stoneMaterial;
        }
    }

    private int GetHitsForBlockType(BlockType type)
    {
        switch (type)
        {
            case BlockType.Grass:
            case BlockType.Dirt:
            case BlockType.Leaves:
                return 2;

            case BlockType.Wood:
            case BlockType.WoodPlank:
            case BlockType.Workbench:
                return 3;

            case BlockType.Stone:
            case BlockType.CoalOre:
            case BlockType.CopperOre:
            case BlockType.IronOre:
            case BlockType.Furnace:
                return 4;

            default:
                return 3;
        }
    }
}