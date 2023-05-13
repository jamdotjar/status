using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour {
 
    // the target the camera should follow (usually the player)
    public Transform target;
 
    // the camera distance (z position)
    public float distance = -10f;
 
    // the height the camera should be above the target (AKA player)
    public float height = 0f;
 
    // damping is the amount of time the camera should take to go to the target
    public float damping = 5.0f;
 
    // map maximum X and Y coordinates. (the final boundaries of your map/level)
    public float mapX = 100.0f;
    public float mapY = 100.0f;
    public float mapMinX = 0.0f;
    public float mapMinY = 0.0f;
 
    // just private var for the map boundaries
    private float minX = 0f;
    private float maxX = 0f;
    private float minY = 0f;
    private float maxY = 0f;
  
    void Start () {
        // the map MinX and MinY are the position that the camera STARTS
        minX = mapMinX;
        minY = mapMinY;
        // the desired max boundaries
        maxX = mapX;
        maxY = mapY;
    }
 
    void Update () {
 

        Vector3 wantedPosition = target.TransformPoint(0, height, distance);
 

        wantedPosition.x = (wantedPosition.x < minX) ? minX : wantedPosition.x;
        wantedPosition.x = (wantedPosition.x > maxX) ? maxX : wantedPosition.x;
 


        wantedPosition.y = (wantedPosition.y < minY) ? minY : wantedPosition.y;
        wantedPosition.y = (wantedPosition.y > maxY) ? maxY : wantedPosition.y;

        // set the camera to go to the wanted position in a certain amount of time
        transform.position = Vector3.Lerp (transform.position, wantedPosition, (Time.deltaTime * damping));
    }
}