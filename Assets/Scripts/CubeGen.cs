using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CubeGen : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject player;
    public KeyCode activationKey = KeyCode.G;
    public int gridWidth = 15;
    public int gridHeight = 15;

    public float offset = 0.06f;
    public float speedUpTime = 2f;

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position + new Vector3(gridWidth * offset, gridHeight * offset, 0f) * 0.5f;
        Vector3 cubeSize = new Vector3(gridWidth * offset, gridHeight * offset, 0f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, cubeSize);
    }
    private void Update()
    {
        if (Input.GetKeyDown(activationKey) && player != null)
        {
            Transform playerTransform = player.transform;
            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Make the player transparent
            player.GetComponent<Collider2D>().enabled = false; // Remove collision
            transform.position = playerTransform.position;
            StartCoroutine(GenerateGridWithSpeedCurve(gridWidth, gridHeight, speedUpTime));
        }
    }

    public IEnumerator GenerateGridWithSpeedCurve(int width, int height, float speedUpDuration)
    {
        if (!spherePrefab)
        {
            Debug.LogError("Sphere prefab has not been assigned!");
            yield break;
        }

        Vector3 parentPosition = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(
                    parentPosition.x + x * offset,
                    parentPosition.y + y * offset,
                    parentPosition.z
                );
                GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity, transform);
                StartCoroutine(SpeedCurveCoroutine(sphere.GetComponent<Rigidbody2D>(), speedUpDuration));
                yield return null;
            }
        }
    }

    private IEnumerator SpeedCurveCoroutine(Rigidbody2D rb, float speedUpDuration)
    {
        float elapsedTime = 0;
        // Store the original gravity scale of the sphere prefab
        float originalGravityScale = rb.gravityScale;

        // Start with a gravity scale of 0 so the spheres start moving slowly
        rb.gravityScale = 0;

        while (elapsedTime < speedUpDuration)
        {
            float progress = elapsedTime / speedUpDuration;
            // Use an animation curve to smoothly increase the gravity scale
            rb.gravityScale = Mathf.Lerp(0, originalGravityScale, progress);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        rb.gravityScale = originalGravityScale;
    }
}