using UnityEngine;

public class DiceRoller : MonoBehaviour {

    public float rotateXSpeed;
    public float rotateYSpeed; 
    public float rotateZSpeed; 
    
    public float baseXSpeed;
    public float baseYSpeed; 
    public float baseZSpeed; 

    public float xSpeedRange;
    public float ySpeedRange; 
    public float zSpeedRange; 

    public float xOscilationSpeed;
    public float yOscilationSpeed;
    public float zOscilationSpeed;

    public bool isDiceSpinning;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        isDiceSpinning = true;
    }

    // Update is called once per frame
    void Update(){
        if(isDiceSpinning){
            RotateDice();
            UpdateRotationSpeeds();
        }

        // if (Input.GetKeyDown(KeyCode.J)){//J = A Button 
        //     isDiceSpinning = !isDiceSpinning;
        //     SnapToNearestFace();
        // }
    }

    public void StopRolling(){
        isDiceSpinning = false;
        SnapToNearestFace();
    }

    public void StartRolling(){
        isDiceSpinning = true;
    }

    public void SnapToNearestFace(){
        // Debug.Log(this.gameObject.transform.eulerAngles);

        float nearestX = nearest90(this.gameObject.transform.eulerAngles.x);
        float nearestY = nearest90(this.gameObject.transform.eulerAngles.y);
        float nearestZ = nearest90(this.gameObject.transform.eulerAngles.z);

        Vector3 rotation = new Vector3 (nearestX, nearestY, nearestZ);
        
        this.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public float nearest90(float angle){
        // float ninetyDegreeRadians = (Mathf.PI) / 2;

        return Mathf.Round(angle / 90f) * 90f;
    }
    
    public void UpdateRotationSpeeds(){
        rotateXSpeed = baseXSpeed;

        float sineValue = Mathf.Sin(Time.time * yOscilationSpeed); //-1 to 1
        rotateYSpeed = baseYSpeed + (ySpeedRange * sineValue);

        rotateZSpeed = baseZSpeed;
    }

    public void RotateDice(){
        Vector3 rotation = new Vector3(rotateXSpeed,rotateYSpeed,rotateZSpeed) * Time.deltaTime;
        this.gameObject.transform.Rotate(rotation.x, rotation.y, rotation.z);
    }
}
