using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Scriptable Objects/CharacterObject")]
public class CharacterObject : ScriptableObject {
   //Store information like Sprite and Model of Characters Like Yoshi
   public GameObject playerModel;
   public Sprite uiImage;
 
}
