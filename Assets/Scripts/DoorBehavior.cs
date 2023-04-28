using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool isOpen = false;
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    [SerializeField] float doorSpeed = 10f;
    [SerializeField] float doorOpenAmount = 3f;

    void Awake(){
       
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x, transform.position.y + doorOpenAmount, transform.position.z);
    }
    void Update(){
        
        if(isOpen)
        {
            OpenDoor();
        }
        else if(!isOpen)
        {
            CloseDoor();
        }
    }
    void OpenDoor()
    {
        if(transform.position != doorOpenPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
        }
    }

    void CloseDoor()
    {
        if(transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    
    }
    
}
