using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public Transform player;
    public Vector2 margin;
    public Vector2 smoothness;
    public BoxCollider2D levelBounds;

    private Vector3 minBound, maxBound;
    private Camera camera;
    private Vector2 cameraHalfExtents;

    void Start()
    {
        minBound = levelBounds.bounds.min;
        maxBound = levelBounds.bounds.max;
        camera = GetComponent<Camera>();
        cameraHalfExtents = new Vector2(camera.aspect * camera.orthographicSize, camera.orthographicSize);
    }
    
    void Update()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        
        float leftBound = minBound.x + cameraHalfExtents.x;
        float bottomBound = minBound.y + cameraHalfExtents.y;
        float rightBound = maxBound.x - cameraHalfExtents.x;
        float topBound = maxBound.y - cameraHalfExtents.y;

        targetPosition.x = Mathf.Clamp(targetPosition.x, leftBound, rightBound);
        targetPosition.y = Mathf.Clamp(targetPosition.y, bottomBound, topBound);

        float xDifference = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(targetPosition.x));
        float yDifference = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(targetPosition.y));

        if (xDifference >= margin.x || yDifference >= margin.y)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
            transform.position = newPosition;
        }
    }
}