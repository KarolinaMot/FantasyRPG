using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Floor{
    public Floor(int floorNum, GameObject obj){
        floorNumber = floorNum;
        floorObject = obj;
    }

    public int floorNumber = 0;
    public GameObject floorObject;
    public List<GameObject> walls;

}
public class Building : MonoBehaviour
{
    Transform lastMarkerTransform;
    public GameObject floorBase;
    public GameObject emptyWallContainer;    
    public  ItemSet objectSet; 
    public GameObject baseObject;
    
    [HideInInspector]
    public List<GameObject> bases;

[Header("Layer masks")]
    public int wallLayer;
    public int floorLayer;
    public int roofLayer;
    public int exteriorLayer;

    [HideInInspector]
    public int chosenTab;
    [HideInInspector]
    public float gridSize = 4;
    [HideInInspector]
    public int currentFloor = 1;
    [HideInInspector]
    public int selectedWall = 0;
    [HideInInspector]
    int selectionTracker = -1;
    int tabSelectionTracker = -1;
    [HideInInspector]
    public bool isBuilding = true;
    [HideInInspector]
    public float baseHeight = 0;
    [HideInInspector]
    public float baseHeightTracker = 0;
    [HideInInspector]
    public List<Floor> floors;
    GameObject instantiatedMarker = null;
    GameObject temporary;
    Vector3 baseTransformPos;
    Quaternion baseTransformRot;


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
        instantiatedMarker.transform.position = new Vector3(markerPosition.x, (currentFloor-1)*6, markerPosition.z);
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

        temporary = Instantiate(emptyWallContainer, new Vector3(0, 0, 0), Quaternion.identity);
        temporary.transform.parent = transform;
        temporary.name = "Floor"+currentFloor;

        floors.Add(new Floor(currentFloor, temporary));
        
        PlaceWall();
        temporary.transform.parent = floors[floors.Count - 1 ].floorObject.transform; 
    
        if(floors[floors.Count - 1].walls == null)
            floors[floors.Count - 1].walls = new List<GameObject>();
        floors[floors.Count - 1].walls.Add(temporary);

        PlaceBase();
        return;
    }

    public void PlaceWall(){

        var objectsHit = Physics.OverlapSphere (instantiatedMarker.transform.position, 0.02f);
        
        foreach ( Collider objectHit in objectsHit ) {
            if(objectHit.gameObject.layer == wallLayer && objectHit.transform.position == instantiatedMarker.transform.position && (chosenTab == 0 || chosenTab ==1 || chosenTab ==2) )
            DestroyImmediate(objectHit.gameObject);
            else if((objectHit.gameObject.layer == floorLayer || objectHit.gameObject.layer == roofLayer) && objectHit.transform.position == instantiatedMarker.transform.position && (chosenTab == 4 || chosenTab ==3) )
            DestroyImmediate(objectHit.gameObject);
            else if(objectHit.gameObject.layer == exteriorLayer && objectHit.transform.position == instantiatedMarker.transform.position && chosenTab == 5)
            DestroyImmediate(objectHit.gameObject);
        }

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
            floors[i].floorObject.transform.position =  new Vector3(0, (baseHeight*3), 0);
        }

        for(int i=0; i<bases.Count; i++){
            bases[i].transform.localScale = new Vector3(1, baseHeight, 1);
        }
    }

    public void PlaceBase(){
        if(currentFloor ==1 && temporary.layer == wallLayer){
            if(baseObject == null){
                baseObject = Instantiate(emptyWallContainer, new Vector3(0, 0, 0), Quaternion.identity);
                baseObject.transform.parent = transform;
                baseObject.name = "Buiding Bases";
            }
            
            baseTransformPos = temporary.transform.position;
            baseTransformRot = temporary.transform.rotation;

            temporary = Instantiate(objectSet.baseObj, baseTransformPos, baseTransformRot);
            temporary.transform.parent = baseObject.transform;
            bases.Add(temporary);
        }
    }
    public void RemoveWall(){
        var objectsHit = Physics.OverlapSphere (instantiatedMarker.transform.position, 0.02f);
        
        foreach ( Collider objectHit in objectsHit ) {
            if(objectHit.gameObject.layer == wallLayer && objectHit.transform.position == instantiatedMarker.transform.position)
            DestroyImmediate(objectHit.gameObject);
        }
    }

    public int findFloorID(){
        for(int i=0; i<floors.Count; i++){
            if(floors[i].floorNumber == currentFloor)
            return i;
        }
        return -1;
    }
}