using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public GameObject navGoal;
    public GameObject player;
    public GameObject adv;
    public GameObject grid;
    public GameObject canvas2;
    public GameObject PlayMode;
    public GameObject DevMode;

    // Start is called before the first frame update
    void Start()
    {
        navGoal.SetActive(false);
        player.SetActive(false);
        adv.SetActive(false);
        grid.SetActive(false);
        DevMode.SetActive(false);
        PlayMode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void setup_play_mode()
	{
        common_objects();
        PlayMode.SetActive(true);
    }

    public void setup_dev_mode()
	{
        common_objects();
        DevMode.SetActive(true);
    }

    void common_objects()
	{
        navGoal.SetActive(true);
        player.SetActive(true);
        adv.SetActive(true); 
        grid.SetActive(true);
        canvas2.SetActive(false);

    }
}
