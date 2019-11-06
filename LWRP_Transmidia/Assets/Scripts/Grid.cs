using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    MeshCollider mesh;

    int[,] grid;

    // 0  means not walkable
    // 1 means walkable
    // 2 means interaction

    [SerializeField]
    GameObject gridDebug;

    [SerializeField]
    GameObject gridDebugInit;

    [SerializeField]
    Transform[] obstacles;
    [SerializeField]
    MeshCollider[] obstaclesCollider;
    [SerializeField]
    bool debugModeOn;

    int x = 0;
    int z = 0;

    private void Awake()
    {
        mesh = GetComponent<MeshCollider>();
        grid = new int[(int)mesh.bounds.size.x, (int)mesh.bounds.size.z];
        // Discover the grid size of the grid
        //Debug.Log($"Grid size = {grid.GetLength(0)}, {grid.GetLength(1)}");
    }

    void Start()
    {
        if(transform.position.x != 0) x = (int)(transform.position.x * -1);
        if(transform.position.z != 0) z = (int)(transform.position.z * -1);
        Debug.Log($"x = {x}, z = {z}");
        AssingGridValues();
        // Debug for looking the size of the gameObject that contains the script
        //Debug.Log($"Bounds X = {mesh.bounds.size.x}, Bounds Y = {mesh.bounds.size.y}, Bounds Z = {mesh.bounds.size.z}");
    }

    private void AssingGridValues()
    {
        for(int rows = 0; rows < grid.GetLength(0); rows++) 
        {
            for(int columns = 0; columns < grid.GetLength(1); columns++) 
            {
                grid[rows, columns] = 1;
            }
        }
        SetColliders();
    }

    private void SetColliders()
    {
        foreach(Transform t in obstacles) {
            int size_x = (int)t.gameObject.GetComponent<MeshCollider>().bounds.size.x;
            int size_z = (int)t.gameObject.GetComponent<MeshCollider>().bounds.size.z;
            int intPos_x = (int)((t.position.x + size_x) + x - size_x);
            int intPos_z = (int)((t.position.z + size_z) - z - size_z);
            Debug.Log($"intPos_x = {intPos_x}, intPos_z = {intPos_z} ");
            for(int rows = 0; rows < t.gameObject.GetComponent<MeshCollider>().bounds.size.x; rows++) 
            {
                for(int columns = 0; columns < t.gameObject.GetComponent<MeshCollider>().bounds.size.z; columns++)
                {
                    grid[intPos_x + rows, intPos_z + columns] = 0;
                }
            }
        }
        if(debugModeOn) SpawnDebug();
        //SetInvalidGridPoints();
    }

    private void SpawnDebug() 
    {
        for(int rows = 0; rows < grid.GetLength(0); rows++) 
        {
            for(int columns = 0; columns < grid.GetLength(1); columns++) 
            {
                if (grid[rows, columns] == 1)Instantiate(gridDebug, new Vector3(rows - x, transform.position.y + 1, columns + z), Quaternion.identity);
            }
        }
        Instantiate(gridDebugInit, new Vector3(0 - x, transform.position.y + 1, 0 + z), Quaternion.identity);
    }

    public bool ValidateMovement(Vector2 requestedIndex) 
    {
        if (requestedIndex.x < grid.Length && requestedIndex.y < grid.Length) 
        {
            if (grid[(int)requestedIndex.x, (int)requestedIndex.y] == 1) return true;
            else return false;
        }
        else return false;
    }

    // private void SetInvalidGridPoints() 
    // {
    //     int rows = 0;
    //     int columns = 0;
    //     int index_x = 0;
    //     int index_y = 1;
    //     int index_z = 0;
    //     Vector3 position = transform.position;
    //     for(rows = 0; rows < grid.GetLength(0); rows++) 
    //     {
    //         for(columns = 0; columns < grid.GetLength(1); columns++) 
    //         {
    //             if (grid[rows, columns] != Vector3.zero)isGridValid[rows, columns] = true;
    //             else isGridValid[rows, columns] = false; // THIS DOESNT WORK, THE IF STATEMENT SHOULD RUN AFTER THE SetInvalidGridPoints METHOD SO ANOTHER METHOD SHOULD BE CREATED TO FIX THIS ISSUE.
    //             index_z--;
    //             if(isGridValid[rows, columns]) Instantiate(gridDebug, grid[rows, columns], Quaternion.identity);
    //         }
    //         index_x++;
    //         index_z = 0;
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
