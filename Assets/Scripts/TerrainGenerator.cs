using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // The 2d bitmap texture defining the map
    public Texture2D MapFile;

    // The prefab to build the terrain with
    public GameObject TerrainBlock;

    public Vector3 TerrainOffset;


    private int[,] HeightMap;


    // Start is called before the first frame update
    void Start()
    {
        ReadHeightMap();
        GenerateTerrain();
    }

    // Convert image to a discrete valued height map
    void ReadHeightMap()
    {
        HeightMap = new int[MapFile.width, MapFile.height];

        for (int x = 0; x < MapFile.width; x++)
        {
            for (int y = 0; y < MapFile.height; y++)
            {
                HeightMap[x, y] = (int)Mathf.Round(255 * MapFile.GetPixel(x, y).grayscale);
            }
        }
    }


    void GenerateTerrain()
    {
        for (int x = 0; x < MapFile.width; x++)
        {
            for (int y = 0; y < MapFile.height; y++)
            {
                // For each x, y position, generate a stack of blocks
                // The stack should start at the height of the lowest directly adjacent tile
                // It should stop at the height value specified by the map file

                // First determine the height of the lowest neighbour
                int lowest_height = HeightMap[x, y];

                if (x > 0)
                {
                    lowest_height = Mathf.Min(lowest_height, HeightMap[x - 1, y]);
                }

                if (x < MapFile.width - 1)
                {
                    lowest_height = Mathf.Min(lowest_height, HeightMap[x + 1, y]);
                }

                if (y > 0)
                {
                    lowest_height = Mathf.Min(lowest_height, HeightMap[x, y - 1]);
                }

                if (y < MapFile.height - 1)
                {
                    lowest_height = Mathf.Min(lowest_height, HeightMap[x, y + 1]);
                }

                // Now stack the blocks
                for (int z = lowest_height; z <= HeightMap[x, y]; z++)
                {
                    Collider terrain_block_collider = TerrainBlock.GetComponent<Collider>();

                    Vector3 position = new Vector3(
                        x * terrain_block_collider.bounds.size.x,
                        z * terrain_block_collider.bounds.size.y,
                        y * terrain_block_collider.bounds.size.z);

                    Instantiate(TerrainBlock, position + TerrainOffset, Quaternion.identity);
                }
            }
        }
    }
}


//    // Read pixel values from bmp and group them in to tile corner data
//    void ReadHeightMapData()
//    {
//        TerrainHeightMap = new TerrainTile[MapFile.width / 2, MapFile.height / 2];

//        for (int i = 0; i < MapFile.width / 2; i++)
//        {
//            for (int j = 0; j < MapFile.height / 2; j++)
//            {
//                TerrainHeightMap[i, j] = new TerrainTile(              // Corners numbered clockwise starting from North
//                    MapFile.GetPixel(2 * i + 1, 2 * j).grayscale,      // NE
//                    MapFile.GetPixel(2 * i + 1, 2 * j + 1).grayscale,      // SE
//                    MapFile.GetPixel(2 * i, 2 * j + 1).grayscale,  // SW
//                    MapFile.GetPixel(2 * i, 2 * j).grayscale);     // NW
//            }
//        }
//    }

//    void GenerateTerrain()
//    {
//        TerrainData terrain_data = new TerrainData();

//        int terrain_width = MapFile.width / 2;
//        int terrain_depth = MapFile.height / 2;

//        terrain_data.size = new Vector3(terrain_width * TerrainTileLength, TerrainHeight, terrain_depth * TerrainTileLength);
//        terrain_data.heightmapResolution = Mathf.Max(terrain_width * TerrainTileResolution, terrain_depth * TerrainTileResolution);

//        float[,] height_map = new float[terrain_width * TerrainTileResolution, terrain_depth * TerrainTileResolution];

//        // Work through each point on the terrain (as determined by the resolution not the grid size)
//        for (int i = 0; i < terrain_width * TerrainTileResolution; i++)
//        {
//            for (int j = 0; j < terrain_depth * TerrainTileResolution; j++)
//            {
//                // Work backwards from the terrain point to the grid position
//                TerrainTile height_map_data = TerrainHeightMap[i / TerrainTileResolution, j / TerrainTileResolution];

//                // Determine whether this tile is flat or angled
//                if (height_map_data.AreAllCornersEqual())
//                {
//                    height_map[i, j] = height_map_data.CornerHeights[0];
//                }
//                else
//                {
//                    // If angled, interpolate height
//                    height_map[i, j] = height_map_data.GetHeightAtRelativePoint(
//                        TerrainTileResolution, 
//                        i % TerrainTileResolution,    // x position relative to current tile
//                        j % TerrainTileResolution);   // y position relative to current tile
//                }
//            }
//        }

//        terrain_data.SetHeights(0, 0, height_map);

//        TerrainComponent.terrainData = terrain_data;
//    }

//    public TerrainTile[,] getTerrain()
//    {
//        return TerrainHeightMap;
//    }

//}
