using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.ComponentModel;



public class Navigator : MonoBehaviour
{
    private Rigidbody2D rb;
    float movementSpeed = 3f;
    private Vector2 endPosition;
    public GameObject navGoal;

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

        if (!onGoal())
        {
            int choice = CalculateMove();

            switch (choice)
            {
                case 0:
                    endPosition.x -= 1;
                    break;
                case 1:
                    endPosition.x += 1;
                    break;
                case 2:
                    endPosition.y -= 1;
                    break;
                default:
                    endPosition.y += 1;
                    break;
            }
        }
		else
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
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        Debug.Log("Collided");
        endPosition.y = (float)Math.Round(rb.position.y, 1);
        endPosition.x = (float)Math.Round(rb.position.x, 1);
    }

    int CalculateMove()
	{
        
        List<float> distances = new List<float>();
        float dist;
        float min_dist;

        dist = (float)Math.Sqrt((Math.Pow((transform.position.x - 1f) - navGoal.transform.position.x, 2)) + (Math.Pow(transform.position.y - navGoal.transform.position.y, 2)));
        distances.Add(dist);

        dist = (float)Math.Sqrt((Math.Pow((transform.position.x + 1f) - navGoal.transform.position.x, 2)) + (Math.Pow(transform.position.y - navGoal.transform.position.y, 2)));
        distances.Add(dist);

        dist = (float)Math.Sqrt((Math.Pow(transform.position.x - navGoal.transform.position.x, 2)) + (Math.Pow((transform.position.y - 1f) - navGoal.transform.position.y, 2)));
        distances.Add(dist);

        dist = (float)Math.Sqrt((Math.Pow(transform.position.x - navGoal.transform.position.x, 2)) + (Math.Pow((transform.position.y + 1f) - navGoal.transform.position.y, 2)));
        distances.Add(dist);

        min_dist = distances.Min();

        for (int i = 0; i <= 3; i++)
		{
			if (distances[i] == min_dist)
			{
                //Debug.Log(i);
				return i;
			}
		}
		return 4;
	}

    Boolean onGoal()
	{
        if (rb.position.x == navGoal.transform.position.x && rb.position.y == navGoal.transform.position.y)
        {
            return true;
        }
        else
            return false;
	}
}
