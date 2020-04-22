using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MocAI : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    float movementSpeed = 3f;
    private Vector2 endPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (rb.position != endPosition)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, endPosition, movementSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        System.Random rand = new System.Random();
        int choice = rand.Next(1, 4);
       
        switch (choice)
        {
            case 1:
                endPosition.x -= 1;
                break;
            case 2:
                endPosition.x += 1;
                break;
            case 3:
                endPosition.y -= 1;
                break;
            default:
                endPosition.y += 1;
                break;
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        Debug.Log("Collided");
        endPosition.y = (float)Math.Round(rb.position.y, 1);
        endPosition.x = (float)Math.Round(rb.position.x, 1);
    }
}
