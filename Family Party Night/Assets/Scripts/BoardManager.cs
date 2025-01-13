using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoardManager : MonoBehaviour{

    public static BoardManager Instance { get; private set; }

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Persist between scenes if necessary
    }

    public Board boardInfo;
    public GameManager gm;

    public bool enableEditorDraw;

    public List<GameObject> spaceObjects;
    public GameObject spaceObjectModel;

    [SerializeField] private int currentStarSpotIndex;

    public GameObject starObject;
    public float starCost;

    public int getCurrentStarSpotIndex(){
        return currentStarSpotIndex;
    }

    public void setCurrentStarSpotIndex(int index){
        //Remove Occurance of Star

        GameObject starSpotObject;
        int oldStarIndex = currentStarSpotIndex;

        if(oldStarIndex != -1){
            //Not Assigned Yet
            starSpotObject = spaceObjects[oldStarIndex];

            GameObject oldStar = starSpotObject.transform.Find("Star(Clone)").gameObject;
            if(oldStar != null){
                Destroy(oldStar);
            }

            AssignSpotActions(oldStarIndex, null, null);
        }
       


        //Index Checking
        if(index < 0 || index > boardInfo.spaces.Count){
            Debug.LogWarning("Index Not in range");
        }
        currentStarSpotIndex = index;

        starSpotObject = spaceObjects[currentStarSpotIndex];
        Vector3 spacePosition = boardInfo.spaces[currentStarSpotIndex].position;

        Vector3 starCoordinates = new Vector3(spacePosition.x, spacePosition.y + 5, spacePosition.z);
        Instantiate(starObject, starCoordinates, Quaternion.Euler(0f,0f,0f), starSpotObject.transform);

        AssignSpotActions(currentStarSpotIndex, null, onPassStar);
    }


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
                BoardSpot bs = boardInfo.spaces[i];
                bs.id = i;
                bs.OnSpotLand = () => Debug.Log($"Landed on spot NUMBER");
                bs.OnSpotEnter = () => Debug.Log($"Entered spot NUMBER");

                GameObject spaceObject = Instantiate(spaceObjectModel, this.gameObject.transform);
                spaceObject.GetComponent<SpaceObject>().spaceInfo = boardInfo.spaces[i];
                spaceObjects[i] = spaceObject;
            }
        }
    }

    public void AssignSpotActions(int index, System.Action onLand, System.Action onEnter = null) {
        List<int> indeces = new List<int>();
        indeces.Add(index);
        AssignSpotActions(indeces, onLand, onEnter);
    }

    //Assign Same onLand and onEnter actions to indeces of spaces.
    public void AssignSpotActions(List<int> indices, System.Action onLand, System.Action onEnter = null) {
        foreach (int index in indices) {
            if (index < 0 || index >= boardInfo.spaces.Count) continue;//Invald Index Check

            boardInfo.spaces[index].OnSpotLand = onLand;
            if (onEnter != null) {
                boardInfo.spaces[index].OnSpotEnter = onEnter;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if (GameManager.Instance != null){
            gm = GameManager.Instance;
        }else{
            Debug.LogWarning("GameManager not found!");
        }

        InstantiateSpaces();

        List<int> blueIndeces = new List<int>();
        List<int> redIndeces = new List<int>();
        for (int i = 0; i < boardInfo.spaces.Count; i++){
            BoardSpot bs = boardInfo.spaces[i];
            if (bs.spotType == SpotType.Blue){
                blueIndeces.Add(i); 
            } else if(bs.spotType == SpotType.Red){
                redIndeces.Add(i);
            }
        }

        AssignSpotActions(blueIndeces, OnLandBlue);
        AssignSpotActions(redIndeces, OnLandRed);

        currentStarSpotIndex = -1;
        chooseNewStarSpot();


    }

    // Update is called once per frame
    void Update(){
        
    }

    public void chooseNewStarSpot(){
        List<int> starSpotCanidateIndexes = new List<int>();
        for (int i = 0; i < boardInfo.spaces.Count; i++){
            BoardSpot bs = boardInfo.spaces[i];
            if (bs.spotType == SpotType.Passable){
                starSpotCanidateIndexes.Add(i);
            }
        } 
        int nextStarSpotIndex = starSpotCanidateIndexes[Random.Range(0, starSpotCanidateIndexes.Count)];// Upper bound is exclusive
        while(currentStarSpotIndex == nextStarSpotIndex){
            nextStarSpotIndex = starSpotCanidateIndexes[Random.Range(0, starSpotCanidateIndexes.Count)];// Upper bound is exclusive
        }
        setCurrentStarSpotIndex(nextStarSpotIndex);
    }

    //Board
    public void OnLandRed(){
        AddCoins(-3f);
    } 

    public void OnLandBlue(){
        AddCoins(3f);
    }

    public void AddCoins(float coins){//, int playerIndex){
        PlayerStruct currentPlayer = gm.playerInfo[gm.playerTurn];
        currentPlayer.addCoins(coins);
    }

    public void onPassStar(){
        PlayerStruct currentPlayer = gm.playerInfo[gm.playerTurn];
        if(currentPlayer.getCoins() >= starCost){
            currentPlayer.subtractCoins(starCost);//Not Working.
            currentPlayer.addStars(1);
            chooseNewStarSpot();
        }
    }

}
