using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NEOToggle : MonoBehaviour
{
    Toggle neoToggle;
    public GameObject attach;

    public void ToggleNEOs()
    {
        neoToggle = GetComponent<Toggle>();
        attach.SetActive(!neoToggle.isOn);
    }
}
