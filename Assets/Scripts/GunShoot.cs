using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    Transform gunpoint;
    public Transform viewport;
    private GameObject pauseObject;
    WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    float nextShoot;
    float shootRate = 1f;
    LineRenderer lineRenderer;
    AudioSource gunAudio;

    Vector3 origin, pseudoSource, destination;
    // Start is called before the first frame update
    void Start()
    {
        pauseObject = GameObject.Find("PauseStatus");
        gunAudio = GetComponent<AudioSource>();
        nextShoot = Time.time;
        gunpoint = this.transform;
        origin = viewport.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth=0.001f;
        lineRenderer.endWidth=0.001f;
        pseudoSource = gunpoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObject.GetComponent<PauseHandler>().gamePaused)
        {
            return;
        }
        bool mouseClick = Input.GetMouseButtonDown(0);
        if(mouseClick && Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            origin = viewport.position;
            pseudoSource = gunpoint.position;
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(origin,viewport.forward,out hitInfo,Mathf.Infinity);
            if(hit)
            {
                destination = hitInfo.transform.position;
                if(hitInfo.transform.tag=="enemy")
                {
                    hitInfo.transform.GetComponent<enemySpaceShip>().hitFaced(10f);
                }
            }
            else
            {
                destination = origin + viewport.forward * 100f;
            }
            
            lineRenderer.SetPosition(0,pseudoSource);
            lineRenderer.SetPosition(1,destination);
            StartCoroutine(Shoot(hit));
        }
    }

    IEnumerator Shoot(bool hit)
    {
        gunAudio.Play();
        lineRenderer.enabled = true;
        if(hit){
            GameObject.Find("MinuteExplosionSound").GetComponent<AudioSource>().Play();
        }
        yield return shotDuration;
        lineRenderer.enabled = false;
    }
}
