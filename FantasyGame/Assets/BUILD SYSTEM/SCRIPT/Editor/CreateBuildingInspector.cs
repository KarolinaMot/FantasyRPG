using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CreateBuilding))]
public class CreateBuildingInspector : Editor
{
    string objName = "BuildingName";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FirstInspectorSection();
    }

    void FirstInspectorSection()
    {
        GUILayout.Space(10);

        Rect rect = EditorGUILayout.GetControlRect(false, 2);
        rect.height = 2;

        GUILayout.Space(60);

        CreateBuilding houseBuilding = (CreateBuilding)target;
        GUI.Label(new Rect(17, 65, 130, 20), "Create New Building!", EditorStyles.boldLabel);
        GUI.Label(new Rect(17, 0, 130, 200), "Building name:");
        objName = GUI.TextField(new Rect(140, 92, 130, 20), objName, 80);

        if (GUILayout.Button("Generate new building"))
        {
            houseBuilding.GenerateNewBuilding(objName);
        }
    }

}
