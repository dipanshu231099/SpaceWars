using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class enemySpaceShip : MonoBehaviour
{
    public float forwardSpeed = 2.5f, rotationSpeed = 2f;
    WaitForSeconds shootDuration= new WaitForSeconds(0.07f);
    AudioSource gunAudio;

    ParticleSystem exp;
    LineRenderer shooter;
    float allowedToShoot;

    private Transform player;
    private Transform followerSensor;

    float health;

    // private float forwardAcceleration = 2.5f;
    // public float activeForwardSpeed;

    AudioSource explosion, burning;

    void Start()
    {
        player = GameObject.Find("PlayerShip").transform;
        exp = GetComponent<ParticleSystem> ();
        allowedToShoot = Time.time;
        gunAudio = GetComponent<AudioSource> ();
        followerSensor = transform.Find("playerFollower");
        shooter = transform.Find("enemyShooter").GetComponent<LineRenderer> ();
        shooter.enabled = false;
        shooter.startWidth = 0.001f;
        shooter.endWidth = 0.001f;
        explosion = GameObject.Find("ExplosionSound").GetComponent<AudioSource> ();
        burning = GameObject.Find("BurningSound").GetComponent<AudioSource> ();
        health = 100f;
    }

    void OnTriggerEnter(Collider other)
    {
        hitFaced(0f);
    }

    void FixedUpdate()
    {
        Vector3 origin = this.transform.position;
        if(Time.time > allowedToShoot + 1f)
        {
            allowedToShoot = Time.time;
            RaycastHit hitinfo;
            bool hit = Physics.SphereCast(origin,5f,this.transform.forward,out hitinfo,Mathf.Infinity);
            if(hit  
                && hitinfo.transform.name=="PlayerShip" 
                && hitinfo.transform.gameObject.GetComponent<SpaceShip>()!=null
                && hitinfo.transform.gameObject.GetComponent<SpaceShip>().GetHealth()>0
                && hitinfo.distance<250)
            {
                shooter.SetPosition(0,origin);
                shooter.SetPosition(1,player.transform.position);
                StartCoroutine(EnemyShoot());
            }
        }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.position - followerSensor.position), rotationSpeed*Time.deltaTime);
        this.transform.position += this.transform.forward * forwardSpeed * Time.deltaTime;
    }
    
    public void hitFaced(float percentDamage)
    {
        ParticleSystem exp = GetComponent<ParticleSystem> ();
        health -= percentDamage;
        if(health<=0){
            Explosion();
        }
    }

    void Explosion()
    {
        explosion.Play();
        exp.Play();
        Text Scoretxt = GameObject.Find("Score").GetComponent<Text> ();
        int previousScore = int.Parse(Scoretxt.text);
        previousScore+=1;
        Scoretxt.text = ""+previousScore;
        Destroy(transform.gameObject, 1f);
    }

    IEnumerator EnemyShoot()
    {
        gunAudio.Play();
        shooter.enabled = true;
        explosion.Play();
        player.GetComponent<SpaceShip> ().hitFaced(5f);
        yield return shootDuration;
        shooter.enabled = false;
    }
}