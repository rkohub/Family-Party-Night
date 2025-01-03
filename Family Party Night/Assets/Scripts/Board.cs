using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Board", menuName = "Scriptable Objects/Board")]
public class Board : ScriptableObject {
    public List<BoardSpot> spaces;    
}
