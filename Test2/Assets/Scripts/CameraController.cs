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
    GameObject[] cameraHolder;
    GameObject[] navigator;
    Vector3[] cameraDirections;

    // Start is called before the first frame update
    void Start()
    {
        cameraHolder = GameObject.FindGameObjectsWithTag("CameraHolder");
        playerCharacter = GameObject.FindGameObjectsWithTag("Player");
        navigator = GameObject.FindGameObjectsWithTag("Navigator");
        //cameraDirections[0] = new Vector3(1f,3f,0f);
        directionFromCameraToObject = cameraHolder[0].transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        followThisObjectWithARotatableNavigatorAndCameraAtThisDistance(playerCharacter[0], navigator[0], cameraHolder[0], directionFromCameraToObject);
    }

    private void followThisObjectWithARotatableNavigatorAndCameraAtThisDistance(GameObject objectToFollow, GameObject thisNavigator, GameObject thisCamera, Vector3 thisDirection)
    {
        setObjectAtThisOtherObject(thisNavigator, objectToFollow);
        setObjectPositionByThisOtherObjectAtThisDistanceAndDirection(thisCamera, thisNavigator, thisDirection); //refactor this
        rotateGameObject(thisNavigator);
        setCameraRotation(thisCamera, thisNavigator);
    }


    private void setObjectAtThisOtherObject(GameObject thisGameObject, GameObject atThisOtherGameObject)
    {
        thisGameObject.transform.position = atThisOtherGameObject.transform.position;
        Debug.Log(thisGameObject.transform.position);
    }

    private void setObjectPositionByThisOtherObjectAtThisDistanceAndDirection(GameObject thisObject, GameObject atThisOtherObject, Vector3 thisDirection)
    {

        thisObject.transform.position = atThisOtherObject.transform.position + atThisOtherObject.transform.rotation * thisDirection; //the difference in these two positions in thisDirection
    }

    private void setCameraRotation(GameObject objectToRotate, GameObject aroundThisObject)
    {

        objectToRotate.transform.rotation = Quaternion.LookRotation(aroundThisObject.transform.position - objectToRotate.transform.position, Vector3.up); // directionFromObjectToCamera in first vector
    }



    private void rotateGameObject(GameObject objectToRotate)
    {
        Quaternion currentObjectRotation = objectToRotate.transform.rotation;
        AngleX += (formerMousePosition.x - Input.mousePosition.x); //Degrees to change X over Y axis based on mouse movement since last frame;
        AngleZ += (formerMousePosition.y - Input.mousePosition.y); //degrees too change Z over X axis



        Vector3 AxisY = new Vector3(0, 1, 0);
        Vector3 AxisX = new Vector3(1, 0, 0);



        objectToRotate.transform.rotation = Quaternion.AngleAxis(AngleX / 4, AxisY) * Quaternion.AngleAxis(-AngleZ / 4, AxisX); //4 is mouse sensitivity,


        formerMousePosition = Input.mousePosition;
    }
}
