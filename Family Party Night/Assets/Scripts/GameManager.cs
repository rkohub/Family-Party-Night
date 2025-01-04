using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Collections;

using TMPro;

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

    public GameObject characterImageObject;

    public List<Sprite> placeSprites;
    public GameObject placeImageObject;

    public GameObject coinsImageObject;
    public GameObject coinsTextObject;
    public GameObject starsImageObject;
    public GameObject starsTextObject;

    public int playerTurn;

    public float bannerScale;

    public Vector3 bannerStartVector;
    public Vector3 bannerDifference;

    private Vector3 characterIconStartVector;
    private Vector3 placeStartVector;
    private Vector3 starStartVector;
    private Vector3 starTextStartVector;
    private Vector3 coinStartVector;
    private Vector3 coinTextStartVector;

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


        //Default Test players
        PlayerStruct p1 = new PlayerStruct();
        p1.playerPortNumber = 0; p1.characterID = 0; 

        PlayerStruct p2 = new PlayerStruct();
        p2.playerPortNumber = 1; p2.characterID = 1;

        playerInfo.Add(p1); playerInfo.Add(p2); //playerInfo = p1.allPlayers;  

        for(int i = 0; i < playerInfo.Count; i++){
            GameObject uiEmpty = Instantiate(playerUIEmpty, UIParent.gameObject.transform);
            playerUIEmpties.Add(uiEmpty);

            playerUIReferences.Add(new PlayerUIReference());

            GameObject banner = Instantiate(playerBannerObject, playerUIEmpties[i].gameObject.transform);
            banner.GetComponent<Image>().sprite = playerBannerSprites[playerInfo[i].playerPortNumber];
            playerUIReferences[i].playerBannerObject = banner;

            GameObject characterImage = Instantiate(characterImageObject, playerUIEmpties[i].gameObject.transform);
            characterImage.GetComponent<Image>().sprite = allCharacters[playerInfo[i].characterID].uiImage;
            playerUIReferences[i].playerCharacterImageObject = characterImage;

            GameObject placeImage = Instantiate(placeImageObject, playerUIEmpties[i].gameObject.transform);
            // placeImage.GetComponent<Image>().sprite = placeSprites[playerInfo[i].getPlace()];
            playerUIReferences[i].playerPlaceObject = placeImage;

            GameObject stars = Instantiate(starsImageObject, playerUIEmpties[i].gameObject.transform);
            playerUIReferences[i].playerStarImageObject = stars;

            GameObject coins = Instantiate(coinsImageObject, playerUIEmpties[i].gameObject.transform);
            playerUIReferences[i].playerCoinImageObject = coins;

            GameObject starsText = Instantiate(starsTextObject, playerUIEmpties[i].gameObject.transform);
            playerUIReferences[i].playerStarTextObject = starsText;

            GameObject coinsText = Instantiate(coinsTextObject, playerUIEmpties[i].gameObject.transform);
            playerUIReferences[i].playerCoinTextObject = coinsText;
            
            GameObject playerObject = Instantiate(allCharacters[playerInfo[i].characterID].playerModel, this.gameObject.transform);
            playerObject.GetComponent<PlayerController>().myPlayerInfo = playerInfo[i];
            playerInfo[i].playerGameObject = playerObject;

            
            playerInfo[i].playerUI = playerUIReferences[i];
        }
    
        for(int i = 0; i < playerInfo.Count; i++){
            if(i == 0){
                playerInfo[i].setStars(1); playerInfo[i].setCoins(10);
            }else if(i == 1){
                playerInfo[i].setStars(1); playerInfo[i].setCoins(10);
            }
        }
        // p1.setStars(0); p1.setCoins(23);
        // p2.setStars(1); p2.setCoins(10); //Do THis After so the Displays Update

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
        checkAndUpdatePlayerUI();
        
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

        //Move Each Empty by the size of the banner, Moves all Chile Elements.
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

    public void checkAndUpdatePlayerUI(){
        bool updateUI = false;
        for(int i = 0; i < playerInfo.Count; i++){
            if(playerInfo[i].updateUI){
                updateUI = true;
            }
        } 
        if(updateUI){
            for(int i = 0; i < playerInfo.Count; i++){
                updatePlayerUI(i);
                playerInfo[i].updateUI = false;
            }
        }
    }

    public void updatePlayerUI(int playerIndex){
        PlayerStruct player = playerInfo[playerIndex];
        TMP_Text starsText = player.playerUI.playerStarTextObject.GetComponent<TMP_Text>();
        starsText.text = "" + player.getStars();
        TMP_Text coinsText = player.playerUI.playerCoinTextObject.GetComponent<TMP_Text>();
        coinsText.text = "x" + player.getCoins();

        player.playerUI.playerPlaceObject.GetComponent<Image>().sprite = placeSprites[player.getPlace()];
    }
}
