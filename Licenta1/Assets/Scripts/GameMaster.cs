using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject level;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject PlayMode;

    // Start is called before the first frame update
    void Start()
    {
        level.SetActive(false);
        canvas1.SetActive(false);
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
        level.SetActive(true);
        canvas2.SetActive(false);
        canvas1.SetActive(false);
        PlayMode.SetActive(true);
    }

    public void setup_dev_mode()
	{
        level.SetActive(true);
        canvas2.SetActive(false);
        canvas1.SetActive(true);
        PlayMode.SetActive(false);
    }

}
