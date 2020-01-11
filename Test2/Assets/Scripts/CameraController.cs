using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 directionFromCameraToObject;
    private Vector3 directionFromObjectToCamera;
    private float AngleX = 0;
    private float AngleZ = 0;
    GameObject[] playerCharacter;
    GameObject[] cameraHolder;
    GameObject[] navigator;
    Vector3[] cameraDirections;
    private Vector3 currentDistanceAndDirection;

    // Start is called before the first frame update
    void Start()
    {
        cameraHolder = GameObject.FindGameObjectsWithTag("CameraHolder");
        playerCharacter = GameObject.FindGameObjectsWithTag("Player");
        navigator = GameObject.FindGameObjectsWithTag("Navigator");
        //cameraDirections[0] = new Vector3(1f,3f,0f);

        directionFromCameraToObject = cameraHolder[0].transform.position - navigator[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        followThisObjectWithARotatableNavigatorAndCameraFromThisDirection(playerCharacter[0], navigator[0], cameraHolder[0], directionFromCameraToObject);
    }

    private void followThisObjectWithARotatableNavigatorAndCameraFromThisDirection(GameObject PlayerControlled, GameObject thisNavigator, GameObject thisCameraHolder, Vector3 directionFromCameraToObject)
    {
        setObjectAtThisOtherObject(thisNavigator, PlayerControlled);
        currentDistanceAndDirection = getCurrentDistanceBetweenTwoObjectsInThisDirection(thisCameraHolder, thisNavigator, directionFromCameraToObject);
        thisCameraHolder.transform.position = currentDistanceAndDirection; //makes camera follow character
        rotateGameObjectWithMouse(thisNavigator);
        rotateObjectAroundThisObject(thisCameraHolder, thisNavigator);
    }



    private void setObjectAtThisOtherObject(GameObject thisNavigator, GameObject playerControlled)
    {
        thisNavigator.transform.position = playerControlled.transform.position;

    }

    private Vector3 getCurrentDistanceBetweenTwoObjectsInThisDirection(GameObject thisCameraHolder, GameObject thisNavigator, Vector3 directionFromCameraToObject)
    {
        return thisNavigator.transform.position + thisNavigator.transform.rotation * directionFromCameraToObject; //the difference in these two positions in thisDirection
    }

    private void rotateObjectAroundThisObject(GameObject thisCameraHolder, GameObject thisNavigator)
    {
        thisCameraHolder.transform.rotation = Quaternion.LookRotation(thisNavigator.transform.position - thisCameraHolder.transform.position, Vector3.up); // directionFromObjectToCamera in first vector
    }



    private void rotateGameObjectWithMouse(GameObject thisNavigator)
    {
        Quaternion currentObjectRotation = thisNavigator.transform.rotation;

        AngleX += Input.GetAxis("Mouse X");
        AngleZ += Input.GetAxis("Mouse Y");

        Vector3 AxisY = new Vector3(0, 1, 0);
        Vector3 AxisX = new Vector3(1, 0, 0);

        thisNavigator.transform.rotation = Quaternion.AngleAxis(AngleX, AxisY) * Quaternion.AngleAxis(-AngleZ+90, AxisX); //rotate this much over this axis. +90 over x axis to make it start horizontally.
    }
}
