using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickEventTest : MonoBehaviour
{
    public GameObject prefab;
    public GameObject wallprefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(Input.mousePosition);
            
            Instantiate(prefab, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y), 0f), transform.rotation);
            
            //Matrix4x4 m = Camera.main.cameraToWorldMatrix;
            //Vector3 p = m.MultiplyPoint(new Vector3(0, 0, distance-2));
            //Instantiate(prefab, p, Quaternion.identity);

        }
        if (Input.GetMouseButton(0))
        {
            float mouse_x = Input.mousePosition.x;
            float mouse_y = Input.mousePosition.y;
            //Debug.Log(mouse_x);
            //Debug.Log(mouse_y);
            
            Vector3 ceva = Camera.main.ScreenToWorldPoint(new Vector3(mouse_x, mouse_y, 1f));
            //Debug.Log(ceva.x);
            //Debug.Log(ceva.y);
            if (ceva.x <= Math.Round(ceva.x))
            {
                ceva.x = (float)Math.Round(ceva.x) - 0.5f;
            }
            if (ceva.x > Math.Round(ceva.x))
            {
                ceva.x = (float)Math.Round(ceva.x) + 0.5f;
            }
            if (ceva.y <= Math.Round(ceva.y))
            {
                ceva.y = (float)Math.Round(ceva.y) - 0.5f;
            }
            if (ceva.y > Math.Round(ceva.y))
            {
                ceva.y = (float)Math.Round(ceva.y) + 0.5f;
            }
            //Debug.Log(ceva.x);
            //Debug.Log(ceva.y);
            Instantiate(wallprefab, ceva, Quaternion.identity);
        }
    }
}
