using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseImageChange : MonoBehaviour
{
    private Vector3 mousePos;
    public GameObject mouseImage;
    public float xOffset;
    public float yOffset;

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
