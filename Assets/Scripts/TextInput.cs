using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextInput : MonoBehaviour, ISelectHandler
{
    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";
    
    
   

    public void OnSelect(BaseEventData eventData)
    {
        
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

        if (overlayKeyboard != null)
        {
            inputText = overlayKeyboard.text;
        }
       
        
        gameObject.GetComponent<InputField>().text = inputText;
    }
}
