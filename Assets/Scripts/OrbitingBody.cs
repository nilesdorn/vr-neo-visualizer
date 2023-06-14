using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingBody : MonoBehaviour
{
    // Variable definitions

    // Orbital elements given by Horizons
    public double e;    // Eccentricity; unitless
    public double a;    // Semi-major axis; au
    double q;    // Perihelion distance; au
    public double i;    // Inclination; degrees
    public double node; // Longitude of the ascending node; degrees
    public double peri; // Argument of perihelion; degrees
    double mC;   // Mean anomaly [current]; degrees
    public double tp;   // Time of perihelion passage; TDB
    double period;   // Period; days or years
    public double n;    // Mean motion; degrees/day
    double Q;    // Aphelion distance; au

    // Calculated orbital elements
    double M;    // Mean anomaly at time t; degrees
    double E;    // Eccentric anomaly at time t; degrees
    double v;    // True anomaly at time t; degrees
    double r;    // Magnitude of radius at time t; au

    // Other
    GameObject NEOManager;
    
    private double currentTime;  

    // Start is called before the first frame update
    void Start()
    {
        ConvertToRads();
        NEOManager = GameObject.FindWithTag("NEOManager");
        var timeManager = NEOManager.GetComponent<TimeManager>();
        currentTime = timeManager.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        var timeManager = NEOManager.GetComponent<TimeManager>();
        currentTime = timeManager.time;
        MeanAnomaly();
        EccentricAnomaly();
        TrueAnomaly();
        RadiusMag();
        CoordinateTransformations();
        
        
    }

   
  
    
    // Converts elements given in degrees to radians
    void ConvertToRads()
    {
        i *= (Math.PI / 180);
        node *= (Math.PI / 180);
        peri *= (Math.PI / 180);
        n *= (Math.PI / 180);
    }
    
    
    // Calculate & normalize mean anomaly
    void MeanAnomaly()
    {
        M = n * (currentTime - tp);
        //Debug.Log("M = " + M); 
        M %= (2 * Math.PI);
        if (M < 0)
        {
            M += (2 * Math.PI);
        }
        // Debug.Log("Normalized M = " + M); 
    }

    // Calculate f(E)
    double F_E(double En)
    {
        return (En - (e * Math.Sin(En)) - M);
    }

    //Calculate f'(E)
    double DF_E(double En)
    {
        return (1 - (e * Math.Cos(En)));
    }

    // Approximate eccentric anomaly
    void EccentricAnomaly()
    {
        double En = M;
        double delta = 1f;
        for (int j = 0; delta > 1e-6 && j < 10; j++)
        {
            double En1 = En - (F_E(En) / DF_E(En));
            delta = Math.Abs(En1 - En);
            En = En1;
        }
        E = En;
        // Debug.Log("E = " + E); 
    }

    void TrueAnomaly()
    {
        v = 2 * Math.Atan(Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(E / 2));
        //Debug.Log("v = " + v); 
        if (v < 0)
        {
            v += (2 * Math.PI);
            //Debug.Log("Normalized v = " + v);
        }
    }

    void RadiusMag()
    {
        r = a * (1 - (e * Math.Cos(E)));
        // Debug.Log("r = " + r); 
    }

    void CoordinateTransformations()
    {
        double x = r * ((Math.Cos(node) * Math.Cos(peri + v)) - (Math.Sin(node) * Math.Sin(peri + v) * Math.Cos(i)));
        double y = r * (Math.Sin(i) * Math.Sin(peri + v));
        double z = r * ((Math.Sin(node) * Math.Cos(peri + v)) + (Math.Cos(node) * Math.Sin(peri + v) * Math.Cos(i)));

        transform.localPosition = new Vector3((float) x, (float) y, (float) z);
    }
}
