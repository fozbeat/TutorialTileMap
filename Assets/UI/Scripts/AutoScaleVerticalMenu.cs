using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaleVerticalMenu : MonoBehaviour
{
    public float childHeight = 30f;
    // Start is called before the first frame update
    void Start()
    {
        ScaleSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleSize()
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        size.y = this.transform.childCount * childHeight;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
