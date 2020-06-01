using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float fastSpeedMultiplier = 2f;
    public float keyScrollSpeed = 0.15f;

    public int zoomSpeed = 10;
    public int zoomMax = 5;
    public int zoomMin = 70;

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;
    public GameObject selectionBox;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyboardScroll();
        CheckZoom();
        CheckMouseScroll();

    }

    void CheckKeyboardScroll()
    {
        float translationX = Input.GetAxis("Horizontal");
        float translationY = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            Camera.main.transform.Translate(translationX * fastSpeedMultiplier * keyScrollSpeed, translationY * fastSpeedMultiplier * keyScrollSpeed, 0);
        else
            Camera.main.transform.Translate(translationX * keyScrollSpeed, translationY * keyScrollSpeed, 0);
    }

    void CheckZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > zoomMax) // Zoom out
            Camera.main.orthographicSize -= zoomSpeed;

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < zoomMin) // Zoom in
            Camera.main.orthographicSize += zoomSpeed;
    }

    void CheckMouseScroll()
    {
        //Mouse panning and selection of correct tile from Mouse position

        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        Tile tileUnderMouse = GetTileAtWorldCoordinate(currFramePosition);

        if (tileUnderMouse != null)
        {
            selectionBox.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            
            selectionBox.transform.position = cursorPosition;
            
        }
        else
            selectionBox.SetActive(false);

        //Multiple selection

        //First left click. Start drag.
        if(Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        //End drag
        if(Input.GetMouseButtonUp(0))
        {
            int start_x = Mathf.FloorToInt(dragStartPosition.x);
            int end_x = Mathf.FloorToInt(currFramePosition.x);

            int start_y = Mathf.FloorToInt(dragStartPosition.y);
            int end_y = Mathf.FloorToInt(currFramePosition.y);


            //swap if dragging from right to left ie start_x > end_x
            if (end_x < start_x)
            {
                int temp = start_x;
                start_x = end_x;
                end_x = temp;
            }

            if (end_y < start_y)
            {
                int temp = start_y;
                start_y = end_y;
                end_y = temp;
            }

            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                        t.Type = Tile.TileType.Floor;
                }
            }
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    Tile GetTileAtWorldCoordinate(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        //A null is returned when the coordinates passed to GetTileAt is invalid or out of bounds
        return WorldController.Instance.World.GetTileAt(x, y);


    }
}