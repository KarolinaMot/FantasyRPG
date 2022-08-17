using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemSet items;
    public MaterialSelection baseMaterials;
    GameObject temp;

    public void ReplaceWallMesh(int selectedWall){
        temp = (GameObject)PrefabUtility.InstantiatePrefab(items.SimpleWalls[selectedWall]);
        temp.transform.parent = transform.parent;
        temp.transform.position = transform.position;
        temp.transform.rotation = transform.rotation;
        temp.transform.localScale = transform.localScale;
        DestroyImmediate(transform.gameObject);

    }

    public void ReplaceWallMaterial(int selectedMaterial){
        foreach(Transform child in transform){
            if(child.name.Contains("BASE"))
            {
                child.GetComponent<MeshRenderer>().material = baseMaterials.materials[selectedMaterial];
                return;
            }
        }
    }

    
}
