﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile
{
    public float[] CornerHeights;

    // Construct a tile from a set of 4 points
    public TerrainTile(float corner_1, float corner_2, float corner_3, float corner_4)
    {
        CornerHeights = new float[4];
        this.CornerHeights[0] = corner_1;
        this.CornerHeights[1] = corner_2;
        this.CornerHeights[2] = corner_3;
        this.CornerHeights[3] = corner_4;
    }

    // Are all corners of the tile the same height
    public bool AreAllCornersEqual()
    {
        bool matching = true;
        for (int i = 0; i < CornerHeights.Length - 1; i++)
        {
            matching = matching && (this.CornerHeights[i] == this.CornerHeights[i + 1]);
        }

        return matching;
    }


    public float GetHeightAtRelativePoint(int tile_resolution, int x_pos_in_tile, int y_pos_in_tile)
    {
        // Basic Idea:
        // Imagine a square tile with 2 strings that connect opposite corners
        // Now imagine tying the two pieces of string together in the middle where they meet.
        // If you raise 1 corner of the square, the piece of string connected to the raised corner
        // will want to angle up.
        // However, the other string will want to remain level.
        // The result of this motion will be for each string to compromise and meet half way.
        // This will result in the square being broken up in to 4 triangles, each on a differently angled plane.


        // Begin by finding the height at the middle

        // North East to South West string
        float mid_height_string_1 = (this.CornerHeights[0] + this.CornerHeights[2]) / 2.0f;

        // North West to South East string
        float mid_height_string_2 = (this.CornerHeights[1] + this.CornerHeights[3]) / 2.0f;

        // Three ways to fold
        // 1) Fold Over
        //float height_of_centre = Mathf.Min(mid_height_string_1, mid_height_string_2);

        // 2) Fold Under
        float height_of_centre = Mathf.Max(mid_height_string_1, mid_height_string_2);

        // 3) Fold 4-ways to compromise
        //float height_of_centre = (mid_height_string_1 + mid_height_string_2) / 2.0f;

        // Find which quarter triangle the point belongs to and find the vertices that make up that triangle
        Vector3 position_of_nearest_corner_1;
        Vector3 position_of_nearest_corner_2;
        Vector3 position_of_centre = new Vector3(tile_resolution / 2.0f, tile_resolution / 2.0f, height_of_centre);

        // Point belongs in the top left half of square
        if (y_pos_in_tile > x_pos_in_tile)
        {
            // Point belongs in the South triangle
            if (y_pos_in_tile > (tile_resolution - x_pos_in_tile))
            {
                position_of_nearest_corner_1 = new Vector3(tile_resolution, tile_resolution, this.CornerHeights[1]);
                position_of_nearest_corner_2 = new Vector3(0, tile_resolution, this.CornerHeights[2]);

            }
            // Point belongs in the West triangle
            else
            {
                position_of_nearest_corner_1 = new Vector3(0, tile_resolution, this.CornerHeights[2]);
                position_of_nearest_corner_2 = new Vector3(0, 0, this.CornerHeights[3]);
            }

        }
        // Point belongs in the bottom right half of square
        else
        {
            // Point belongs in the East triangle
            if (y_pos_in_tile > (tile_resolution - x_pos_in_tile))
            {
                position_of_nearest_corner_1 = new Vector3(tile_resolution, 0, this.CornerHeights[0]);
                position_of_nearest_corner_2 = new Vector3(tile_resolution, tile_resolution, this.CornerHeights[1]);
            }
            // Point belongs in the North triangle
            else
            {
                position_of_nearest_corner_1 = new Vector3(0, 0, this.CornerHeights[3]);
                position_of_nearest_corner_2 = new Vector3(tile_resolution, 0, this.CornerHeights[0]);
            }
        }

        // Find the equation of the plane defined by the 3 points
        Vector3 centre_to_point_1 = position_of_nearest_corner_1 - position_of_centre;
        Vector3 centre_to_point_2 = position_of_nearest_corner_2 - position_of_centre;

        Vector3 orthogonal_vector = Vector3.Cross(centre_to_point_2, centre_to_point_1);

        float height_offset = position_of_nearest_corner_1.x * orthogonal_vector.x
            + position_of_nearest_corner_1.y * orthogonal_vector.y
            + position_of_nearest_corner_1.z * orthogonal_vector.z;

        // Substitute the point in to the equation to find its height
        float height = (height_offset - orthogonal_vector.x * x_pos_in_tile - orthogonal_vector.y * y_pos_in_tile) / orthogonal_vector.z;

        return height;
    }
};