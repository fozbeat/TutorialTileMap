using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInGamePanels : MonoBehaviour
{
    public GameObject tilesPanel;

    public void TogglePanel()
    {
        if(tilesPanel != null)
        {
            bool isActive = tilesPanel.activeSelf;
            tilesPanel.SetActive(!isActive);
        }
    }
}
