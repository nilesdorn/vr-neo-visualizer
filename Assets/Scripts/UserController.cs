using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    PlayerControls controls;
    Vector2 refPos;
    Vector2 lastPos;
    Vector3 rotation;
    Vector3 movement;
    bool rotating = false;
    bool moving = false;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Rotate.performed += context => BeginRotate();
        controls.Player.Rotate.canceled += context => EndRotate();
        controls.Player.Move.performed += context => BeginMove();
        controls.Player.Move.canceled += context => EndMove();
        controls.Player.MousePosition.performed += context => TranslateView(context.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    void BeginRotate()
    {
        refPos = controls.Player.MousePosition.ReadValue<Vector2>();
        rotating = true;
        //Debug.Log("begin" + refPos);
    }
    
    void EndRotate()
    {
        rotating = false;
        //Debug.Log("end");
    }

    void BeginMove()
    {
        refPos = controls.Player.MousePosition.ReadValue<Vector2>();
        moving = true;
    }

    void EndMove()
    {
        moving = false;
    }

    void TranslateView(Vector2 pos)
    {
        if (pos.x - lastPos.x == 0 || pos.y - lastPos.y == 0)   
        {
            //Debug.Log("new ref");
            refPos = pos;
        }
        
        if (rotating)
        {
            Debug.Log(pos);
            Vector2 offset = pos - refPos;
            rotation.x = -(offset.y);
            rotation.y = -(offset.x);
            transform.eulerAngles += rotation;
            //Debug.Log("rotating" + offset);
        }

        if (moving)
        {
            Vector2 offset = pos - refPos;
            movement.x = offset.x / 1000;
            movement.y = offset.y / 1000;
            transform.position += movement;
        }

        lastPos = pos;
    }
}
