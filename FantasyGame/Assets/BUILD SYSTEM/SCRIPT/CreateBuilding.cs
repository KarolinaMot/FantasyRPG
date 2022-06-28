using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateBuilding : MonoBehaviour
{
    public GameObject buildingEmptyObj;

    public GameObject floorBase;

    GameObject building = null;
    GameObject floor = null;

    public void GenerateNewBuilding(string name)
    {
        building = Instantiate(buildingEmptyObj, transform);
        building.name = name;
        floor = Instantiate(floorBase, transform);
        floor.transform.parent = building.transform;


        GameObject[] gos = {building};
        Selection.objects = gos;
    }

}
