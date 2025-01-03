using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStruct {
    //Store Runtime information about player and their status

    public int playerPortNumber;

    public float stars;
    public float coins;
    // public List<Item>() items; //Scriptable Objects?

    public int characterID; //Players are Scriptable objects Find The Reference

    public GameObject playerGameObject;

    public PlayerUIReference playerUI;
}
