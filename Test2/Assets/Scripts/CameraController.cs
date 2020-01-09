using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 formerMousePosition;
    private Vector3 directionFromCameraToObject;
    private Vector3 directionFromObjectToCamera;
    private float AngleX = 0;
    private float AngleZ = 0;
    GameObject[] playerCharacter;
 
    // Start is called before the first frame update
    void Start()
    {
        directionFromCameraToObject = Camera.main.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         

        setCameraPosition();
        rotateGameObject();
        setCameraRotation();
    }

    private void setCameraPosition()
    {
        playerCharacter =  GameObject.FindGameObjectsWithTag("Player");
        this.transform.position = playerCharacter[0].transform.position;
        Camera.main.transform.position = this.transform.position + this.transform.rotation * directionFromCameraToObject;
    }

    private void setCameraRotation()
    {
        
        Camera.main.transform.rotation = Quaternion.LookRotation( this.transform.position-Camera.main.transform.position ,Vector3.up); // directionFromObjectToCamera in first vector
    }

 

     private void rotateGameObject()
    {
        Quaternion currentObjectRotation = this.transform.rotation;
        AngleX += (formerMousePosition.x - Input.mousePosition.x); //Degrees to change X over Y axis based on mouse movement since last frame;
        AngleZ += (formerMousePosition.y - Input.mousePosition.y); //degrees too change Z over X axis
       
        
    
        Vector3 AxisY = new Vector3(0,1,0);
        Vector3 AxisX = new Vector3(1,0,0);


   
        this.transform.rotation = Quaternion.AngleAxis(AngleX/4, AxisY)*Quaternion.AngleAxis(-AngleZ/4, AxisX); //4 is mouse sensitivity,
      

        formerMousePosition = Input.mousePosition;
    }
}
