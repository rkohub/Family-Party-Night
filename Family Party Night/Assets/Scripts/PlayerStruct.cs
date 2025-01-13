using UnityEngine;

// using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStruct {
    //Store Runtime information about player and their status

    public int playerPortNumber;

    [SerializeField] private float stars;
    [SerializeField] private float coins;

    [SerializeField] private int place;
    public int characterID;
    public GameObject playerGameObject;
    public PlayerUIReference playerUI;

    public bool updateUI;

    public PlayerController playerController;
    private static List<PlayerStruct> allPlayers = new List<PlayerStruct>();

    public PlayerStruct(){
        allPlayers.Add(this);
    }

    public void loadPlayerController(){
        this.playerController = playerGameObject.GetComponent<PlayerController>();
    }

    public float getStars(){
        return stars;
    }
    public void setStars(float value){
        stars = Mathf.Max(0, value);  // Prevents negative stars
        updateAllPlacing();
        updateUI = true;// updateMyDisplay();
    }

    public float getCoins(){
        return coins;
    }
    public void setCoins(float value){
        coins = Mathf.Max(0, value);  // Prevents negative coins
        updateAllPlacing();
        updateUI = true;// updateMyDisplay();
    }

    public void addCoins(float coins){
        this.setCoins(this.getCoins() + coins);
    }

    public void addStars(float stars){
        this.setStars(this.getStars() + stars);
    }

    public void subtractCoins(float coins){
        this.setCoins(this.getCoins() - coins);
    }

    public void subtractStars(float stars){
        this.setStars(this.getStars() - stars);
    }



    public int getPlace(){
        return place;
    }

    // public void setPlace(int value){
    //     place = Mathf.Max(1, value);  // Ensures place is at least 1
    // }

    public int CompareTo(PlayerStruct b){
        //Sort Based on what Comparision between a and B.
        if (b.stars.CompareTo(this.stars) != 0){
            return b.stars.CompareTo(this.stars);  // Descending by stars
        } //else
        return b.coins.CompareTo(this.coins);  // Descending by coins
    }

    private void updateAllPlacing() {
        allPlayers.Sort((a, b) => { 
            //Sort Based on what Comparision between a and B.
            if (b.stars.CompareTo(a.stars) != 0){
                return b.stars.CompareTo(a.stars);  // Descending by stars
            } //else
            return b.coins.CompareTo(a.coins);  // Descending by coins
        }); //a.CompareTo(b);

        // Assign new places
        for (int i = 0; i < allPlayers.Count; i++){
            allPlayers[i].place = i;// + 1;  // First place is 0, second is 1, etc.
            if(i != 0){
                if(allPlayers[i-1].CompareTo(allPlayers[i]) == 0){
                    allPlayers[i].place = allPlayers[i-1].place;
                }
            }

            // allPlayers[i].playerUI.GetComponent<Image>().sprite = placeSprites[playerInfo[i].getPlace()];
        }
    }

    // private void updateMyDisplay() {
    //     //This objects coins and stars are changing.
    //     TMP_Text starsText = playerUI.playerStarTextObject.GetComponent<TMP_Text>();
    //     starsText.text = "" + this.getStars();
    //     TMP_Text coinsText = playerUI.playerCoinTextObject.GetComponent<TMP_Text>();
    //     coinsText.text = "x" + this.getCoins();
    // }


    // public List<Item>() items; //Scriptable Objects?
}
