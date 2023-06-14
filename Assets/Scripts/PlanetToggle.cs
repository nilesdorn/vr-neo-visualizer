using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetToggle : MonoBehaviour
{
    Toggle planetToggle;
    public GameObject planets;

    public void TogglePlanets()
    {
        planetToggle = GetComponent<Toggle>();
        planets.SetActive(!planetToggle.isOn);
    }
}
