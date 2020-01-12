using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{

    public Vector3 objectInput;
    public Vector3 lastPlayerInput;
    public Vector3 inputForce;
    public Vector3 vectorSpeed;
    public float currentSpeed;
    public float currentYSpeed;
    public float maxSpeed = 6.0f;
    public int jumpPower = 50;

    public float animationSpeed;
    private Rigidbody moveBody;

    public float objectCurrentAcceleration = 0.0f;
    public float objectAccelerationCoefficient = 0.4f;
    public bool isGrounded;
    private bool hasJumped;
    public int jumpsAvailable = 2;
    private Quaternion currentRotation;
    private Quaternion newRotation;
    private Vector3 newRotationInV3;

    // Start is called before the first frame update
    void Start()
    {
        moveBody = gameObject.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        movement();

        setRotation();
    }



    private void setRotation()
    {
        currentRotation = this.transform.rotation;
        newRotation = Quaternion.LookRotation(this.transform.position - Camera.main.transform.position, Vector3.up);
        newRotationInV3 = newRotation.eulerAngles;
        newRotationInV3.x = 0;
        newRotationInV3.z = 0;
        newRotation = Quaternion.Euler(newRotationInV3);
        this.transform.rotation = newRotation;
    }
    private void movement()
    {
        vectorSpeed = moveBody.velocity;

        objectInput = getPlayerInput();
        objectInput = rotateThisInputByThisRotation(objectInput, this.transform.rotation); //TODO!
        objectCurrentAcceleration = calculateAcceleration(objectCurrentAcceleration, objectAccelerationCoefficient);
        yMovement(); //refactor this
        resetJumpsAvailableIfGrounded();
       
        inputForce = calculateInputForceInThisDirection(objectInput, objectCurrentAcceleration, this.transform.rotation );
        currentSpeed  = setHighestDirectionalSpeedAsCurrentSpeed(moveBody.velocity.x, moveBody.velocity.z);
        inputForce = stopAddingToForceAtThisVelocity(inputForce, maxSpeed, moveBody);
        addForceToRigidBody(inputForce, moveBody);

    }

    private Vector3 rotateThisInputByThisRotation(Vector3 playerInput, Quaternion playerRotation){

        float angle;
        Vector3 axis = Vector3.zero;
        playerRotation.ToAngleAxis(out angle, out axis);
        playerInput = Quaternion.AngleAxis(angle, Vector3.up) * playerInput;

        return playerInput;
    }
    private void calculateRotationDirectionForPlayerInputForce()
    {

    }

    private Vector3 stopAddingToForceAtThisVelocity(Vector3 thisForce, float thisVelocity, Rigidbody thisBody)
    {
        Vector3 returnForce = new Vector3(thisForce.x,thisForce.y,thisForce.z);
        if (Mathf.Abs(thisBody.velocity.x) > thisVelocity)
        {
            returnForce.x = 0;
        }
        if (Mathf.Abs(thisBody.velocity.z) > thisVelocity)
        {
            returnForce.z = 0;
        }
        return returnForce;
    }
    public void addForceToRigidBody(Vector3 forceToAdd, Rigidbody thisBody)
    {
        thisBody.AddForce(forceToAdd);

    }

    public void yMovement()
    {
        currentYSpeed = Mathf.Abs(vectorSpeed.y);
        if (canjump())
        {
            if (Input.GetButtonDown("Jump"))
            {
                objectInput.y = jumpsAvailable * jumpPower + 50;
                hasJumped = true;
            }
            else
            {
                hasJumped = false;
            }
        }
        else
        {
            hasJumped = false;
        }

        if (hasJumped)
        {
            jumpsAvailable = jumpsAvailable - 1;
            isGrounded = false;
        }

    }

    private Vector3 getPlayerInput()
    {

        objectInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        return objectInput;
    }

    private float calculateAcceleration(float thisAcceleration, float byThisAccelerationCoeffcient)
    {
        if (isPlayerInputCreatingMovement())
        {
            if (hasDirectionalInputChanged())
            {
                decreaseAccelerationBasedOnChangedDirection(calculateChangeInDirection());
                updateLastInput();
            }
            if (currentSpeed < maxSpeed * 0.8)
            {
                return increaseAcceleration(thisAcceleration, byThisAccelerationCoeffcient);
            }
            else
            {
                return thisAcceleration;
            }
        }
        else
        {
            return decreaseAccelerationToZero(thisAcceleration, objectAccelerationCoefficient);
        }
       
    }
    public float increaseAcceleration(float thisAcceleration, float acceleration)
    {
        return thisAcceleration += acceleration;
    }
    private float decreaseAccelerationToZero(float thisAcceleration, float deaccelerationCoefficient)
    {
        if (thisAcceleration > 0) //if were not presing any butten and have acceleration, decrease it.
        {
            return thisAcceleration -= objectAccelerationCoefficient * 2;
        }
        else if (thisAcceleration < 0.1) //if were almost still, stand still.
        {
            return thisAcceleration = 0f;
        }
        else
        {
            return 0f;
        }

    }

    private Vector3 calculateInputForceInThisDirection(Vector3 byThisInput, float byThisAcceleration, Quaternion directions)
    {
        if (isMovementKeysDown())
        {
            return new Vector3(byThisInput.x * byThisAcceleration, byThisInput.y, byThisInput.z * byThisAcceleration);
        }
        else
        {
            return new Vector3(byThisInput.x, byThisInput.y, byThisInput.z);
        }
    }

    private bool isMovementKeysDown()
    {
        bool returnBool = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            returnBool = true;
        }
        return returnBool;
    }

    private float setHighestDirectionalSpeedAsCurrentSpeed(float xDirectionalSpeed, float zDirectionalSpeed
    )
    {
        if (Mathf.Abs(xDirectionalSpeed) > Mathf.Abs(zDirectionalSpeed))
        {
            return Mathf.Abs(xDirectionalSpeed);
        }
        else
        {
            return Mathf.Abs(zDirectionalSpeed);
        }
    }

    private float calculateChangeInDirection()
    {
        return Vector3.Dot(objectInput.normalized, lastPlayerInput.normalized);
    }

    private bool hasDirectionalInputChanged()
    {
        if (objectInput != lastPlayerInput)
        {
            return true;
        }
        return false;
    }

    private void updateLastInput()
    {
        if (hasDirectionalInputChanged())
        {
            lastPlayerInput = objectInput;
        }
    }

    private void decreaseAccelerationBasedOnChangedDirection(float changeInDirection)
    {
        if (changeInDirection < 0) //if directions changes 180 reduce acceleeration to 0
        {
            objectCurrentAcceleration = 10f;
        }
        if (changeInDirection <= 0.75)
        { //if directions changes slightly ex north to northwest, reduce acceleration slightly
            objectCurrentAcceleration = objectCurrentAcceleration * 0.75f;
        }
    }





    public bool isPlayerInputCreatingMovement()
    {
        if (Mathf.Abs(objectInput.x) > 0 || Mathf.Abs(objectInput.z) > 0)
        {
            return true;
        }
        return false;
    }

    public void setIsGroundedBool(bool setToThis)
    {
        isGrounded = setToThis;
    }

    public bool canjump()
    {
        if (isGrounded || jumpsAvailable > 0)
        {
            return true;
        }
        return false;
    }

    public void resetJumpsAvailableIfGrounded()
    {
        if (isGrounded)
        {
            jumpsAvailable = 2;

        }
    }

}
