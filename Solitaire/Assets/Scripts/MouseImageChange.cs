using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseImageChange : MonoBehaviour
{
    private Vector3 mousePos;
    public Image mouseImage;
    public float xOffset;
    public float yOffset;


    private void Awake()
    {
        //Cursor.visible = false;
    }
    void Update()
    {
        SetMousePosition();
        mouseImage.transform.position = mousePos;
    }

    void SetMousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x += xOffset;
        mousePos.y -= yOffset;
        mousePos.z = -9;
    }
}
