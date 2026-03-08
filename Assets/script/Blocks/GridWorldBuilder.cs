using UnityEngine;

public class GridWorldBuilder : MonoBehaviour
{
    public int width = 10;
    public float blockSize = 1f;

    public Material grassMaterial;
    public Material dirtMaterial;
    public Material stoneMaterial;

    private BlockType[,] worldData;

    void Start()
    {
        BuildWorld();
    }

    void BuildWorld()
    {
        worldData = new BlockType[width, 3];

        for (int x = 0; x < width; x++)
        {
            CreateBlock(x, 0, BlockType.Grass, 2, grassMaterial);
            worldData[x, 0] = BlockType.Grass;

            CreateBlock(x, -1, BlockType.Dirt, 3, dirtMaterial);
            worldData[x, 1] = BlockType.Dirt;

            CreateBlock(x, -2, BlockType.Stone, 5, stoneMaterial);
            worldData[x, 2] = BlockType.Stone;
        }
    }

    void CreateBlock(int gridX, int gridY, BlockType type, int hits, Material mat)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);

        block.name = type.ToString() + "_" + gridX + "_" + gridY;
        block.transform.position = new Vector3(gridX * blockSize, gridY * blockSize, 0);
        block.transform.localScale = Vector3.one * blockSize;
        block.transform.SetParent(transform);

        if (mat != null)
        {
            Renderer rend = block.GetComponent<Renderer>();
            rend.material = mat;
        }

        Block blockScript = block.AddComponent<Block>();
        blockScript.blockType = type;
        blockScript.gridX = gridX;
        blockScript.gridY = gridY;
        blockScript.maxHits = hits;
        blockScript.currentHits = 0;
    }
}