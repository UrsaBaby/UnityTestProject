using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{

    public Vector3 playerInput;
    public Vector3 lastPlayerInput;
    public Vector3 playerInputForce;
    public Vector3 vectorSpeed;
    public float currentSpeed;
    public float currentYSpeed;
    public float maxSpeed = 6.0f;
    public int jumpPower = 50;

    public float animationSpeed;
    private Rigidbody moveBody;

    public float currentAcceleration = 0.0f;
    public float accelerationCoefficient = 0.4f;
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
        getPlayerInput();
        calculateAcceleration();
        yMovement();
        resetJumpsAvailableIfGrounded();
        calculatePlayerInputForce();
        setCurrentSpeed();
        normalizePlayerInputForce();
        addForceToRigidBody(playerInputForce);

    }

    private void normalizePlayerInputForce()
    {
        if (Mathf.Abs(vectorSpeed.x) > maxSpeed)
        {
            playerInputForce.x = 0;
        }
        if (Mathf.Abs(vectorSpeed.z) > maxSpeed)
        {
            playerInputForce.z = 0;
        }
    }
    public void addForceToRigidBody(Vector3 forceToAdd)
    {
        moveBody.AddForce(forceToAdd);

    }

    public void yMovement()
    {
        currentYSpeed = Mathf.Abs(vectorSpeed.y);
        if (canjump())
        {
            if (Input.GetButtonDown("Jump"))
            {
                playerInput.y = jumpsAvailable * jumpPower + 50;
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

    private void getPlayerInput()
    {

        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    }

    private void calculateAcceleration()
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
                increaseAcceleration(accelerationCoefficient);
            }
        }
        else
        {
            decreaseAccelerationToZero();
        }
    }

    private void calculatePlayerInputForce()
    {
        if (isMovementKeysDown())
        {
            playerInputForce = new Vector3(playerInput.x * currentAcceleration, playerInput.y, playerInput.z * currentAcceleration);
        }
        else
        {
            playerInputForce = new Vector3(playerInput.x, playerInput.y, playerInput.z);
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

    private void setCurrentSpeed()
    {
        if (Mathf.Abs(vectorSpeed.x) > Mathf.Abs(vectorSpeed.z))
        {
            currentSpeed = Mathf.Abs(vectorSpeed.x);
        }
        else
        {
            currentSpeed = Mathf.Abs(vectorSpeed.z);
        }
    }

    private float calculateChangeInDirection()
    {
        return Vector3.Dot(playerInput.normalized, lastPlayerInput.normalized);
    }

    private bool hasDirectionalInputChanged()
    {
        if (playerInput != lastPlayerInput)
        {
            return true;
        }
        return false;
    }

    private void updateLastInput()
    {
        if (hasDirectionalInputChanged())
        {
            lastPlayerInput = playerInput;
        }
    }

    private void decreaseAccelerationBasedOnChangedDirection(float changeInDirection)
    {
        if (changeInDirection < 0) //if directions changes 180 reduce acceleeration to 0
        {
            currentAcceleration = 10f;
        }
        if (changeInDirection <= 0.75)
        { //if directions changes slightly ex north to northwest, reduce acceleration slightly
            currentAcceleration = currentAcceleration * 0.75f;
        }
    }

    private void decreaseAccelerationToZero()
    {
        if (currentAcceleration > 0) //if were not presing any butten and have acceleration, decrease it.
        {
            currentAcceleration -= accelerationCoefficient * 2;
        }
        if (currentAcceleration < 0.1) //if were almost still, stand still.
        {
            currentAcceleration = 0f;
        }
    }

    public void increaseAcceleration(float acceleration)
    {
        currentAcceleration += acceleration;
    }

    public bool isPlayerInputCreatingMovement()
    {
        if (Mathf.Abs(playerInput.x) > 0 || Mathf.Abs(playerInput.z) > 0)
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
