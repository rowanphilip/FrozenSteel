using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Texture2D MapFile;

    // How big is each tile in Unity's coordinate space
    public float TerrainTileLength;
    public int TerrainTileResolution;

    public float TerrainHeight;

    private Terrain TerrainComponent;
    private TerrainTile[,] TerrainHeightMap;

    public Texture2D FlatTexture;

    // Start is called before the first frame update
    void Start()
    {
        TerrainComponent = GetComponent<Terrain>();

        TerrainComponent.Flush();

        TerrainComponent.transform.position = new Vector3(-164, -1, 25);

        ReadHeightMapData();
        GenerateTerrain();

        Debug.Log("Width: " + MapFile.width);
        Debug.Log("Length: " + MapFile.height);
    }

    // Read pixel values from bmp and group them in to tile corner data
    void ReadHeightMapData()
    {
        TerrainHeightMap = new TerrainTile[MapFile.width / 2, MapFile.height / 2];

        for (int i = 0; i < MapFile.width / 2; i++)
        {
            for (int j = 0; j < MapFile.height / 2; j++)
            {
                TerrainHeightMap[i, j] = new TerrainTile(              // Corners numbered clockwise starting from North
                    MapFile.GetPixel(2 * i + 1, 2 * j).grayscale,      // NE
                    MapFile.GetPixel(2 * i + 1, 2 * j + 1).grayscale,      // SE
                    MapFile.GetPixel(2 * i, 2 * j + 1).grayscale,  // SW
                    MapFile.GetPixel(2 * i, 2 * j).grayscale);     // NW
            }
        }
    }

    void GenerateTerrain()
    {
        TerrainData terrain_data = new TerrainData();

        int terrain_width = MapFile.width / 2;
        int terrain_depth = MapFile.height / 2;

        terrain_data.size = new Vector3(terrain_width * TerrainTileLength, TerrainHeight, terrain_depth * TerrainTileLength);
        terrain_data.heightmapResolution = Mathf.Max(terrain_width * TerrainTileResolution, terrain_depth * TerrainTileResolution);

        float[,] height_map = new float[terrain_width * TerrainTileResolution, terrain_depth * TerrainTileResolution];

        // Work through each point on the terrain (as determined by the resolution not the grid size)
        for (int i = 0; i < terrain_width * TerrainTileResolution; i++)
        {
            for (int j = 0; j < terrain_depth * TerrainTileResolution; j++)
            {
                // Work backwards from the terrain point to the grid position
                TerrainTile height_map_data = TerrainHeightMap[i / TerrainTileResolution, j / TerrainTileResolution];

                // Determine whether this tile is flat or angled
                if (height_map_data.AreAllCornersEqual())
                {
                    height_map[i, j] = height_map_data.CornerHeights[0];
                }
                else
                {
                    // If angled, interpolate height
                    height_map[i, j] = height_map_data.GetHeightAtRelativePoint(
                        TerrainTileResolution, 
                        i % TerrainTileResolution,    // x position relative to current tile
                        j % TerrainTileResolution);   // y position relative to current tile
                }
            }
        }

        terrain_data.SetHeights(0, 0, height_map);

        TerrainComponent.terrainData = terrain_data;
    }

    public TerrainTile[,] getTerrain()
    {
        return TerrainHeightMap;
    }

}
