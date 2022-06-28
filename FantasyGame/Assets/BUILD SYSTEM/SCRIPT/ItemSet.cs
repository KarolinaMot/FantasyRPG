using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemSet")]
public class ItemSet : ScriptableObject
{
    public GameObject baseObj;

    [Header("Solid objects")]
    public List<GameObject> SimpleWalls;
    public List<GameObject> WindowWalls;
    public List<GameObject> DoorWalls;
    public List<GameObject> Ground;
    public List<GameObject> Roof;
    public List<GameObject> OutsideDetails;



    [Header("Transparent objects")]

    public List<GameObject> SimpleWallsT;
    public List<GameObject> WindowWallsT;
    public List<GameObject> DoorWallsT;
    public List<GameObject> GroundT;
    public List<GameObject> RoofT;
    public List<GameObject> OutsideDetailsT;


}
