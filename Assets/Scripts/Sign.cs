using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : MonoBehaviour
{
    public float fadeSpeed = 1f;
    public Transform playerTransform;
    public float maxDistanceToShow = 5f;
    public Vector3 offset = new Vector3(0, 0, 0); // Added offset field
    private TextMeshProUGUI textUi;
    private float currentAlpha = 0f;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, maxDistanceToShow); // Added offset to gizmo position
    }
    private void Start()
    {
        textUi = GetComponent<TextMeshProUGUI>();
        textUi.alpha = 0f;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogError("playerTransform is null");
            return;
        }

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position + offset); // Added offset to distance check
        if(distanceToPlayer <= maxDistanceToShow)
        {
            currentAlpha = 1f;
        }
        else
        {
            currentAlpha = 0f;
        }
        textUi.alpha = Mathf.Lerp(textUi.alpha, currentAlpha, fadeSpeed * Time.deltaTime);
    }
}