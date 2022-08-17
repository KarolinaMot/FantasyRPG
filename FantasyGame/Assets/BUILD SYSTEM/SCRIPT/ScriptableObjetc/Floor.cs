using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ScriptableObject{

    public void Init(int floorNum, GameObject obj){
        floorNumber = floorNum;
        floorObject = obj;
    }

    public int floorNumber = 0;
    public GameObject floorObject;
    public List<GameObject> walls;

}
