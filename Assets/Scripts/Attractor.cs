using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static List<Attractor> Attractors;
    public Rigidbody rb;
    float radiusScaler=(float)50;
    public const float G = 1000;
    const float pi = (float)3.1427;

    public Vector3 vel;

    static float distanceMultiplier = 10;

    /* radius, density, distance_from_sun */
    Dictionary<string, float[]> solarSystem = new Dictionary<string, float[]>{
        {"sun",new float[] {(float)0.696*distanceMultiplier,(float)1.4,0*distanceMultiplier }}, // sun
        {"mercury",new float[] {(float)0.004*distanceMultiplier,(float)5.4,57*distanceMultiplier}}, // mercury
        {"venus",new float[] {(float)0.012*distanceMultiplier,(float)5.1,108*distanceMultiplier}}, // venus
        {"earth",new float[] {(float)0.012*distanceMultiplier,(float)5.5,149*distanceMultiplier}}, // earth
        {"mars",new float[] {(float)0.006*distanceMultiplier,(float)3.9, 228*distanceMultiplier}}, // mars
        {"jupiter",new float[] {(float)0.142*distanceMultiplier,(float)1.3, 780*distanceMultiplier}}, // jupiter
        {"saturn",new float[] {(float)0.12*distanceMultiplier,(float)0.7, 1437*distanceMultiplier}}, // saturn
        {"uranus",new float[] {(float)0.051*distanceMultiplier,(float)1, 2871*distanceMultiplier}}, // saturn
    };

    // My own attraction mechanism
    void attract(Attractor other) {
        Rigidbody otherRB = other.rb;
        Vector3 direction = (rb.position - otherRB.position);
        float distance = direction.magnitude;

        float GravitationalForce = G*(rb.mass * otherRB.mass)/(distance*distance);
        Vector3 NormalisedDirection = direction.normalized;
        Vector3 ForceDirection = GravitationalForce*NormalisedDirection;
        
        otherRB.AddForce(ForceDirection);
    }

    // Start is called before the first frame update
    void Start()
    {   
        rb = this.gameObject.GetComponent<Rigidbody>();

        // calculating the radius
        string name = this.name;

        float radius = solarSystem[name][0];
        float perDiectionScale = radius*radiusScaler;
        Vector3 scale = new Vector3(perDiectionScale,perDiectionScale,perDiectionScale);
        this.transform.localScale = scale;

        // estimating the mass of the celestial object
        float Volume = (float)(1.333333)*pi*radius*radius*radius;
        float massEstimated = solarSystem[name][1]*Volume;
        Debug.Log(this.name+" mass: "+massEstimated);

        // setting the distance from the sun
        float distanceFromSun = solarSystem[name][2];
        transform.position = new Vector3(distanceFromSun,0,0);

        // setting the mass
        rb.mass = massEstimated; 

        // setting initial velocity
        float radiusSun = solarSystem["sun"][0];
        float SunVolume = (float)(1.333333)*pi*radiusSun*radiusSun*radiusSun;
        float massSun = solarSystem["sun"][1]*SunVolume;
        float initialVel = distanceFromSun!=0?Mathf.Sqrt((G*massSun)/distanceFromSun):0;
        rb.velocity = new Vector3(0,0,initialVel);
    }

    void OnEnable() 
    {
        if(Attractors==null){
            Attractors = new List<Attractor>();
        }
        Attractors.Add(this);
    }

    void OnDisable() 
    {
        Attractors.Remove(this);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Attractor attractor in Attractors)
        {
            if(attractor!= this)
            {
                attract(attractor);
            }
        }
    }

    public float GetDistanceMultiplier() {
        return distanceMultiplier;
    }
}
