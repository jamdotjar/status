using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private DoorBehavior door;

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

        if (hit.collider == null)
        {
            door.isOpen = true;
            lineRenderer.SetPosition(1, endPoint.position);
        }
        else
        {
            door.isOpen = false;
            lineRenderer.SetPosition(1, hit.point);
        }
    }
}