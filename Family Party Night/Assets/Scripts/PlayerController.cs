using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour{

    public BoardSpot startSpace;
    public BoardSpot currentSpace;

    // public GameObject playerObject;

    public Canvas globalCanvas;

    public TMP_Text diceTextObject;
    public int diceNumber;

    public float timeToChangeNumber;
    public float elapsedTime;
    public bool spinning;

    public DiceRoller dr;

    public float timeBeforeMove;
    public float timeToMove;

    public bool myTurn;

    public GameManager gm;

    public GameObject myCamera;

    //Run before Start
    void Awake(){
        myTurn = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
    
        //Safe to search for singleton instance because all awake runs before Start
        if (GameManager.Instance != null){
            gm = GameManager.Instance;
        }else{
            Debug.LogWarning("GameManager not found!");
        }

        timeToMove = 0.4f;
        timeBeforeMove = timeToMove; 

        if (CanvasManager.Instance != null){
            globalCanvas = CanvasManager.Instance.mainCanvas;
            // Modify canvas elements here
            // Debug.Log("Canvas found and modified");
        }else{
            Debug.LogWarning("CanvasManager not found!");
        }

        //Path To Text. Canvas/TurnsUI/Rolled_Number
        Transform target = globalCanvas.transform.Find("TurnsUI/Rolled_Number");
        if (target != null){
            //Safe to Search for object in start beacuse it isnt generated
            diceTextObject = target.GetComponent<TMP_Text>();
            if (diceTextObject == null){
                Debug.LogWarning("Dice Text not found!");
            }
        }

        currentSpace = startSpace;
        moveAboveSpace();
        
        //Finds a child by name "[n]" and returns it.
        myCamera = this.gameObject.transform.Find("Main Camera").gameObject;
        SetCameraActive(false);

        // playerObject = GetComponentInChildren<CapsuleCollider>().gameObject;
    }

    public void StartTurn() {
        SetCameraActive(true);

        diceNumber = 1;
        changeNumber();

        spinning = true;
        myTurn = true;
    }

    public void EndTurn() {
        SetCameraActive(false);

        // spinning = false;
        dr.StopRolling();
        myTurn = false;

        gm.IncrementTurn();
        //Message Controller
    }

    // Update is called once per frame
    void Update(){
        if(myTurn){
        
            if (Input.GetKeyDown(KeyCode.N)){
                currentSpace = currentSpace.outConnections[0];
                moveAboveSpace();
            }
            
            if (Input.GetKeyDown(KeyCode.J)){
                if(spinning){
                    spinning = false;
                    dr.StopRolling();
                }else{
                    spinning = true;
                    dr.StartRolling();
                }
            }

            if(spinning){
                elapsedTime += Time.deltaTime;
                if(elapsedTime > timeToChangeNumber){
                    elapsedTime = 0;
                    changeNumber();
                }
            }else{
                //Moving
                if(diceNumber > 0 && timeBeforeMove <= 0){
                    currentSpace = currentSpace.outConnections[0];
                    moveAboveSpace();
                    if(currentSpace.spotType == SpotType.Passable){
                        timeBeforeMove = 3*timeToMove; 
                    }else{
                        updateDiceTextNumber(diceNumber - 1);
                        timeBeforeMove = timeToMove;
                    }
                }

                if(diceNumber <= 0 && timeBeforeMove < 0){
                    EndTurn();
                }
                timeBeforeMove -= Time.deltaTime;
            }
        }

    }

    public void SetCameraActive(bool isActive){
        if (myCamera != null){
            myCamera.SetActive(isActive);
        }else{
            Debug.LogWarning("My Camera not found!");
        }
    }

    public void moveAboveSpace(){
        Vector3 aboveSpace = currentSpace.position;
        aboveSpace.y += 3;
        this.gameObject.transform.position = aboveSpace;

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;  // Stops all movement
    } 

    public void changeNumber(){
        int newNumber = Random.Range(1, 11);  // Upper bound is exclusive
        while(diceNumber == newNumber){
            newNumber = Random.Range(1, 11);  // Upper bound is exclusive
        }
        updateDiceTextNumber(newNumber);
   }

    public void updateDiceTextNumber(int number){
        diceNumber = number;
        diceTextObject.text = "" + diceNumber;
    }
}
