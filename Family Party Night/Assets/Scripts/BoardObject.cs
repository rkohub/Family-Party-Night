using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoardObject : MonoBehaviour{

    public Board boardInfo;

    public bool enableEditorDraw;

    public List<GameObject> spaceObjects;
    public GameObject spaceObjectModel;



    public void OnValidate(){
        if(enableEditorDraw){

            InstantiateSpaces();

        }else{ 
            //Disable Edit Drawging
            if(spaceObjects != null){
                //Destroy All Kids
                foreach (Transform child in this.gameObject.transform){
                    StartCoroutine(DestroyObj(child.gameObject));
                }
                spaceObjects = null;
            }
        }
    }

    IEnumerator DestroyObj(GameObject go){
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    public void ResetSpaceObjects(){
        spaceObjects = new List<GameObject>();
        for (int i = 0; i < boardInfo.spaces.Count; i++){
            spaceObjects.Add(null);
        }
    }

    public void InstantiateSpaces(){
        List<BoardSpot> boardSpots = boardInfo.spaces;
        
        if (spaceObjects == null || spaceObjects.Count != boardInfo.spaces.Count){
            ResetSpaceObjects();
        }

        for(int i = 0; i < boardInfo.spaces.Count; i++){
            if(spaceObjects[i] == null){
                GameObject spaceObject = Instantiate(spaceObjectModel, this.gameObject.transform);
                spaceObject.GetComponent<SpaceObject>().spaceInfo = boardInfo.spaces[i];
                spaceObjects[i] = spaceObject;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        InstantiateSpaces();
    }

    // Update is called once per frame
    void Update(){
        
    }

}
