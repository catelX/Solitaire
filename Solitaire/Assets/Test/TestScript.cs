using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    public event Action<int> hello;

    private void Start()
    {
        hello += Hello;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            hello(3);
        }
    }

    public void Hello(int num)
    {
        Debug.Log("Hello " + num);
    }
}
