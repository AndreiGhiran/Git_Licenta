using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.Tilemaps;

public class PlayerDetector : MonoBehaviour
{
    List<Vector2> old_points;
    List<Vector2> positions;
    List<List<GameObject>> clusters;
    List<GameObject> cluster_centers;
    List<GameObject> points_go;
    List<Color> colors;
    public int cl_number;
    public GameObject clusterCenter;
    public GameObject point_prefab;
    public GameObject navGoal;
    int moved;
    int iterations = 0;
    bool clustering = true;
    public GameObject playMode;
    public GameObject vision_field;
    public TextMeshProUGUI Dev_Text;
    public Tilemap tilemap;


    // Start is called before the first frame update
    void Start()
    {
        points_go = new List<GameObject>();
        positions = new List<Vector2>();
        old_points = new List<Vector2>();
        cluster_centers = new List<GameObject>();
        clusters = new List<List<GameObject>>();
        Load();
        make_Colors();
        initialize_Cluster_Centers_and_Points();
        moved = cl_number;
        if (playMode.activeSelf && old_points.Count!=0)
        {
            K_means();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !playMode.activeSelf)
        {
            Clusterise();
        }
        if (Input.GetKeyDown(KeyCode.K) && !playMode.activeSelf && old_points.Count!=0)
        {
			clustering = true;
			InvokeRepeating("Clusterise", 0.5f, 0.5f);
        }
        else{
            string mesaj = "points.txt was not found and a new empty file was created or a it is empty" ;
            Dev_Text.text=mesaj;
        }
        if (Input.GetKeyDown(KeyCode.D) && !playMode.activeSelf)
        {
            toggle_display_Clusters();
        }
        if (moved == 0 && clustering && !playMode.activeSelf)
        {
            clustering = false;
            CancelInvoke();
            string mesaj = "K-means has stoppend after " + (iterations - 1) + " iterations";
            Dev_Text.text=mesaj;
            SetNavGoalCoords();
        }
    }

    void SetNavGoalCoords()
	{
        float new_x = 0f;
        float new_y = 0f;
        float sum_x = 0, sum_y = 0;

        for (int i = 0; i < cluster_centers.Count; i++)
		{
            sum_x += cluster_centers[i].transform.position.x ;
            sum_y += cluster_centers[i].transform.position.y ;
        }
        float new_x_t = sum_x / cluster_centers.Count;
        float new_y_t = sum_y / cluster_centers.Count;

		new_x = (float)Math.Round(new_x_t);
		new_y = (float)Math.Round(new_y_t);

		if (new_x < new_x_t)
			new_x = new_x + 0.5f;
		else
			new_x = new_x - 0.5f;

		if (new_y < new_y_t)
			new_y = new_y + 0.5f;
		else
			new_y = new_y - 0.5f;

        Vector3 navCoords = new Vector3(new_x, new_y, 0.5f);

        if (!isWall(navCoords))
		{
            navGoal.transform.position = new Vector3(new_x, new_y, 0.5f);
		}
        else
		{
            int step = 1;
            while (true)
            {
                if (!isWall(new Vector3(new_x + step, new_y, 0.5f)))
                {
                    navGoal.transform.position = new Vector3(new_x + step, new_y, 0.5f);
                    break;
                }
                else
                    if (!isWall(new Vector3(new_x - step, new_y, 0.5f)))
                {
                    navGoal.transform.position = new Vector3(new_x - step, new_y, 0.5f);
                    break;
                }
                else
                    if (!isWall(new Vector3(new_x, new_y + step, 0.5f)))
                {
                    navGoal.transform.position = new Vector3(new_x, new_y + step, 0.5f);
                    break;
                }
                else
                    if (!isWall(new Vector3(new_x, new_y - step, 0.5f)))
                {
                    navGoal.transform.position = new Vector3(new_x, new_y - step, 0.5f);
                    break;
                }
                step++;
            }
        }
    }

    bool isWall(Vector3 place)
    {
        Vector3Int coords = new Vector3Int((int)Math.Floor(place.x), (int)Math.Floor(place.y), 0);

        TileBase tile = tilemap.GetTile(coords);
        if (tile != null)
        {
            //Debug.Log("Not null");
            if (tile.name == "WallPiece")
            {
                //Debug.Log(tile.name);
                return true;
            }
            return false;
        }
        return false;
    }

    void K_means()
    {
        while (moved != 0)
        {
            clusterize_Points();

            move_Cluster_Centers();

            iterations++;
        }
        string mesaj = "K-means has stoppend after " + (iterations - 1) + " iterations";
        Dev_Text.text=mesaj;
        SetNavGoalCoords();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll.gameObject.name);
        if(coll.gameObject.name == "Player")
        {
            positions.Add(coll.gameObject.transform.position);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
	{
        if (coll.gameObject.name == "Player")
        {
            float x, y, z, new_x, new_y;
            x = coll.gameObject.transform.position.x;
            y = coll.gameObject.transform.position.y;
            z = 0.5f;
            new_x = (float)Math.Round(x);
            new_y = (float)Math.Round(y);

            if (new_x > x)
                x = new_x - 0.5f;
            else
                x = new_x + 0.5f;

            if (new_y > y)
                y = new_y - 0.5f;
            else
                y = new_y + 0.5f;

            navGoal.transform.position = new Vector3(x, y, z);
        }
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void Save()
    {
        using (StreamWriter sw = new StreamWriter(@".\points.txt",true))
        {
            
            for (int i = 0; i < positions.Count; i++)
            {
                sw.WriteLine(Math.Round(positions[i].x,1).ToString() + " " + Math.Round(positions[i].y, 1).ToString());
            }
            sw.Close();
            Debug.Log("Saved");
        }
    }

    void Load()
    {
        old_points = new List<Vector2>();
        StreamReader sr = new StreamReader(@".\points.txt");
        string ceva;
        while ((ceva = sr.ReadLine()) != null)
        {
            string[] split = ceva.Split(' ');
            Vector2 point = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
            old_points.Add(point);
        }
        Debug.Log("Loaded");
    }

    void make_Colors()
    {
        colors = new List<Color>();
        for (float i = 1; i >= 0; i-=0.5f)
        {
            for (float j = 1; j >= 0; j-=0.5f)
            {
                for (float k = 1; k >= 0; k -= 0.5f)
                {
                    colors.Add(new Color(i, j, k));
                }
            }
        }
        colors.RemoveAt(0);
        colors.RemoveAt(colors.Count - 1);
        Debug.Log(colors.Count);
    }

    void initialize_Cluster_Centers_and_Points()
    {
        double x_min = double.MaxValue;
        double x_max = double.MinValue;
        double y_min = double.MaxValue;
        double y_max = double.MinValue;
        for (int i = 0; i < old_points.Count; i++)
        {
            
            if (old_points[i].x > x_max)
                x_max = old_points[i].x;
            if (old_points[i].x < x_min)
                x_min = old_points[i].x;
            if (old_points[i].y > y_max)
                y_max = old_points[i].y;
            if (old_points[i].y < y_min)
                y_min = old_points[i].y;
            GameObject point = Instantiate(point_prefab, old_points[i], Quaternion.identity);
            point.SetActive(false);
            points_go.Add(point);
        }

        System.Random rand = new System.Random();
        int col = 2;
        for (int i = 0; i < cl_number; i++)
        {
            double x = rand.NextDouble() * (x_max - x_min) + x_min;
            double y = rand.NextDouble() * (y_max - y_min) + y_min;

            GameObject center = Instantiate(clusterCenter, new Vector3((float)x, (float)y, 0f), Quaternion.identity);
            center.GetComponent<SpriteRenderer>().color = colors[col];
            center.SetActive(false);
            cluster_centers.Add(center);
            clusters.Add(new List<GameObject>());
            col += 2;
        }
    }
    void clusterize_Points()
    {
        clusters = new List<List<GameObject>>();
        for (int i = 0; i < cl_number; i++)
        {
            clusters.Add(new List<GameObject>());
        }

        for (int i = 0; i < points_go.Count; i++)
        {
            
            int k = 0;
            float dist = (float)Math.Pow(Math.Sqrt(Math.Pow((points_go[i].transform.position.x - cluster_centers[k].transform.position.x), 2) + Math.Pow((points_go[i].transform.position.y - cluster_centers[k].transform.position.y), 2)),2);
            for (int j = 1; j < cluster_centers.Count; j++)
            {
                if (dist > (float)Math.Pow(Math.Sqrt(Math.Pow((points_go[i].transform.position.x - cluster_centers[j].transform.position.x), 2) + Math.Pow((points_go[i].transform.position.y - cluster_centers[j].transform.position.y), 2)),2))
                {
                    dist = (float)Math.Pow(Math.Sqrt(Math.Pow((points_go[i].transform.position.x - cluster_centers[j].transform.position.x), 2) + Math.Pow((points_go[i].transform.position.y - cluster_centers[j].transform.position.y), 2)),2);
                    k = j;
                }
            }
            clusters[k].Add(points_go[i]);
            points_go[i].GetComponent<SpriteRenderer>().color = cluster_centers[k].GetComponent<SpriteRenderer>().color;

        }
    }

    void move_Cluster_Centers()
    {
        float x_sum;
        float y_sum;
        float new_center_x;
        float new_center_y;
        moved = 0;
        for (int i = 0; i < clusters.Count; i++)
        {
            new_center_x = cluster_centers[i].transform.position.x;
            new_center_y = cluster_centers[i].transform.position.y;
            if (clusters[i].Count > 0)
            {
                x_sum = 0;
                y_sum = 0;
                for (int j = 0; j < clusters[i].Count; j++)
                {
                    x_sum += clusters[i][j].transform.position.x;
                    y_sum += clusters[i][j].transform.position.y;
                }   
                new_center_x = x_sum / (float)clusters[i].Count;
                new_center_y = y_sum / (float)clusters[i].Count;
            }
            if (new_center_x != cluster_centers[i].transform.position.x || new_center_y != cluster_centers[i].transform.position.y)
            {
                cluster_centers[i].transform.position = new Vector2(new_center_x, new_center_y);
                moved++;
            }   
        }
        //Debug.Log(moved);
    }

    void toggle_display_Clusters()
    {
        vision_field.SetActive(!vision_field.activeSelf);
        for (int i = 0; i < clusters.Count; i++)
        {
            cluster_centers[i].SetActive(!cluster_centers[i].activeSelf);
        }
        for (int j = 0; j < points_go.Count; j++)
        {
            GameObject point = points_go[j];
            point.SetActive(!point.activeSelf);
        }
    }
    void Clusterise()
    {
        clusterize_Points();

        move_Cluster_Centers();

        iterations++;
    }
}
