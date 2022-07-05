using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Building))]
public class BuildingInspector :  Editor
{
    BuildingWindow buildingSettings;
    
    void OnSceneGUI()
    {
        Building building = (Building)target;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            
            buildingSettings = (BuildingWindow)EditorWindow.GetWindow( typeof(BuildingWindow), false, "Building Settings" );

            buildingSettings.Show();
            building.selectedWall = buildingSettings.selectedWall;
            building.currentFloor =  buildingSettings.currentFloor;
            building.gridSize = buildingSettings.gridSize;
            building.isBuilding = buildingSettings.isBuilding;
            building.chosenTab = buildingSettings.openTab;
            building.baseHeight = buildingSettings.baseHeight;

            if(building.baseHeight != building.baseHeightTracker &&(building.baseHeightTracker != 0 || building.baseHeight !=0)){
                building.ManageBaseHeight();
                building.baseHeightTracker = building.baseHeight;
            }

            if(buildingSettings.items == null)
                buildingSettings.items = building.objectSet;
            
            building.objectSet  = buildingSettings.items;

            Event e = Event.current;
            switch (e.type) {
                case EventType.KeyDown:
                    if(e.keyCode == KeyCode.D)
                        building.ManageRotation(45);
                    else if(e.keyCode == KeyCode.A)
                        building.ManageRotation(-45);
                    else if(e.keyCode == KeyCode.W)
                        buildingSettings.currentFloor++;
                    else if(e.keyCode == KeyCode.S && buildingSettings.currentFloor !=1)
                        buildingSettings.currentFloor--;
                break;
                case EventType.MouseDown:
                if(Event.current.button == 0){
                        if(building.isBuilding)
                            building.BuildingManager();
                        else{
                            building.RemoveObjects();
                        }
                }
                else if(Event.current.button == 1)
                    building.ManageRotation(45);
                break;
            }
            ManageMarkerMovement(building);
        
    }

    #region  Marker Manager
    void ManageMarkerMovement(Building houseBuilding)
    {
        houseBuilding.MoveMarker(snapPosition(getWorldPoint(), houseBuilding));
    }

    public Vector3 getWorldPoint()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 worldPoint = new Vector3(hit.point.x, 0, hit.point.z);
            return worldPoint;
        }

        return Vector3.zero;
    }

    public Vector3 snapPosition(Vector3 original, Building building)
    {
        Vector3 snapped;
        snapped.x = Mathf.Round(Mathf.Round(original.x + 0.5f)/building.gridSize) * building.gridSize * building.scale;
        snapped.y = 0;
        snapped.z = Mathf.Round(Mathf.Round(original.z + 0.5f) /building.gridSize) * building.gridSize * building.scale;

        return snapped;
    }
    #endregion
}
