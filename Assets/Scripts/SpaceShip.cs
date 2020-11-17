using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
    public float forwardSpeed = 250f, strafeSpeed = 75f, hoverSpeed = 50f;
    public float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;

    private float health;
    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;
    private float lookRateSpeed = 45f;

    private AudioSource explosion, burning;
    private ParticleSystem exp;
    private GameObject pauseObject;

    private float BoostRemaining = 100;
    private float BoostMagnitude = 50;
    private float boosting;

    private Vector2 lookInput, screenCenter, mouseDistance;

    void Start()
    {
        pauseObject = GameObject.Find("PauseStatus");
        explosion = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        burning = GameObject.Find("BurningSound").GetComponent<AudioSource>();
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;
        health = 100f;
        exp = GetComponent<ParticleSystem>();
        exp.maxParticles = 2;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnTriggerEnter(Collider other)
    {
        hitFaced(100f);
    }

    void Update()
    {
        if (pauseObject.GetComponent<PauseHandler>().gamePaused)
        {
            return;
        }

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x)/screenCenter.x;
        mouseDistance.y = (lookInput.y - screenCenter.y)/screenCenter.y;

        float mouseMagnitude = mouseDistance.magnitude;
        float maximumMagnitude = 1f;

        if(mouseMagnitude>maximumMagnitude){
            mouseDistance = mouseDistance / mouseMagnitude;
        }

        float yAngle = mouseDistance.y*lookRateSpeed*Time.deltaTime;
        float xAngle = mouseDistance.x*lookRateSpeed*Time.deltaTime;

        transform.Rotate(yAngle, xAngle,0f, Space.Self);

        BoostRemaining += Time.deltaTime * 0.1f;
        if(BoostRemaining>100)
        {
            BoostRemaining=100;
        }

        float inpVertical,inpHorizontal,inpHover;
        inpVertical = -1f* Input.GetAxisRaw("Vertical");
        inpHorizontal = -1f* Input.GetAxisRaw("Horizontal");
        inpHover = Input.GetAxisRaw("Hover");
        boosting = Input.GetAxisRaw("Boost");

        if(boosting==1f && inpVertical<0 && BoostRemaining>0.01f*Time.deltaTime){
            BoostRemaining-= 1f*Time.deltaTime;
            inpVertical *= BoostMagnitude;
        }

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, inpVertical, forwardAcceleration*Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, inpHorizontal, strafeAcceleration*Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, inpHover, hoverAcceleration*Time.deltaTime);


        transform.position += (transform.forward * activeForwardSpeed * Time.deltaTime);
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
        transform.position += transform.up * activeHoverSpeed * Time.deltaTime;
    }
    
    public void hitFaced(float percentDamage)
    {
        health -= percentDamage;
        exp.Play();
        if(health<=0){
            health=0;
            Explosion();
        }
    }

    void Explosion()
    {

        StartCoroutine(QuitToMainMenu(1.5f));
        explosion.Play();
        burning.Play();
        exp.loop = true;
        exp.maxParticles=300;
        exp.Play();
        Destroy(GameObject.Find("leftShooter").GetComponent<GunShoot>(),0f);
        Destroy(GameObject.Find("rightShooter").GetComponent<GunShoot>(),0f);
    }

    IEnumerator QuitToMainMenu(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    public float GetBoost()
    {
        return BoostRemaining;
    }

    public float GetHealth()
    {
        return health;
    }
}