using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Info")]
public class BuildingInfo : ScriptableObject
{
    [Header("Basic Objects")]
    public GameObject emptyContainer;   
    public  ItemSet objectSet; 
    
    [Header("Building settings")]
    public float scale;
    public int wallLayer;
    public int floorLayer;
    public int roofLayer;
    public int exteriorLayer;
    public float gridSize = 4;

    [Header("Instantiated objects")]
    public List<Floor> floors;
    public List<GameObject> bases;


    public GameObject instantiatedMarker = null;
    public GameObject baseObject;     
    public GameObject temporary;
    public Transform lastMarkerTransform;
    public Vector3 baseTransformPos;
    public Quaternion baseTransformRot;
    public int chosenTab;
    public int currentFloor = 1;
    public int selectedWall = 0;
    public int selectionTracker = -1;
    public int tabSelectionTracker = -1;
    public bool isBuilding = true;
    public float baseHeight = 0;
    public float baseHeightTracker = 0;

}


