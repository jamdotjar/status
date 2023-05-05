using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gas : MonoBehaviour
{
 public float invertedGravityIntensity = 9.81f; // Control the intensity of the inverted gravity

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing!");
            return;
        }
    }

    private void FixedUpdate()
    {
        // Apply the inverted gravity force on every FixedUpdate
        rb.AddForce(Vector3.up * invertedGravityIntensity, ForceMode.Acceleration);
    }
}