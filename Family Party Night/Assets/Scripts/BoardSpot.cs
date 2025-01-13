using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BoardSpot", menuName = "Scriptable Objects/BoardSpot")]
public class BoardSpot : ScriptableObject {
    public int id;
    public Vector3 position;
    public SpotType spotType;
    public List<BoardSpot> outConnections;
    public bool updateVisual;

    public System.Action OnSpotLand;
    public System.Action OnSpotEnter;

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

    //On Spot Enter
        //Prompt Buy Star
        //Prompt Which Way to go. 
    //On Spot Leave
    //On Spot land
        //Increase Coins
        //Trigger Events

    //Can Change from Passable to not
    //Tree of possible SpotTypes?
    //Star Can Move, Blue Space when not Star

    //Spot Type, StarSponSpot, StarLocation

    //Extendable Board Manager Class?
}


