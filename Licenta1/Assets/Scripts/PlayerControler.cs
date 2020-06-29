using UnityEngine;
using System;
public class PlayerControler : MonoBehaviour
{
    public GameObject adversar;
    private Rigidbody2D rb;
    private Vector2 endPosition;
    float movementSpeed = 3f;
    bool move = true;
    Navigator adv_move;

    // Start is called before the first frame update
    void Start()
    {
        adv_move = (Navigator)adversar.GetComponent("Navigator");
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
        else
        {
            move = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                endPosition.x -= 1;
                adv_move.Move();
                move = false;

            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                
                endPosition.x += 1;
                adv_move.Move();
                move = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                endPosition.y += 1;
                adv_move.Move();
                move = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                endPosition.y -= 1;
                adv_move.Move();
                move = false;
            }
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        //Debug.Log("Collided");
        endPosition.y = (float)Math.Round(rb.position.y, 1);
        endPosition.x = (float)Math.Round(rb.position.x, 1);
    }
}
