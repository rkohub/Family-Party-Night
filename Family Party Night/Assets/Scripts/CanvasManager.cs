using UnityEngine;

public class CanvasManager : MonoBehaviour{

    public static CanvasManager Instance { get; private set; }
    public Canvas mainCanvas;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Persist between scenes if necessary
    }
}