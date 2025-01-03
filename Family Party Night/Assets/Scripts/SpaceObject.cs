using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpaceObject : MonoBehaviour{

    public BoardSpot spaceInfo;

    public GameObject spaceModel;
    public GameObject spaceObject;
    public List<GameObject> arrowObjects;
    public GameObject arrowModel;

    public bool enableEditorDraw;

    public Material blueMaterial;
    public Material eventMaterial;
    public Material redMaterial;
    public Material luckyMaterial;
    public Material DKBowserMaterial;
    public Material passableMaterial;

    public void OnValidate(){
        if(enableEditorDraw){

            InstantiateSpace();
            
            // EditorApplication.delayCall += () =>{
            //     if (this != null)  {// Ensure the object still exists
            //         InstantiateSpace();
            //     }
            // };

        }else{ 
            //Disable Edit Drawging
            if(spaceObject != null){
                //Destroy The object
                foreach (Transform child in this.gameObject.transform){
                    StartCoroutine(DestroyObj(child.gameObject));
                }
                spaceObject = null;
            }
        }
    }

    IEnumerator DestroyObj(GameObject go){
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        InstantiateSpace();
    }

    // Update is called once per frame
    void Update(){
        DrawObject(spaceInfo);
    }

    public void InstantiateSpace(){
        // if(spaceModel == null){
        //     Debug.Log("NO MODEL");
        //     return;
        // }
        if(spaceObject == null){
            //Object, Position, Rotation, Parent
            //This rotation makes the Piece lie flat

            // Debug.Log(this.gameObject.name);
            spaceObject = Instantiate(spaceModel, spaceInfo.position, Quaternion.Euler(-90f, 0f, 0f), this.gameObject.transform);
        }
    }

    public void DrawObject(){
        this.DrawObject(this.spaceInfo);
    }

    public void DrawObject(BoardSpot spaceInfo){
        InstantiateSpace();
        
        spaceObject.transform.position = spaceInfo.position;
        // space

        AssignMaterial();

        DrawConnections();
    }

    public void AssignMaterial(){
        Renderer rend = spaceObject.GetComponent<Renderer>();

        switch (spaceInfo.spotType){
            case SpotType.Blue:
                rend.material = blueMaterial;
                break;
            case SpotType.Event:
                rend.material = eventMaterial;
                break;
            case SpotType.Red:
                rend.material = redMaterial;
                break;
            case SpotType.Lucky:
                rend.material = luckyMaterial;
                break;
            case SpotType.DKBowser:
                rend.material = DKBowserMaterial;
                break;
            case SpotType.Passable:
                rend.material = passableMaterial;
                break;
            default:
                Debug.LogWarning("Material not assigned");
                break;
        }
    }

    public void ResetArrowsObject(){
        arrowObjects = new List<GameObject>();
        for (int i = 0; i < spaceInfo.outConnections.Count; i++){
            arrowObjects.Add(null);
        }
    }

    public void DrawConnections(){
        List<BoardSpot> nextSpots = spaceInfo.outConnections;
        
        if (arrowObjects == null || arrowObjects.Count == 0){
            ResetArrowsObject();
        }
        
        // Debug.Log(nextSpots.Count);
        // Debug.Log(arrowObjects.Count);

        for(int i = 0; i < nextSpots.Count; i++){
            // Debug.Log(i);

            if(arrowObjects[i] == null){
                GameObject arrow = Instantiate(arrowModel, this.gameObject.transform);                
                arrowObjects[i] = arrow;
            }

            if(nextSpots[i] != null){
                Vector3 midpointPosition = (spaceInfo.position + nextSpots[i].position) / 2.0f;
                arrowObjects[i].transform.position = midpointPosition;

                // Calculate the direction from object1 to object2
                Vector3 direction = nextSpots[i].position - spaceInfo.position;
                // Get the rotation that looks in the direction
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                arrowObjects[i].transform.rotation = lookRotation;

                float distance = Vector3.Distance(nextSpots[i].position, spaceInfo.position);
                arrowObjects[i].transform.localScale = new Vector3(1,1, distance / (3.5f));
            }
        }
    }
}
