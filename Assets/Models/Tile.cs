using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

public class Tile
{
    public enum TileType { Empty, Floor };

    TileType type = TileType.Empty;

    Action<Tile> cbTileTypeChanged;
    public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            //Reduce the number of times that all tiles are looped across for a change
            //ie. reduce the number of call backs in the cbTileTypeChanged setter here.
            ////TileType oldType = type;
            ///
            if (type != value)
            {
                type = value;
                if (cbTileTypeChanged != null)
                    cbTileTypeChanged(this);
            }

            //call the callback and let it know that the TileType has changed
            //check if cbTileTypeChanged is null
            //if(cbTileTypeChanged != null && type != oldType)
            //{
            //    cbTileTypeChanged(this);    
            //}
           

        }
    }

    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
    int x;
    int y;

    //Getters x & y
    public int X
    {
        get
        {
            return x;
        }

    }

    public int Y
    {
        get
        {
            return y;
        }

    }

    //Constructor

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;

    }

    //Register and deregister callback - required for modularity plug and play. To avoid data layer
    //from having no dependency on the view.
    public void RegisterTileTypeChangedCallback(Action<Tile> callback)
    {  
        cbTileTypeChanged += callback;
    }

    public void UnregisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged -= callback;
    }

}
