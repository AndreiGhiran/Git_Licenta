using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using TMPro;

public class Navigator : MonoBehaviour
{
    
    private Rigidbody2D rb;
    float movementSpeed = 3f;
    private Vector2 endPosition;
    public GameObject navGoal;
    public Tilemap tilemap;
    public GameObject path_prefab;
    Vector3 lastGoalPosition;
    List<Vector3> path;
    int move_number;
    List<int> moves = new List<int>();
    List<GameObject> path_objs = new List<GameObject>();
    bool display = false;
    public GameObject playMode;
    public TextMeshProUGUI GO_Text;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endPosition = rb.position;
        lastGoalPosition = navGoal.transform.position;
        move_number = 0;

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
        if (!onGoal(lastGoalPosition))
        {
            lastGoalPosition = navGoal.transform.position;
            move_number = 0;
            A_star_path_finder(endPosition);
            CalculateMoves();
            if (!playMode.activeSelf)
            {
                make_path();
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && !playMode.activeSelf)
        {
            toggle_display_Path();
        }
    }
    
    public void Move()
    {
       

        if (!onGoal(rb.position))
        {

            int choice = moves[move_number];
            move_number++;
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
			int choice = rand.Next(0, 3);

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
			after_random_move(choice);
		}
	}

    void after_random_move(int choice)
	{
        if (onGoal(rb.position)) {
            Debug.Log("in after_random_move if");

            move_number = 0;
            moves = new List<int>();

            switch (choice)
            {
                case 0:
                    moves.Add(1);
                    break;
                case 1:
                    moves.Add(0);
                    break;
                case 2:
                    moves.Add(3);
                    break;
                default:
                    moves.Add(2);
                    break;

            }
        }
    }


    void OnCollisionStay2D(Collision2D coll)
    {
        //Debug.Log("Collided");
         if(coll.gameObject.name == "Player")
        {
            Time.timeScale = 0f;
            GO_Text.text = "Game Over\nAI Wins the Game";
        }
        endPosition.y = (float)Math.Round(rb.position.y, 1);
        endPosition.x = (float)Math.Round(rb.position.x, 1);
       
    }

    void CalculateMoves()
	{
        Vector3 start = new Vector3(), end = new Vector3();
        moves = new List<int>();
        for(int i = 0; i < path.Count - 1; i++)
		{
            start = path[i];
            end = path[i + 1];

            if(start.x > end.x)
                moves.Add(0);

            if (start.x < end.x)
                moves.Add(1);

            if (start.y > end.y)
                moves.Add(2);

            if (start.y < end.y)
                moves.Add(3);
		}
	}

    bool isWall(Vector3 place)
	{
        Vector3Int coords = new Vector3Int((int)Math.Floor(place.x), (int)Math.Floor(place.y), 0);

        TileBase tile = tilemap.GetTile(coords);
        if (tile != null)
        {
            //Debug.Log("Not null");
            if (tile.name == "WallPiece") { 
                //Debug.Log(tile.name);
                return true;
            }
            return false;
        }
        return false;
    }

    bool onGoal(Vector3 coords)
	{
        if (coords.x == navGoal.transform.position.x && coords.y == navGoal.transform.position.y)
        {
            return true;
        }
        else
            return false;
	}

    void A_star_path_finder(Vector3 start)
	{
        Debug.Log("A_Star_Start");
        HashSet<Vector3> visited = new HashSet<Vector3>();
        List<KeyValuePair<Vector3, float>> A_star_stack = new List<KeyValuePair<Vector3, float>>();
        Dictionary<Vector3, Vector3> from = new Dictionary<Vector3, Vector3>();
        Dictionary<Vector3, int> cost = new Dictionary<Vector3, int>();
        List<Vector3> directions;
        List<Vector3> proto_path = new List<Vector3>();
        path = new List<Vector3>();

        A_star_stack.Add(new KeyValuePair<Vector3, float>(start, getDistance(start, navGoal.transform.position)));
        from.Add(start, start);
        cost.Add(start, 0);
        while (true){
            Vector3 location = A_star_stack[0].Key;

            visited.Add(location);

            proto_path.Add(location);

            A_star_stack.RemoveAt(0);

			if (onGoal(location))
			{
                break;
			}

            directions = new List<Vector3>();

            Vector3 left = new Vector3(location.x - 1f, location.y, location.z);
            Vector3 right = new Vector3(location.x + 1f, location.y, location.z);
            Vector3 up = new Vector3(location.x, location.y + 1f, location.z);
            Vector3 down = new Vector3(location.x, location.y - 1f, location.z);


            if (location.x > rb.position.x && location.y == rb.position.y)
            {
                directions.Add(right);
                directions.Add(up);
                directions.Add(down);
                directions.Add(left);
            }
            else
            if (location.x > rb.position.x && location.y > rb.position.y)
            {
                directions.Add(up);
                directions.Add(right);
                directions.Add(left);
                directions.Add(down);
                
            }
            else
            if (location.x > rb.position.x && location.y < rb.position.y)
            {
                directions.Add(down);
                directions.Add(right);
                directions.Add(left);
                directions.Add(up);
                
            }
            else
            if (location.x < rb.position.x && location.y == rb.position.y)
            {
                directions.Add(left);
                directions.Add(up);
                directions.Add(down);
                directions.Add(right);
            }
            else
            if (location.x < rb.position.x && location.y > rb.position.y)
            {
                directions.Add(left);
                directions.Add(up);
                directions.Add(down);
                directions.Add(right);
            }
            else
            if (location.x < rb.position.x && location.y < rb.position.y)
            {
                directions.Add(left);
                directions.Add(down);
                directions.Add(up);
                directions.Add(right);
            }
            else
            if (location.y > rb.position.y && location.x == rb.position.x)
            {
                directions.Add(up);
                directions.Add(right);
                directions.Add(left);
                directions.Add(down);
            }
            else
            if (location.y < rb.position.y && location.x == rb.position.x)
            {
                directions.Add(down);
                directions.Add(right);
                directions.Add(left);
                directions.Add(up);
            }
            else
            {
                directions.Add(down);
                directions.Add(right);
                directions.Add(left);
                directions.Add(up);
            }                


            for (int i = 0; i < 4; i++)
            {
                if (!isWall(directions[i]) && !visited.Contains(directions[i]))
                {
                    if (cost.ContainsKey(directions[i]))
                    {
                        cost[directions[i]] += cost[location];
                    }
                    else
                        cost.Add(directions[i], 1);

                    if (from.ContainsKey(directions[i]))
                    {
                        from[directions[i]] = location;
                    }
                    else
                        from.Add(directions[i], location);
                    bool exists = false;
                    for (int j = 0; j < A_star_stack.Count; j++)
                    {
                        if (A_star_stack[j].Key == directions[i])
                        { 
                            var newEntry = new KeyValuePair<Vector3, float>(A_star_stack[j].Key, cost[directions[i]] + getDistance(directions[i], navGoal.transform.position));
                            A_star_stack[j] = newEntry;
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        A_star_stack.Add(new KeyValuePair<Vector3, float>(directions[i], getDistance(directions[i], navGoal.transform.position) + cost[directions[i]]));
                    }
                }
            }
            A_star_stack.Sort((x, y) => x.Value.CompareTo(y.Value));
        }

        Vector3 step = proto_path[proto_path.Count - 1];
        path.Add(step);
        while(step != start)
		{
            step = from[step];
            path.Add(step);
		}
        path.Reverse();
    }

    float getDistance(Vector3 start, Vector3 end)
	{
        float dist = (float)Math.Sqrt((Math.Pow((start.x - 1f) - end.x, 2)) + (Math.Pow(start.y - end.y, 2)));
        return dist;
    }

    void make_path()
	{
        if (path_objs.Count != 0)
        {
            for (int i = 0; i < path_objs.Count; i++)
            {
                Destroy(path_objs[i]);
            }
            path_objs.Clear();
        }
        for (int i = 0; i < path.Count; i++)
        {
            GameObject point = Instantiate(path_prefab, path[i], Quaternion.identity);
            point.SetActive(display);
            path_objs.Add(point);
        }
    }

    void toggle_display_Path()
	{
        display = !display;
        Debug.Log(path_objs.Count);
        for (int i = 0; i < path_objs.Count; i++)
		{
			path_objs[i].SetActive(display);
		}
        navGoal.GetComponent<SpriteRenderer>().enabled = display;


    }
}
