using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CubeGen : MonoBehaviour
{
     public GameObject otherPrefab;
    public GameObject spherePrefab;
    public GameObject player;
    public KeyCode activationKey = KeyCode.G;
     public KeyCode newPrefabKey = KeyCode.J;
    public int gridWidth = 15;
    public int gridHeight = 15;
        public GameObject playerPrefab;  // Add this new variable
    public KeyCode respawnKey = KeyCode.H;
    public float outlierThreshold = 2.0f;
    public float offset = 0.06f;
    public float minRespawnDistance = 0.5f; 
        public float movementTime = 2f;
    public Vector3 particleSpawnOffset; // Add this new variable




private void OnDrawGizmos()
{
    Vector3 adjustedTransformPosition = transform.position + particleSpawnOffset;
    Vector3 center = adjustedTransformPosition + new Vector3(gridWidth * offset, gridHeight * offset, 0f) * 0.5f;
    Vector3 cubeSize = new Vector3(gridWidth * offset, gridHeight * offset, 0f);

    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(center, cubeSize);
}
    private void Update()
    {
        if (Input.GetKeyDown(activationKey) && player != null)
        {
            Transform playerTransform = player.transform;
            Destroy(player);
            transform.position = playerTransform.position;
            GenerateSphereGrid(gridWidth, gridHeight);
        }
                if (Input.GetKeyDown(respawnKey))
        {
            
            StartCoroutine(MoveSpheresAndRespawnPlayer());
        }
         if (Input.GetKeyDown(newPrefabKey) && player != null)
        {
                  Transform playerTransform = player.transform;
            Destroy(player);
            transform.position = playerTransform.position;   
            SpawnOtherPrefabGrid(gridWidth, gridHeight);
        }
    }

    private void GenerateSphereGrid(int width, int height)
    {
        if (!spherePrefab)
        {
            Debug.LogError("Sphere prefab has not been assigned!");
            return;
        }

        Vector3 parentPosition = transform.position + particleSpawnOffset;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(
                    parentPosition.x + x * offset,
                    parentPosition.y + y * offset,
                    parentPosition.z
                );
                Instantiate(spherePrefab, position, Quaternion.identity, transform);
            }
        }
    }
    
     private IEnumerator MoveSpheresAndRespawnPlayer()
    {
        if (player == null)
        {
            Vector3 averagePosition = GetAveragePositionWithoutOutliers();

            float startTime = Time.time;
            float elapsedTime = 0f;

            List<Vector3> initialPositions = new List<Vector3>();

            foreach (Transform sphere in transform)
            {
                initialPositions.Add(sphere.position);

                // Set isKinematic back to false for all the spheres after moving them
                Rigidbody rb = sphere.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }

            bool allSpheresInRange = false;

            while (elapsedTime < movementTime && !allSpheresInRange)
            {
                elapsedTime = Time.time - startTime;
                float t = elapsedTime / movementTime;
                allSpheresInRange = true; // Reset the flag to true before checking distances

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform sphere = transform.GetChild(i);
                    sphere.position = Vector3.Lerp(initialPositions[i], averagePosition, t);

                    // Respawn player when spheres are within the minRespawnDistance
                    float distance = Vector3.Distance(sphere.position, averagePosition);
                    if (distance > minRespawnDistance)
                    {
                        allSpheresInRange = false;
                    }
                }

                yield return null; // Wait for the next frame
            }


            RespawnPlayerAtAveragePosition(); // Respawn the player after moving the spheres
            DeleteSpherePrefabs(); // Delete the spheres after respawning player
        }
    }

    private void RespawnPlayerAtAveragePosition()
    {
        if (player == null)
        {
            Vector3 averagePosition = GetAveragePositionWithoutOutliers();
            player = Instantiate(playerPrefab, averagePosition, Quaternion.identity);
        }
    }

    private Vector3 GetAveragePositionWithoutOutliers()
    {
        Vector3 totalPosition = Vector3.zero;
        List<Vector3> positions = new List<Vector3>();
        int count = 0;

        foreach (Transform sphere in transform)
        {
            positions.Add(sphere.position);
        }

        Vector3 tempTotalPosition = Vector3.zero;
        foreach (Vector3 position in positions)
        {
            tempTotalPosition += position;
        }
        Vector3 tempAveragePosition = tempTotalPosition / positions.Count;

        foreach (Vector3 position in positions)
        {
            if (Vector3.Distance(tempAveragePosition, position) <= outlierThreshold)
            {
                totalPosition += position;
                count++;
            }
        }

        Vector3 averagePosition = totalPosition / count;
        return averagePosition;
    }
        private void SpawnOtherPrefabGrid(int width, int height)
    {
        if (!otherPrefab)
        {
            Debug.LogError("Other prefab has not been assigned!");
            return;
        }

        Vector3 parentPosition = transform.position + particleSpawnOffset;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(
                    parentPosition.x + x * offset,
                    parentPosition.y + y * offset,
                    parentPosition.z
                );
                Instantiate(otherPrefab, position, Quaternion.identity, transform);
            }
        }
    }

    private void DeleteSpherePrefabs()
    {
        while (transform.childCount > 0)
        {
            Transform sphereChild = transform.GetChild(0);
            sphereChild.SetParent(null);
            Destroy(sphereChild.gameObject);
        }
    }
}
