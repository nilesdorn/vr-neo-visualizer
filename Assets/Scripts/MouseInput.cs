using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInput : MonoBehaviour
{
    private Inputs _inputs;
    private bool isDragging = false;
    private Vector2 refPos;
    private Vector3 rotation;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //_inputs.UI.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Click");
            refPos = mousePos;
            isDragging = true;
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            Debug.Log("No Click");
            isDragging = false;
        }
        
        if (isDragging)
        {
            Vector2 offset = mousePos - refPos;
            rotation.y = -(offset.x);
            rotation.x = -(offset.y);
            transform.eulerAngles += rotation;

        }
        
        
        


    
    }

   


}
