using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private DoorBehavior DoorBehavior;
    [SerializeField] private bool opensDoor;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            gameObject.AddComponent<LineRenderer>();
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    void Update()
    {
        EmitLaser();
    }

    void EmitLaser()
    {
        Vector2 direction = endPoint.position - startPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint.position, direction, Vector2.Distance(startPoint.position, endPoint.position), blockingLayer);
        if(opensDoor)
        {
            //if it dosent hit anything
            if (hit.collider == null && DoorBehavior.isOpen == false)
            {
                DoorBehavior.isOpen = true;
                lineRenderer.SetPosition(1, endPoint.position);
            }
            //if it does 
            else if(hit.collider != null && DoorBehavior.isOpen == true)
            {
                DoorBehavior.isOpen = false;
                lineRenderer.SetPosition(1, hit.point);
            }
            }
            else if(!opensDoor){
            if (hit.collider == null && DoorBehavior.isOpen == true )
            {
                DoorBehavior.isOpen = false;
                lineRenderer.SetPosition(1, endPoint.position);
            }
            else if(hit.collider != null && DoorBehavior.isOpen == false)
            
            {
                DoorBehavior.isOpen = true;
                lineRenderer.SetPosition(1, hit.point);
            }
        
        }
    }
}