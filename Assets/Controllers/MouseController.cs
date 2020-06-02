﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseController : MonoBehaviour
{
    public float fastSpeedMultiplier = 2f;
    public float keyScrollSpeed = 0.15f;

    public int zoomSpeed = 10;
    public int zoomMax = 5;
    public int zoomMin = 70;

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;
    Vector3 currFramePosition;
    public GameObject selectionTileAura;
    public RectTransform selectionBox;

    // Start is called before the first frame update
    void Start()
    {
       selectionTileAura = Instantiate(selectionTileAura);
       selectionTileAura.name = "SelectionTileAura";
       selectionTileAura.transform.parent = this.transform; 
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

        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoordinate(currFramePosition);

        if(tileUnderMouse != null) { 
            selectionTileAura.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            selectionTileAura.transform.position = cursorPosition;
        }
        else
            selectionTileAura.SetActive(false);

        //First left click. Start drag.
        if(Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        //Left mouse button held down
        if (Input.GetMouseButton(0))
        {
            //UpdateSelectionBox(dragStartPosition);
          
            if(!selectionBox.gameObject.activeInHierarchy)
                selectionBox.gameObject.SetActive(true);
                Debug.Log(currFramePosition + "Curr Cursor");
                Debug.Log(dragStartPosition + "Start Position");
                Vector3 diff = currFramePosition - dragStartPosition;
                Debug.Log(diff + "difference");
            //selectionBox.sizeDelta = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, diff.x);
                selectionBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, diff.y);
                selectionBox.anchoredPosition = new Vector2((dragStartPosition).x, (dragStartPosition).y);        

        }


        //End drag
        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);

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

    void UpdateSelectionBox(Vector3 dragStartPosition)
    {
        //Vector3 currCursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //currCursorPosition.z = 0f;
        
    }
    

    
}