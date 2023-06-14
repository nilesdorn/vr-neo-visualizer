using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class TargetDeselectHandler : MonoBehaviour, IDeselectHandler //This Interface is required to receive OnDeselect callbacks.
{
    public GameObject neo;
    public GameObject nameplate;
    public string designation;
    // Start is called before the first frame update
    void Start()
    {
        neo = GameObject.Find(designation);
        Debug.Log(neo);
        nameplate = neo.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void OnDeselect (BaseEventData data) 
	{
		nameplate.SetActive(false);
	}
}