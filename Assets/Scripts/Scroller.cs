using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField]
    private RawImage _img;

    [SerializeField]
    private float CoordX, CoordY;

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(CoordX, CoordY) * Time.deltaTime,_img.uvRect.size);
        
    }
}
