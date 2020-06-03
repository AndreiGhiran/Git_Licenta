using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class KeyValueSortingTest : MonoBehaviour
{

     
    // Start is called before the first frame update
    void Start()
    {
        var list = new List<KeyValuePair<Vector3, int>>();
        //int x = list[0].Value;
		list.Sort((x ,y) => x.Value.CompareTo(y.Value));
		Debug.Log(list[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
