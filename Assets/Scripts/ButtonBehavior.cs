using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] DoorBehavior DoorBehavior;
    [SerializeField] string[] tags;
    [SerializeField] bool isDoorOpenButton;
    [SerializeField] bool isDoorCloseButton;
    [SerializeField] bool isDoorToggleButton;
    float buttonSizeY;

    Vector3 buttonUpPos;
    Vector3 buttonDownPos;

    float buttonSpeed = 1f;
    float buttonDelay = 0.2f;
    bool buttonPressed = false;

    void Awake()
    {
        buttonSizeY = transform.localScale.y/2;

        buttonUpPos = transform.position;
        buttonDownPos = new Vector3(transform.position.x, transform.position.y - buttonSizeY, transform.position.z);
    }
    
    void Update()
    {
        if (buttonPressed)
        {
            ButtonDown();
        }
        else if(!buttonPressed)
        {
            ButtonUp();
        }
    }

    void ButtonDown()
    {
        if (transform.position != buttonDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonDownPos, buttonSpeed * Time.deltaTime);
        }
    }
    void ButtonUp()
    {
        if (transform.position != buttonUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonUpPos, buttonSpeed * Time.deltaTime);
        }
        
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Array.Exists(tags, tag => tag.Equals(collision.tag)))
        {
            buttonPressed = !buttonPressed;

            if (isDoorOpenButton && !DoorBehavior.isOpen)
            {
                DoorBehavior.isOpen = !DoorBehavior.isOpen;
            }
            else if (isDoorCloseButton && DoorBehavior.isOpen)
            {
                DoorBehavior.isOpen = !DoorBehavior.isOpen;
            }

        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
    if (isDoorToggleButton)
    {
       StartCoroutine(ButtonUpDelay(buttonDelay));  
    }

    else if (!isDoorToggleButton)
    {
       if (Array.Exists(tags, tag => tag.Equals(collision.tag)))
        {
            buttonPressed = !buttonPressed;

            //make the door become closed if open or open if closed
            if (DoorBehavior.isOpen)
            {
                DoorBehavior.isOpen = !DoorBehavior.isOpen;
            }
            else if (!DoorBehavior.isOpen)
            {
                DoorBehavior.isOpen = !DoorBehavior.isOpen;
            }

        }
    }
    }

    IEnumerator ButtonUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        buttonPressed = false;
    
    }
    
}