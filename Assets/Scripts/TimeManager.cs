using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeManager : MonoBehaviour
{
    public InputField dateInput;
    public Text currentJD;
    public Text currentCD;
    double julianDate;
    public bool pauseTime = false;
    public double time;
    public DateTime calendarDate;
    public double scaleTime = 0.2;
    public bool reverseTime = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GetCurrentTime();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayCurrentJD();
        DisplayCalendarDate();
        ManageTime();
    }

    public void DisplayCurrentJD()
    {
        currentJD.text = time.ToString();
    }

    public void DisplayCalendarDate()
    {
        double OADate = (time - 2415018.5);
        calendarDate = DateTime.FromOADate(OADate);
        currentCD.text = calendarDate.ToString();
    }
    public static double ToJulianDate(System.DateTime date)
    {
        return date.ToOADate() + 2415018.5;
    }

    public void GetCurrentTime()
    {
        time = ToJulianDate(System.DateTime.Now);
    }
    public void ManageTime()
    {
        if (!pauseTime)
        {
            time += scaleTime;
        }
    }

    public void PauseTime()
    {
        if (!pauseTime)
        {
            pauseTime = true;
        }
        else
        {
            pauseTime = false;
        }
    }

    public void ReverseTime()
    {
        
    
        Toggle timeToggle = GameObject.Find("Reverse Time").GetComponent<Toggle>();
    
        scaleTime *= -1;
        if (timeToggle.isOn)
        {
            reverseTime = true;
        }
        else
        {
            reverseTime = false;
        }
    }

    // Used by the Scale Time UI Slider
    public void OnValueChanged(float scale)
    {
        if (reverseTime)
        {
            scaleTime = scale * -1;
        }
        else
        {
            scaleTime = scale;
        }
    }

    public void ManualJulianDate()
    {
        julianDate = Double.Parse(dateInput.text);
        Debug.Log(julianDate);
        time = julianDate;

    }
}
