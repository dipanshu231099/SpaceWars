using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class shipStatusController : MonoBehaviour
{
    private Text boostText;
    private Text speedText;
    private Text healthText;
    private Text dangerText;
    SpaceShip ship;

    GameObject[] enemySpaceShips;
    // Start is called before the first frame update
    void Start()
    {
        boostText = GameObject.Find("playerBoosttxt").GetComponent<Text> ();
        speedText = GameObject.Find("playerSpeedtxt").GetComponent<Text> ();
        healthText = GameObject.Find("playerHealthtxt").GetComponent<Text> ();
        dangerText = GameObject.Find("Dangertxt").GetComponent<Text> (); 
        dangerText.enabled = false;
        ship = GetComponent<SpaceShip>();
    }

    float dotProduct (Vector3 a, Vector3 b)
    {
        float sum = a.x*b.x + a.y*b.y + a.z*b.z; 
        return sum;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemySpaceShips = GameObject.FindGameObjectsWithTag("enemy");
        dangerText.enabled=false;
        if(enemySpaceShips.Length!=0)
        {
            for(int i=0;i<enemySpaceShips.Length;i++)
            {
                float proximity = (enemySpaceShips[i].transform.position - transform.position).magnitude;
                if(proximity<500)
                {
                    dangerText.enabled=true;
                }
            }
        }

        float boostFuel = ship.GetBoost();
        boostText.text = Mathf.Round(boostFuel*100)/100 +"%";

        float health = ship.GetHealth();
        healthText.text = health + "%";

        float forwardProjection = -ship.activeForwardSpeed;
        speedText.text = Mathf.RoundToInt(forwardProjection) + " units";

    }
}
