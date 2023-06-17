using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        //Debug.Log(startPos);
        GenerateGrid();    
    }

    public void GenerateGrid()
    {
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var createdTile = Instantiate(GameManager.Instance.tilePrefab, transform, false);
                createdTile.transform.localPosition = new Vector3(x * 4, 0.01f, z * 4);
                createdTile.name = "Tile " + x + " " + z;
            }
        }
    }
}


