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
        /*BuildingInfo asset = ScriptableObject.CreateInstance<BuildingInfo>();
        asset.emptyContainer = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/BUILD SYSTEM/PREFABS/TOOL ASSETS/EmptyWall.prefab", typeof(GameObject));
        asset.objectSet = (ItemSet)AssetDatabase.LoadAssetAtPath("Assets/BUILD SYSTEM/Base Items.asset", typeof(ItemSet));
        asset.scale = 1.2f;
        asset.wallLayer = 3;
        asset.floorLayer = 15;
        asset.roofLayer = 18;
        asset.exteriorLayer = 16;

        AssetDatabase.CreateAsset(asset, "Assets/BUILDINGS/BUILDING MENUS/"+building.name+".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;*/

        GameObject[] gos = {building};
        Selection.objects = gos;
    }

}
