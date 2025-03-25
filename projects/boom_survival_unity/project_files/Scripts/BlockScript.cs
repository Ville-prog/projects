using UnityEngine;
using System.Collections.Generic;

 /*
 * This class is responsible for generating the terrain and placing blocks in the 
 * game world. It uses various parameters to determine the types and placement of blocks.
 */
public class BlockScript : MonoBehaviour
{
    // Sprites for blocks and gameobject referances. "[SerializeField]" allows
    // private fields to be visible and editable in the Unity Inspector.
    [SerializeField] private Sprite grassSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite dirtSprite;
    [SerializeField] private Sprite magmaSprite;
    [SerializeField] private Sprite cabinSprite;
    [SerializeField] private Sprite cabinBackroundSprite;
    [SerializeField] private Sprite cabinDoorSprite;
    [SerializeField] private Sprite roofSprite;
    [SerializeField] private Sprite woodSprite;
    [SerializeField] private Sprite claySprite;
    [SerializeField] private Sprite sandSprite;
    [SerializeField] private Sprite sandStoneSprite;
    [SerializeField] private Sprite leafSprite;
    [SerializeField] private Sprite caveSprite;
    [SerializeField] private GameObject blockPrefab; 
    [SerializeField] private GameObject worldBorder; 

    private static int gridSize; // World size in width (blocks)

    // Layers to determine how many rows of block do each type occupy
    [SerializeField] private int dirtLayer = 15;
    [SerializeField] private int clayLayer = 25;
    [SerializeField] private int sandLayer = 30;
    [SerializeField] private int sandStoneLayer = 40;
    [SerializeField] private int stoneLayer = 40;

    // Multiplier to affect cave frequency. Lower value results to smaller caves
    [SerializeField] private float caveFreq = 0.055f;

    // Multiplier to affect hill frequency. Lower value results to less hills
    [SerializeField] private float terrainFreq = 0.025f;

    // Multiplier to affect how drastic height chances are.
    [SerializeField] private float heightMultiplier = 35f;

    // Base height the terrain will at the very least have.
    [SerializeField] private int heightLevel = 150;

    // Used to determine wether a block is placed or not. Larger value leads to more caves
    [SerializeField] private float caveThreshold = 0.62f;

    // Used to add randomness to the terrain. Same seed will generate the same terrain everytime
    [SerializeField] private float seed;

    [SerializeField] private enum MaterialType
    {
        grass,
        dirt,
        clay,
        sand,
        sandStone,
        stone,
        magma,
        cabinWood,
        cabinBackround,
        roofTile,
        wood,
        leaf,
        cave
    }
    
    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        gridSize = (int)GameData.GetGridSize(); 
        seed = Random.Range(-10000, 10000);

        generateBlocks();
        generateWorldBorders();
    }

    /*
    * Generates the blocks in the 2D game world based on Perlin noise.
    * It creates blocks and assigns materials to them based on their
    * coordinates.
    */
    void generateBlocks() 
    {
        List<Vector2Int> surfaceBlocks = new List<Vector2Int>();

        Sprite blockSprite;
        MaterialType material;

        for (int x = 0; x < gridSize; x++) 
        {
            // Calculate the height of the terrain at this x-coordinate using Perlin noise
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) 
                * heightMultiplier + heightLevel;
            
            for (int y = 0; y < height; y++) 
            {
                // caveValue is used to determine wether a block is placed or not
                // using perlin noise. Empty spots form caves.
                float caveValue = Mathf.PerlinNoise((x + seed) * caveFreq,
                 (y + seed) * caveFreq);
                
                // Block is placed
                if (caveValue < caveThreshold) 
                {
                    // Grasslevel
                    if (y == (int)height) 
                    {
                        blockSprite = grassSprite;
                        material = MaterialType.grass;
                        surfaceBlocks.Add(new Vector2Int(x, y));
                    }
                    // Dirtlevel
                    else if (y < (int)height && y >= (int)height - dirtLayer) 
                    {
                        blockSprite = dirtSprite;
                        material = MaterialType.dirt;
                    }
                    // Claylevel
                    else if (y < (int)height - dirtLayer && 
                             y >= (int)height - dirtLayer - clayLayer) 
                    {
                        blockSprite = claySprite;
                        material = MaterialType.clay;
                    }
                    // Sandlevel
                    else if (y < (int)height - dirtLayer - clayLayer &&
                             y >= (int)height - dirtLayer - clayLayer - sandLayer) 
                    {   
                        blockSprite = sandSprite;
                        material = MaterialType.sand;
                    }
                    // Sandstonelevel
                    else if (y < (int)height - dirtLayer - clayLayer - sandLayer &&
                             y >= (int)height - dirtLayer - clayLayer - sandLayer - sandStoneLayer) 
                    {
                        blockSprite = sandStoneSprite;
                        material = MaterialType.sandStone;
                    }
                    // Stonelevel
                    else if (y < (int)height - dirtLayer - clayLayer - sandLayer - sandStoneLayer &&
                             y >= (int)height - dirtLayer - clayLayer - sandLayer - sandStoneLayer - stoneLayer) 
                    {
                        blockSprite = stoneSprite;
                        material = MaterialType.stone;
                    }
                    // Rest blocks at the bottom are magma
                    else 
                    {
                        blockSprite = magmaSprite;
                        material = MaterialType.magma;
                    }
                    generateBlock(blockSprite, x, y, material);
                }
                else 
                {
                    // Fill empty spots (caves) with backroundBlocks
                    generateBlock(caveSprite, x, y, MaterialType.cave, true);
                }
            }
        }
        generateSurfaceDecoration(surfaceBlocks);
    }
    
    /*
    * Generates block at given coordinates and depth (backround/ foreground)
    */
    GameObject generateBlock(Sprite sprite, int x, int y, MaterialType material,
                             bool isBackround = false) 
    {
        Vector2 position = new Vector2(x, y);
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);

        // Reference to Block class
        Block blockScript = block.GetComponent<Block>();
        blockScript.InitializeBlock(sprite, GetHpForMaterial(material),
         getMassForMaterial(material), isBackround);

        return block;
    }
    
    /*
    * Decorates surface with decorations (trees and/ or buildings) based on
    * the vertical sequence of level blocks (y-axis).
    */
    void generateSurfaceDecoration(List<Vector2Int> surfaceBlocks) 
    {
        int consecutiveBlocks = 0;
        Vector2Int lastBlock = new Vector2Int(-1, -1); // Initialize to an invalid value

        foreach (Vector2Int block in surfaceBlocks) 
        {
            if (block.y == lastBlock.y && block.x == lastBlock.x + 1) 
            {
                consecutiveBlocks++;

                // 20% chance to create a tree 
                if (consecutiveBlocks == 3) 
                {
                    if (Random.value < 0.2f) 
                    {
                        generateTree(block.x - 1, block.y + 1);
                        consecutiveBlocks = 0;
                        
                    }
                }
                // 30% chance to create a building
                if (consecutiveBlocks == 5) 
                {
                    if (Random.value < 0.3f) 
                    {
                        generateBuilding(block.x - 4, block.x, block.y + 1);
                    }
                    consecutiveBlocks = -1;
                }
            } 
            else 
            {
                consecutiveBlocks = 1;
            }
            lastBlock = block;
        }
    }

    void generateTree(int x, int y) 
    {
        int treeHeight = Random.Range(3, 8);

        // Create the trunk
        for (int i = 0; i < treeHeight; i++)
        {
            generateBlock(woodSprite, x, y + i, MaterialType.wood, true);
        }

        // Create the bush of leaves at the top 
        int radius = 2;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx * dx + dy * dy <= radius * radius)
                {
                    generateBlock(leafSprite, x + dx, y + treeHeight + dy,
                                 MaterialType.leaf, true);
                }
            }
        }
    }

    void generateBuilding(int leftBound, int rightBound, int y) 
    {
        int buildingHeight = Random.Range(5, 10); 

        for (int x = leftBound; x <= rightBound; x++) 
        {
            for (int i = 0; i < buildingHeight; i++) 
            {
                // Leave room for doors
                if ((x == leftBound && (i == 1 || i == 2)) || (x == rightBound &&
                     (i == 1 || i == 2)))
                {
                    generateBlock(cabinDoorSprite, x, y + i, MaterialType.cabinBackround, true);
                    continue; 
                }

                // Generate the outer walls of the building
                if (x == leftBound || x == rightBound || i == 0) 
                {
                    generateBlock(cabinSprite, x, y + i, MaterialType.cabinWood);
                }
                else
                {
                    // Generate background blocks for the inner part of the building
                    generateBlock(cabinBackroundSprite, x, y + i, MaterialType.cabinBackround, true);
                }
            }
        }

        // Generate triangle shaped roof with flat top
        int roofHeight = (rightBound - leftBound) / 2;
        int roofCenter = leftBound + (rightBound - leftBound) / 2;

        for (int i = 0; i <= roofHeight; i++)
        {
            for (int j = -i; j <= i; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                generateBlock(roofSprite, roofCenter + j, y + buildingHeight + roofHeight - i,
                             MaterialType.roofTile);
            }
        }
    }

    int GetHpForMaterial(MaterialType material)
    {
        switch (material)
        {
            // Terrain materials
            case MaterialType.grass:
                return 5;
            case MaterialType.dirt:
                return 10;
            case MaterialType.clay:
                return 15; 
            case MaterialType.sand:
                return 20;
            case MaterialType.sandStone:
                return 25;
            case MaterialType.stone:
                return 30;
            case MaterialType.magma:
                return 35; 

            // Decorations
            case MaterialType.cave:
                return 1;
            case MaterialType.cabinBackround:
                return 1;
            case MaterialType.wood:
                return 20;
            case MaterialType.leaf:
                return 1;
            case MaterialType.cabinWood:
                return 30;
            case MaterialType.roofTile:
                return 40;
            default:
                return 0;
        }
    }

    int getMassForMaterial(MaterialType material)
    {
        switch (material)
        {
            // Terrain materials
            case MaterialType.grass:
                return 100;
            case MaterialType.dirt:
                return 102;
            case MaterialType.clay:
                return 105;
            case MaterialType.sand:
                return 107;
            case MaterialType.sandStone:
                return 110;
            case MaterialType.stone:
                return 115;
            case MaterialType.magma:
                return 130;

            // Decorations
            case MaterialType.cave:
                return 50;
            case MaterialType.cabinBackround:
                return 50;
            case MaterialType.wood:
                return 102;
            case MaterialType.leaf:
                return 20;
            case MaterialType.cabinWood:
                return 105;
            case MaterialType.roofTile:
                return 109;
            default:
                return 100;
        }
    }

    /*
    * Surrounds the world with borders to contain the player and blocks.
    */
    void generateWorldBorders()
    {
        int width = gridSize;
        int height = gridSize;

        // bottom border
        for (int x = -1; x <= width; x++)
        {
            Instantiate(worldBorder, new Vector2(x, -1), Quaternion.identity); 
        }

        // left and right borders
        for (int y = 0; y < height * 2; y++)
        {
            Instantiate(worldBorder, new Vector2(-1, y), Quaternion.identity); 
            Instantiate(worldBorder, new Vector2(width, y), Quaternion.identity); 
        }
    }

    public int getGridSize()
    {
        return gridSize;
    }
}