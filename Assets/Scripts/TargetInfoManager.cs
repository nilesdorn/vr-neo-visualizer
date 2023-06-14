using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.
using TMPro;

public class TargetInfoManager : MonoBehaviour, ISelectHandler // required interface when using the OnSelect method.
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
        neo = GameObject.Find(designation);
        nameplate = neo.transform.GetChild(0).gameObject;
        textInput = GameObject.Find("TargetInputField").GetComponent<InputField>();
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Nameplate");
        foreach (GameObject np in gameObjects)
        {
            np.SetActive(false);
        }
        nameplate.SetActive(true);
        textInput.text = designation;
        var orbitingBody = neo.GetComponent<OrbitingBody>();
        targetName = GameObject.Find("TargetInfoName").GetComponent<Text>();
        infoText = GameObject.Find("TargetInfoText").GetComponent<Text>();
        targetName.text = "Target: " + designation;
        infoText.text = ("e = " + orbitingBody.e + "\n" + "a = " + orbitingBody.a + "\n" 
        + "i = " + orbitingBody.i + "\n" + "node = " + orbitingBody.node + 
        "\n" + "peri = " + orbitingBody.peri + "\n" + "tp = " + orbitingBody.tp + "\n" + 
        "n = " + orbitingBody.n);
    }

 
}
