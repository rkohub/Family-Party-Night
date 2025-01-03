using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BoardSpot", menuName = "Scriptable Objects/BoardSpot")]
public class BoardSpot : ScriptableObject {
    public int id;
    public Vector3 position;
    public SpotType spotType;
    public List<BoardSpot> outConnections;
    public bool updateVisual;

    public void OnValidate(){
        // Debug.Log("ScriptableObject data updated!");
        
        SpaceObject[] foundBoardSpots = FindObjectsByType<SpaceObject>(FindObjectsSortMode.None);
        // Debug.Log(foundBoardSpots + " : " + foundBoardSpots.Length);

        foreach (SpaceObject space in foundBoardSpots){
            if (space.spaceInfo.id == this.id && space.enableEditorDraw){
                // Debug.Log($"Equal: {space.spaceInfo.id} == {this.id}");
                space.DrawObject();
            }else{
                // Debug.Log($"Not Equal: {space.spaceInfo.id} != {this.id}");

                //!!!TODO (Remove and Fix ID's)
                space.DrawObject();
            }
        }
    }
}


