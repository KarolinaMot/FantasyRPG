using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildingWindow : EditorWindow
{   
   public Texture2D[] textures;
 
   public int selectedWall = 0;
   public int currentFloor = 1;
   public int gridSize = 4;
   public bool isBuilding = true; 
   public float baseHeight = 0;
   int buildingSetting;
   public ItemSet items;
   ItemSet previousItems = null;
    List<Texture2D> doors;
    List<Texture2D> windows;
    List<Texture2D> walls;
    List<Texture2D> floors;
    List<Texture2D> roofs;
    List<Texture2D> exterior;
        [HideInInspector]

   public int openTab = 0;
   int temp = 0;

   void OnGUI()
    {   
        GUILayout.Space(30);
        GUI.Label(new Rect(5, 10, 130, 20), "Build Objects Set", EditorStyles.boldLabel);
        items = (ItemSet)EditorGUILayout.ObjectField(items, typeof(ItemSet), true);

        if(items!= null && (previousItems == null || previousItems != items)){
            doors = new List<Texture2D>();
            windows = new List<Texture2D>();
            walls = new List<Texture2D>();
            floors = new List<Texture2D>();
            roofs = new List<Texture2D>();
            exterior = new List<Texture2D>();

                        
            for(int i=0; i<items.DoorWalls.Count; i++){
                doors.Add(AssetPreview.GetAssetPreview(items.DoorWalls[i]));
            }
            for(int i=0; i<items.WindowWalls.Count; i++){
                windows.Add(AssetPreview.GetAssetPreview(items.WindowWalls[i]));
            }
            for(int i=0; i<items.SimpleWalls.Count; i++){
                walls.Add(AssetPreview.GetAssetPreview(items.SimpleWalls[i]));
            }
            for(int i=0; i<items.Ground.Count; i++){
                floors.Add(AssetPreview.GetAssetPreview(items.Ground[i]));
            }
            for(int i=0; i<items.Roof.Count; i++){
                roofs.Add(AssetPreview.GetAssetPreview(items.Roof[i]));
            }
            for(int i=0; i<items.OutsideDetails.Count; i++){
                exterior.Add(AssetPreview.GetAssetPreview(items.OutsideDetails[i]));
            }
            previousItems = items;
        }

        GUILayout.Space(20);
        GUI.Label(new Rect(5, 55, 130, 20), "Building walls", EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box");
        buildingSetting = GUILayout.SelectionGrid(buildingSetting, new string[] {"Build", "Remove"}, 2);

        if(buildingSetting == 0)
            isBuilding = true;
        else
            isBuilding=false;

        GUILayout.EndVertical();

        GUILayout.Space(20);
        GUI.Label(new Rect(5, 105, 130, 20), "Pacement precision", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal("box");
            gridSize = EditorGUILayout.IntField("Grid size", gridSize); 
            if(GUILayout.Button("<") && gridSize != 1)
                gridSize--;
            if(GUILayout.Button(">"))
                gridSize++;
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUI.Label(new Rect(5, 150, 130, 20), "Floor", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal("box");
            temp = EditorGUILayout.IntField("Current floor", currentFloor); 
            currentFloor = temp;
            if(GUILayout.Button("<") && currentFloor != 1)
                currentFloor--;
            if(GUILayout.Button(">"))
                currentFloor++;
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUI.Label(new Rect(5, 210, 130, 20), "Base height:", EditorStyles.boldLabel);

        baseHeight = EditorGUI.Slider(new Rect(5, 230, 290, 20), baseHeight, 0, 2);

        GUILayout.Space(30);
        GUI.Label(new Rect(5, 260, 130, 20), "Select wall", EditorStyles.boldLabel);
        GUILayout.Space(20);


        GUILayout.BeginVertical("Box");
        openTab = GUILayout.Toolbar (openTab, new string[] {"PlainWalls", "Windows", "Doors", "Floors", "Roof", "Exterior"});
        switch (openTab) {
            case 0:
                selectedWall = GUILayout.SelectionGrid(selectedWall, walls.ToArray(), 2);
                break;
            case 1:
                selectedWall = GUILayout.SelectionGrid(selectedWall, windows.ToArray(), 2);
                break;
            case 2:
                selectedWall = GUILayout.SelectionGrid(selectedWall, doors.ToArray(), 2);
                break;       
            case 3:
                selectedWall = GUILayout.SelectionGrid(selectedWall, floors.ToArray(), 2);
                break; 
            case 4:
                selectedWall = GUILayout.SelectionGrid(selectedWall, roofs.ToArray(), 2);
                break;
            case 5:
                selectedWall = GUILayout.SelectionGrid(selectedWall, exterior.ToArray(), 2);
                break;
        }
        GUILayout.EndVertical();




        //building.CheckForGridRedraw();
    }


}
