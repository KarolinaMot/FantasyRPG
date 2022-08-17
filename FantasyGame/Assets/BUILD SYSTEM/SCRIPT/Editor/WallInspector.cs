using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Wall))]
public class WallInspector : Editor
{
    List<Texture2D> wallThumbnails;
    List<Texture2D> materialThumbnails;

    int selectedWall = 0;
    int selectedMaterial = 0;
    public override void OnInspectorGUI(){

        base.OnInspectorGUI();
        Wall wall = (Wall)target;

        if(wall.items ==null || wall.baseMaterials == null)
            return;

        GUILayout.Space(20);
        GUILayout.Label("REPLACE WALL");

        if(wallThumbnails == null){
            wallThumbnails = new List<Texture2D>();                    
            for(int i=0; i<wall.items.SimpleWalls.Count; i++){
               wallThumbnails.Add(AssetPreview.GetAssetPreview(wall.items.SimpleWalls[i]));
            }
        }

        selectedWall = GUILayout.SelectionGrid(selectedWall, wallThumbnails.ToArray(), 2);
        
        GUILayout.Space(5);
        if(GUILayout.Button("Replace")){
            wall.ReplaceWallMesh(selectedWall);
        }


        GUILayout.Space(20);
        GUILayout.Label("REPLACE BASE MATERIAL");
        if(materialThumbnails == null){
            materialThumbnails = new List<Texture2D>();                    
            for(int i=0; i<wall.baseMaterials.materials.Count; i++){
                materialThumbnails.Add(AssetPreview.GetAssetPreview(wall.baseMaterials.materials[i]));
            }
        }
        selectedMaterial = GUILayout.SelectionGrid(selectedMaterial, materialThumbnails.ToArray(), 2);
        GUILayout.Space(5);
        if(GUILayout.Button("Replace")){
            wall.ReplaceWallMaterial(selectedMaterial);
        }
    }
}
