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

    GameObject selectedTile;
    public GameObject selectionTileAura;
    public RectTransform selectionBox;

    List<GameObject> allSelectedTiles = new List<GameObject>();
    bool firstClickFlag = false;



    // Start is called before the first frame update
    void Start()
    {
       selectionTileAura = Instantiate(selectionTileAura);
       selectionTileAura.name = "SelectionHoverAura";
       selectionTileAura.transform.SetParent(this.transform, true); 
    }

    // Update is called once per frame
    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;
        
        CheckMouseScroll();
        CheckKeyboardScroll();
        CheckZoom();
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
        //Selection aura over mouse pointer
        HoverSelectionAura(selectionTileAura);
        
        if (Input.GetMouseButtonDown(0))
        {
            //Position where user started drag
            dragStartPosition = currFramePosition;
        }
        int start_x = Mathf.FloorToInt(dragStartPosition.x);
        int end_x = Mathf.FloorToInt(currFramePosition.x);

        int start_y = Mathf.FloorToInt(dragStartPosition.y);
        int end_y = Mathf.FloorToInt(currFramePosition.y);

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

        while (allSelectedTiles.Count > 0)
        {
            GameObject go = allSelectedTiles[0];
            allSelectedTiles.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        if (Input.GetMouseButton(0))
        {
            CursorDragSelectionBox(selectionBox, dragStartPosition, currFramePosition);
            
            //swap if dragging from right to left ie start_x > end_x
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if(t != null)
                    {
                        GameObject selectedTile = SimplePool.Spawn(selectionTileAura, new Vector3(x, y, 0), Quaternion.identity);
                        selectedTile.name = "TilesUnderSelection_" + x + "_" + y;
                        selectedTile.transform.parent = this.transform;
                        allSelectedTiles.Add(selectedTile);
                    }
                }
            }
        }
        //End drag
        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);

            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        t.Type = Tile.TileType.Empty;
                    }
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

    public void HoverSelectionAura(GameObject selectionTileAura)
    {
        Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoordinate(currFramePosition);

        if (tileUnderMouse != null)
        {
            selectionTileAura.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            selectionTileAura.transform.position = cursorPosition;
        }
        else
            selectionTileAura.SetActive(false);
    }

    public void CursorDragSelectionBox(RectTransform selectionBox, Vector3 dragStartPosition, Vector3 currFramePosition)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        Vector3 diff = currFramePosition - dragStartPosition;

        if (diff.x < 0 && diff.y < 0)
        {
            diff.x = -diff.x;
            diff.y = -diff.y;

            selectionBox.anchoredPosition = new Vector2((currFramePosition.x), (currFramePosition.y));
        }
        else if (diff.x < 0 && diff.y > 0)
        {
            diff.x = -diff.x;
            selectionBox.anchoredPosition = new Vector2(currFramePosition.x, dragStartPosition.y);

        }
        else if (diff.x > 0 && diff.y < 0)
        {
            diff.y = -diff.y;
            selectionBox.anchoredPosition = new Vector2(dragStartPosition.x, currFramePosition.y);
        }
        else
        {
            selectionBox.anchoredPosition = new Vector2((dragStartPosition.x), (dragStartPosition.y));

        }
        selectionBox.sizeDelta = diff;

    }

    




}