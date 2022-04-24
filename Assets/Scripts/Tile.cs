using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    GridManager gridManager;
    PathFinder pathFinder;
    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start() {
        if( gridManager != null) {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            // Debug.Log(coordinates);
            if(!isPlaceable) {
                gridManager.BlockNode(coordinates);
            }
        }
    }
    //properties in c#
    //helps to expose private fields of class to other classes without getters or setter methods
    public bool IsPlaceable
    {
        get
        {
            return isPlaceable;
        }
    }

    void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if(isSuccessful) {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyRecievers();
            }
        }
    }
}
