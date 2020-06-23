using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalControlerScript : MonoBehaviour
{
    public TextMeshProUGUI GO_Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll.gameObject.name);
        if(coll.gameObject.name == "Player")
        {
            Time.timeScale = 0f;
            Debug.Log("Goal Reached");
            GO_Text.text = "Game Over\nPlayer Won";
        }
    }
}
