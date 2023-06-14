using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectObjectInScene : MonoBehaviour
{
    public Text infoText;
    public Text targetName;
    public string designation;
    public GameObject neo;
    public GameObject nameplate;
    public InputField textInput;
    // Start is called before the first frame update
    void Start()
    {
        nameplate = transform.GetChild(0).gameObject;
        textInput = GameObject.Find("TargetInputField").GetComponent<InputField>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectObject()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Nameplate");
        foreach (GameObject np in gameObjects)
        {
            np.SetActive(false);
        }
       
        nameplate.SetActive(true);
        textInput.text = designation;
        Debug.Log(designation);

        var orbitingBody = GetComponent<OrbitingBody>();
        targetName = GameObject.Find("TargetInfoName").GetComponent<Text>();
        infoText = GameObject.Find("TargetInfoText").GetComponent<Text>();
        targetName.text = "Target: " + designation;
        infoText.text = ("e = " + orbitingBody.e + "\n" + "a = " + orbitingBody.a + "\n" 
        + "i = " + orbitingBody.i + "\n" + "node = " + orbitingBody.node + 
        "\n" + "peri = " + orbitingBody.peri + "\n" + "tp = " + orbitingBody.tp + "\n" + 
        "n = " + orbitingBody.n);
    }
}
