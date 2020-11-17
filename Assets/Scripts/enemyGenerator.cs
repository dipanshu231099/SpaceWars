using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;

    Text Scoretxt;
    int score;

    float starttime;
    private GameObject pauseObject;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        pauseObject = GameObject.Find("PauseStatus");
        player = GameObject.Find("PlayerShip");
        Scoretxt = GameObject.Find("Score").GetComponent<Text> ();
        starttime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObject.GetComponent<PauseHandler>().gamePaused)
        {
            return;
        }
        Vector3 randomPoint = player.transform.position + Random.insideUnitSphere * 500;
        if(Time.time > starttime+20)
        {
            starttime = Time.time;
            Debug.Log("Created a new Space ship");
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
        }
        
    }
}
