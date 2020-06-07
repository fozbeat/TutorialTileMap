using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class World 
{
    Tile[,] tiles;
    int width;

    public int Width 
    {
        get
        {   
            return width;
        }
    }

    int height;

    public int Height
    {
        get
        {
            return height;
        }
    }

    //Constructor
    public World(int width = 100, int height = 100)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Check constructor of Tile
                tiles[x, y] = new Tile(this, x, y);
            }

        }

        Debug.Log("World created with (" +width+ "," +height+ ") dimensions.");

    }

    public Tile GetTileAt(int x, int y)
    {
        if (x >= width || x < 0 || y >= height || y < 0)
        {
            return null;
        } else
            return tiles[x, y];
    }

    public void InitializeWorldTiles(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y].Type = Tile.TileType.Floor; 
                
            }
        }
    }


}
