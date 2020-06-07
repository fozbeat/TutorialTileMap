using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    
    public static WorldController Instance { get; protected set; }

    public Sprite floorSprite;

     

    
    public World World { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            Debug.Log("An instance of world is already set. This shouldn't happen! Instance should be null"); 
        Instance = this;
        //Create a world with empty tiles
        //Default arguments in constructore eg: 100 x 100
        World = new World(50, 50);

        //Create GameObjects ie. tiles to link them to visual port
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                GameObject tile_go = new GameObject();

                Tile tile_data = World.GetTileAt(x, y);
                

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                //Set as parent of worldcontroller and maintain position rotation and scale
                tile_go.transform.SetParent(this.transform, true);

                //Add a spriter renderer, but dont bother setting a sprite beacause all tiles empty right now 
                tile_go.AddComponent<SpriteRenderer>();

                //RegisterCallBack into cbTileTypeChanged in setter of Tile.Type
                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });

            }
        
        }

        World.InitializeWorldTiles(World.Width, World.Height);

        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        if(tile_data.Type == Tile.TileType.Floor)
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        
        else if(tile_data.Type == Tile.TileType.Empty)
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        
        else
            Debug.LogError("OnTileTypeChanged: Unrecognized tile type.");
        
    }

    public Tile GetTileAtWorldCoordinate(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        //A null is returned when the coordinates passed to GetTileAt is invalid or out of bounds
        return WorldController.Instance.World.GetTileAt(x, y);


    }

    //    public List<Tile> GetTilesUnderSelection(Vector3 selectionStart, Vector3 selectionEnd)
    //    {
    //        List<Tile> selectedTiles = new List<Tile>();

    //        int startX = Mathf.FloorToInt(selectionStart.x);
    //        int startY = Mathf.FloorToInt(selectionEnd.y);
    //        int endX = Mathf.FloorToInt(selectionStart.x);
    //        int endY = Mathf.FloorToInt(selectionEnd.y);

    //        if (endX < startX)
    //        {
    //            int temp = startX;
    //            startX = endX;
    //            endX = temp;
    //        }

    //        if (endY < startY)
    //        {
    //            int temp = startY;
    //            startY = endY;
    //            endY = temp;
    //        }

    //        for (int x = startX; x <= endX; x++)
    //        {
    //            for (int y = startY; y <= endY; y++)
    //            {
    //                selectedTiles.Add(GetTileAtWorldCoordinate(new Vector3(x, y, 0)));

    //            }
    //        }
    //        return selectedTiles;
    //    }

}
