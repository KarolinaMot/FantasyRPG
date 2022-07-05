using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Building : MonoBehaviour
{
      [Header("Basic Objects")]
    public GameObject emptyContainer;   
    public  ItemSet objectSet; 
    
    [Header("Building settings")]
    public float scale = 1.2f;
    public int wallLayer;
    public int floorLayer;
    public int roofLayer;
    public int exteriorLayer;
    public float gridSize = 4;
    public Floor examp;

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


    public void MoveMarker(Vector3 markerPosition)
    {
        if(selectionTracker != selectedWall || tabSelectionTracker != chosenTab ){
            if(instantiatedMarker != null)
                DestroyImmediate(instantiatedMarker);

            selectionTracker = selectedWall;
            tabSelectionTracker = chosenTab;
        }

        if (instantiatedMarker == null)
        {
            InstantiateMarker(markerPosition);
            return;
        }

        lastMarkerTransform = instantiatedMarker.transform;
        instantiatedMarker.transform.position = new Vector3(markerPosition.x, (currentFloor-1)*6*scale+baseHeight*3, markerPosition.z);
    }

    public void InstantiateMarker(Vector3 markerPosition)
    {
        Vector3 pos = new Vector3(markerPosition.x+4, (currentFloor-1)*6, markerPosition.z);
        switch (chosenTab) {
            case 0:
                instantiatedMarker = Instantiate(objectSet.SimpleWallsT[selectedWall], pos, Quaternion.identity);
                break;
            case 1:
                instantiatedMarker = Instantiate(objectSet.WindowWallsT[selectedWall], pos, Quaternion.identity);
                break;
            case 2:
                instantiatedMarker = Instantiate(objectSet.DoorWallsT[selectedWall], pos, Quaternion.identity);
                break;       
            case 3:
                instantiatedMarker = Instantiate(objectSet.GroundT[selectedWall], pos, objectSet.GroundT[selectedWall].transform.rotation);
                break; 
            case 4:
                instantiatedMarker = Instantiate(objectSet.RoofT[selectedWall], pos, Quaternion.identity);
                break;  
            case 5:
                instantiatedMarker = Instantiate(objectSet.OutsideDetailsT[selectedWall], pos, Quaternion.identity);
                break;  
        }
        instantiatedMarker.SetActive(true);
        instantiatedMarker.transform.parent = transform;
    }

    public void ManageRotation(int degree){
        instantiatedMarker.transform.Rotate( 0, degree, 0);
        if(instantiatedMarker.transform.rotation.y >= 360){
             instantiatedMarker.transform.rotation = Quaternion.Euler(instantiatedMarker.transform.rotation.x, instantiatedMarker.transform.rotation.y -360, instantiatedMarker.transform.rotation.z);
        }
    }

    public void BuildingManager(){
        if(floors == null){
            floors = new List<Floor>();
        }
        if(floors.Count > 0){
            for(int i=0; i<floors.Count; i++){
                if(floors[i].floorNumber != 0 && floors[i].floorNumber == currentFloor){
                    PlaceWall();         
                    temporary.transform.parent = floors[i].floorObject.transform; 
                    floors[i].walls.Add(temporary);
                    PlaceBase();
                    return;
                }
            }
        }

        temporary = Instantiate(emptyContainer, new Vector3(0, 0, 0), Quaternion.identity);
        temporary.transform.parent = transform;
        temporary.name = "Floor"+currentFloor;

        floors.Add(ScriptableObject.Instantiate(examp));
        floors[floors.Count-1].Init(currentFloor, temporary);
        
        PlaceWall();
        temporary.transform.parent = floors[floors.Count - 1 ].floorObject.transform; 
    
        if(floors[floors.Count - 1].walls == null)
            floors[floors.Count - 1].walls = new List<GameObject>();
        floors[floors.Count - 1].walls.Add(temporary);

        PlaceBase();
        return;
    }

    public void PlaceWall(){

        RemoveObjects();

        switch (chosenTab) {
            case 0:
                temporary = Instantiate(objectSet.SimpleWalls[selectedWall], instantiatedMarker.transform.position, instantiatedMarker.transform.rotation);
                break;
            case 1:
                temporary = Instantiate(objectSet.WindowWalls[selectedWall], instantiatedMarker.transform.position, instantiatedMarker.transform.rotation);
                break;
            case 2:
                temporary = Instantiate(objectSet.DoorWalls[selectedWall], instantiatedMarker.transform.position, instantiatedMarker.transform.rotation);
                break;       
            case 3:
                temporary = Instantiate(objectSet.Ground[selectedWall], instantiatedMarker.transform.position, instantiatedMarker.transform.rotation);
                break;   
            case 4:
                temporary = Instantiate(objectSet.Roof[selectedWall], instantiatedMarker.transform.position, instantiatedMarker.transform.rotation);
                break; 
            case 5:
                temporary = Instantiate(objectSet.OutsideDetails[selectedWall], instantiatedMarker.transform.position, instantiatedMarker.transform.rotation);
                break; 
        }
    }

    public void ManageBaseHeight(){
        for(int i=0; i<floors.Count; i++){
            floors[i].floorObject.transform.position =  new Vector3(0, baseHeight*3, 0);
        }

        for(int i=0; i<bases.Count; i++){
            bases[i].transform.localScale = new Vector3(scale, baseHeight*scale*scale, scale);
        }
    }

    public void PlaceBase(){
        if(currentFloor ==1 && temporary.layer == wallLayer){
            baseTransformPos = temporary.transform.position;
            baseTransformRot = temporary.transform.rotation;

            temporary = Instantiate(objectSet.baseObj, baseTransformPos, baseTransformRot);
            if(baseObject == null){
                baseObject = Instantiate(emptyContainer, new Vector3(0, 0, 0), Quaternion.identity);
                baseObject.transform.parent = transform;
                baseObject.name = "Buiding Bases";
            }
            temporary.transform.parent = baseObject.transform;
            bases.Add(temporary);
        }
    }
    public void RemoveObjects(){
        var objectsHit = Physics.OverlapSphere (instantiatedMarker.transform.position, 1);
        
        foreach ( Collider objectHit in objectsHit ) {
            if(objectHit.gameObject.layer == instantiatedMarker.layer && objectHit.transform.position == instantiatedMarker.transform.position && objectHit.tag != "Marker")
            DestroyImmediate(objectHit.gameObject);
        }
    }
}