using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aray : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab; // Assign the prefab in the Inspector
    [SerializeField] private int gridSize = 3; // Set the size of the grid (x * x)
    [SerializeField] private float spacing = 1.0f; // Set the spacing between the cubes

    private void Start()
    {
        GenerateCubeGrid();
    }

    private void GenerateCubeGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    Vector3 cubePosition = new Vector3(x * spacing, y * spacing, z * spacing);
                    Instantiate(cubePrefab, cubePosition, Quaternion.identity, transform);
                }
            }
        }
    }
}