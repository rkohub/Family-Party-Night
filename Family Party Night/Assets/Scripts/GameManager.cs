using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Collections;


public class GameManager : MonoBehaviour {
    //Where I manage anything that has todo with all players

    public static GameManager Instance { get; private set; }

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Persist between scenes if necessary
    }

    public GameObject playerUIEmpty;
    public List<GameObject> playerUIEmpties;

    public List<PlayerUIReference> playerUIReferences;

    public GameObject playerBannerObject;
    public List<Sprite> playerBannerSprites; //Blue, Red, Green, Yellow
    public GameObject UIParent;

    public int playerTurn;

    public float bannerScale;

    public Vector3 bannerStartVector;
    public Vector3 bannerDifference;

    public Vector3 characterIconStartVector;
    public Vector3 placeStartVector;
    public Vector3 starStartVector;
    public Vector3 starTextStartVector;
    public Vector3 coinStartVector;
    public Vector3 coinTextStartVector;

    public List<PlayerStruct> playerInfo;
    
    public List<CharacterObject> allCharacters;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        playerTurn = 0;


        bannerScale = 0.5f;
        bannerDifference    = new Vector3 (0f, -105f, 0f); //238/2 - 10 (For Borders and 0.5 Scale)
        bannerStartVector = new Vector3 (-840f, 465f, 0f);

        characterIconStartVector= new Vector3 (-900f, 465f, 0f);
        placeStartVector        = new Vector3 (-900f, 505f, 0f);
        starStartVector         = new Vector3 (-825f, 490f, 0f);
        starTextStartVector     = new Vector3 (-760f, 502f, 0f);
        coinStartVector         = new Vector3 (-825f, 440f, 0f);
        coinTextStartVector     = new Vector3 (-760f, 455f, 0f);

        playerUIEmpties = new List<GameObject>();
        playerUIReferences = new List<PlayerUIReference>();

        for(int i = 0; i < playerInfo.Count; i++){
            GameObject uiEmpty = Instantiate(playerUIEmpty, UIParent.gameObject.transform);
            playerUIEmpties.Add(uiEmpty);

            playerUIReferences.Add(new PlayerUIReference());

            GameObject banner = Instantiate(playerBannerObject, playerUIEmpties[i].gameObject.transform);
            banner.GetComponent<Image>().sprite = playerBannerSprites[playerInfo[i].playerPortNumber];
            playerUIReferences[i].playerBanner = banner;
            
            playerInfo[i].playerGameObject = Instantiate(allCharacters[playerInfo[i].characterID].playerModel, this.gameObject.transform);
        }
        PositionObjectsOnTurn();

        StartCoroutine(LateStart()); //Late Starting to change a Value on a generated Object
    }

    IEnumerator LateStart(){
        // Wait until the end of the frame to ensure all Start methods run first
        yield return new WaitForEndOfFrame();

        // Debug.Log("LateStart called.");
        // print(playerInfo[playerTurn].playerGameObject);
        playerInfo[playerTurn].playerGameObject.GetComponent<PlayerController>().StartTurn();
    }



    // Update is called once per frame
    void Update() {
        
    }

    public void positionUIObject(GameObject uiObject, int index){

        /*
        Vector3 bannerScaleVector =  new Vector3(bannerScale, bannerScale, bannerScale);
        // print(bannerScaleVector);

        bannerObject.transform.localScale = bannerScaleVector;    

        //*/
        RectTransform rectTransform = uiObject.GetComponent<RectTransform>();

        // // Set anchor point to top left
        // rectTransform.anchorMin = new Vector2(0, 1);
        // rectTransform.anchorMax = new Vector2(0, 1);

        rectTransform.localPosition = (index * bannerDifference);//bannerStartVector + (index * bannerDifference);
    }

    public void PositionObjectsOnTurn(){
        for(int i = 0; i < playerInfo.Count; i++){
            GameObject empty = playerUIEmpties[i];//playerUIReferences[i].playerBanner;
            positionUIObject(empty, i);
        }
    }

    public void IncrementTurn(){
        //Sent Here By End Turn. Call Start Turn on New Player
        playerTurn = (playerTurn + 1) % playerInfo.Count;

        playerInfo[playerTurn].playerGameObject.GetComponent<PlayerController>().StartTurn(); 
    }
}
