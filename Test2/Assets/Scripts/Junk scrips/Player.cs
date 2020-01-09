using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxSpeed = 3.0f;
    public float currentAcceleration = 0.0f; //is not used
    public Vector3 currentInputMovement;
    public Vector3 currentPlayerInputForces;
    public Vector3 currentForces;
    public List<ForceObject> listOfCurrentForcesThisGameObjectCreates = new List<ForceObject>();
    public Vector3 direction;

    public float accelerationCoefficient = 0.01f;
    public float jumpPower = 6.0f;
    public int gravity = 15;
    public float currentHorizontalPlaneForce = 0.0f;

    public float horizontalPlaneMovementTimer = 0f;
    public float minSpeed = 0.0f;
    private bool canJump = true;
    public float maxAcceleration = 1.5f;
    private CharacterController moveController;
 


    void Start()
    {


        moveController = this.GetComponent<CharacterController>();
     



        //list of ForceObject with force and name of the force (ex inputMovement). Check internally (ie before) adding to list if the force has changed.
     


    }

    // Update is called once per frame
    void Update()
    {
        this.movement();
    }

    private void movement()
    {

        this.horizontalPlaneMovement();
        currentInputMovement = new Vector3(this.verticalPlaneMovement().x, currentHorizontalPlaneForce, this.verticalPlaneMovement().z);
        returnCurrentInputForceToZeroIfInputEqualsZero(currentInputMovement); //probably not used
        addToCurrentPlayerInputForces(currentInputMovement);
        // addAccelerationToCurrentPlayerInputForce(); // makes things wierd


        normalizeForceByMaxSpeed(currentPlayerInputForces);

        addPlayerForcesToCurrentForces(currentPlayerInputForces * currentAcceleration);
        addToDirection2(currentForces);
        moveInThisDirectionAtThisSpeed(direction);
        deaccelerateCurrentForces();

        //movement is based on current vertical force, whis is based on currentacceleration, whis is based on movement input. fix this.

    }

    private Vector3 verticalPlaneMovement()
    {
        Vector3 verticalPlaneMovementCatcher = new Vector3();
        if (isMovementKeysDown())
        {
            if (canAccelerate())
            {
                accelerateCurrentAcceleration(accelerationCoefficient); //acceleratorCoefficient, should be based on groundtype and possibly speed;
            }
            verticalPlaneMovementCatcher = new Vector3(Input.GetAxis("Horizontal"), currentHorizontalPlaneForce, Input.GetAxis("Vertical"));

        }
        else
        {
            if (canDeaccelerate())
            {
                accelerateCurrentAcceleration(-accelerationCoefficient); // this deaccelerates the input force, not the current force or direction. GOES WAY TOO FAST.
            }

        }
        return verticalPlaneMovementCatcher;

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

    private bool canAccelerate()
    {
        if (currentAcceleration <= maxAcceleration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void accelerateCurrentAcceleration(float accelerationCoefficient)
    {
        if (direction.x == 0 && direction.z == 0)
        {
            currentAcceleration = 0;
        }
        currentAcceleration += accelerationCoefficient;

    }


    private bool canDeaccelerate()
    {
        if (currentAcceleration > minSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void horizontalPlaneMovement()
    {
        if (moveController.isGrounded)
        {
            setCurrentHorizontalPlaneForce(0.0f);
            this.canJump = true;
        }
        if (Input.GetButtonDown("Jump") && canJump)
        {
            addToCurrentHorizontalPlaneSpeed(jumpPower);
        }
        else
        { // make maxJumpHeight Based on current horizontalPlaneSpeed
            if (!moveController.isGrounded)
            {
                canJump = false;
                addToCurrentHorizontalPlaneSpeed(-gravity * Time.deltaTime);
            }

        }
    }

    public void setCurrentHorizontalPlaneForce(float toThisSpeed)
    {
        currentHorizontalPlaneForce = toThisSpeed;
    }

    public void addToCurrentHorizontalPlaneSpeed(float addThisSpeed)
    {
        currentHorizontalPlaneForce += addThisSpeed;
    }

    private void returnDirectionToZeroIfInputEqualsZero(Vector3 thisInput)
    {
        if (thisInput.x == 0.0f)
        {
            direction.x = deAccelerateThisForce(direction.x);
        }
        if (thisInput.z == 0.0f)
        {
            direction.z = deAccelerateThisForce(direction.z);
        }
    }

    private void returnCurrentInputForceToZeroIfInputEqualsZero(Vector3 thisInput)
    {
        if (thisInput.x == 0.0f)
        {
            currentPlayerInputForces.x = 0;
        }
        if (thisInput.z == 0.0f)
        {
            currentPlayerInputForces.z = 0;
        }

    }

    private void deaccelerateCurrentForces()
    {
        currentForces.x = deAccelerateThisForce(currentForces.x);
        currentForces.z = deAccelerateThisForce(currentForces.z);
        if (currentForces.x > -0.1f && currentForces.x < 0.1f)
        {
            currentForces.x = 0.0f;
        }
        if (currentForces.z > -0.1f && currentForces.z < 0.1f)
        {
            currentForces.z = 0.0f;
        }
    }

    private float deAccelerateThisForce(float thisFloat)
    {
        if (thisFloat > 0)
        {
            return thisFloat - accelerationCoefficient * 5; // should not be acceleration coefficient, but based on ground type.
        }
        if (thisFloat < 0)
        {
            return thisFloat + accelerationCoefficient * 5;
        }

        return thisFloat;
    }


    public void addToCurrentForces(Vector3 forceToAdd)
    {
        currentForces = new Vector3(currentForces.x + forceToAdd.x, currentForces.y + forceToAdd.y, currentForces.z + forceToAdd.z);

    }

    public void addToDirection2(Vector3 addThisDerection)
    {
        Vector3 newDirection = new Vector3(addThisDerection.x, addThisDerection.y, addThisDerection.z);
        setDirection(newDirection);
    }
    public void addToDirection(Vector3 addThisDerection)
    {
        Vector3 newDirection = new Vector3(playerXMovement(addThisDerection.x), addThisDerection.y, playerZMovement(addThisDerection.z));
        setDirection(newDirection);
    }

    public void addToForces(Vector3 addThisForce)
    {
        Vector3 newForce = new Vector3(addThisForce.x + direction.x, addThisForce.y, playerZMovement(addThisForce.z));
        addToCurrentForces(newForce);
    }

    public void addToCurrentPlayerInputForces(Vector3 forceToAdd)
    {
        if (currentPlayerInputForces.x + forceToAdd.x >= maxSpeed)
        {
            currentPlayerInputForces = new Vector3(currentPlayerInputForces.x + (maxSpeed - currentPlayerInputForces.x), currentPlayerInputForces.y, currentPlayerInputForces.z); // redo all this with matf.abs instead
        }
        else if (currentPlayerInputForces.x + forceToAdd.x <= -maxSpeed)
        {
            currentPlayerInputForces = new Vector3(currentPlayerInputForces.x + (-maxSpeed - currentPlayerInputForces.x), currentPlayerInputForces.y, currentPlayerInputForces.z);
        }
        else
        {
            currentPlayerInputForces = new Vector3(currentPlayerInputForces.x + forceToAdd.x, currentPlayerInputForces.y, currentPlayerInputForces.z);
        }

        if (currentPlayerInputForces.z + forceToAdd.z >= maxSpeed)
        {
            currentPlayerInputForces = new Vector3(currentPlayerInputForces.x, currentPlayerInputForces.y, currentPlayerInputForces.z + (maxSpeed - currentPlayerInputForces.z));
        }
        else if (currentPlayerInputForces.z + forceToAdd.z <= -maxSpeed)
        {
            currentPlayerInputForces = new Vector3(currentPlayerInputForces.x, currentPlayerInputForces.y, currentPlayerInputForces.z + (-maxSpeed - currentPlayerInputForces.z));
        }
        else
        {
            currentPlayerInputForces = new Vector3(currentPlayerInputForces.x, currentPlayerInputForces.y, currentPlayerInputForces.z + forceToAdd.z);
        }

    }

    /*public void AddPlayerForcesToCurrentForceList(ForceObject forceToAdd){
         if (currentForces.x + forceToAdd >= maxSpeed)
        {
            listOfCurrentForcesThisGameObjectCreates.Add(forceToAdd);
            currentForces = new Vector3(currentForces.x + (maxSpeed - currentForces.x), currentForces.y, currentForces.z); // redo all this with matf.abs instead
        }
        else if (currentForces.x + forceToAdd.x <= -maxSpeed)
        {
            listOfCurrentForcesThisGameObjectCreates.Add(forceToAdd);
            currentForces = new Vector3(currentForces.x + (-maxSpeed - currentForces.x), currentForces.y, currentForces.z);
        }
        else
        {
            listOfCurrentForcesThisGameObjectCreates.Add(forceToAdd);
            currentForces = new Vector3(currentForces.x + forceToAdd.x, currentForces.y, currentForces.z);
        }

        if (currentForces.z + forceToAdd.z >= maxSpeed)
        {
            listOfCurrentForcesThisGameObjectCreates.Add(forceToAdd);
            currentForces = new Vector3(currentForces.x, currentForces.y, currentForces.z + (maxSpeed - currentForces.z));
        }
        else if (currentForces.z + forceToAdd.z <= -maxSpeed)
        {
            listOfCurrentForcesThisGameObjectCreates.Add(forceToAdd);
            currentForces = new Vector3(currentForces.x, currentForces.y, currentForces.z + (-maxSpeed - currentForces.z));
        }
        else
        {
            listOfCurrentForcesThisGameObjectCreates.Add(forceToAdd);
            currentForces = new Vector3(currentForces.x, currentForces.y, currentForces.z + forceToAdd.z);
        }
    }*/



    public void addPlayerForcesToCurrentForces(Vector3 forceToAdd)
    {
        if (currentForces.x + forceToAdd.x >= maxSpeed)
        {
            currentForces = new Vector3(currentForces.x + (maxSpeed - currentForces.x), currentForces.y, currentForces.z); // redo all this with matf.abs instead
        }
        else if (currentForces.x + forceToAdd.x <= -maxSpeed)
        {
            currentForces = new Vector3(currentForces.x + (-maxSpeed - currentForces.x), currentForces.y, currentForces.z);
        }
        else
        {
            currentForces = new Vector3(currentForces.x + forceToAdd.x, currentForces.y, currentForces.z);
        }

        if (currentForces.z + forceToAdd.z >= maxSpeed)
        {
            currentForces = new Vector3(currentForces.x, currentForces.y, currentForces.z + (maxSpeed - currentForces.z));
        }
        else if (currentForces.z + forceToAdd.z <= -maxSpeed)
        {
            currentForces = new Vector3(currentForces.x, currentForces.y, currentForces.z + (-maxSpeed - currentForces.z));
        }
        else
        {
            currentForces = new Vector3(currentForces.x, currentForces.y, currentForces.z + forceToAdd.z);
        }
    }

    private Vector3 normalizeForceByMaxSpeed(Vector3 normalizeThisForce)
    {
        if (normalizeThisForce.x > maxSpeed)
        {
            normalizeThisForce.x = maxSpeed;
        }
        if (normalizeThisForce.x < -maxSpeed)
        {
            normalizeThisForce.x = -maxSpeed;
        }
        if (normalizeThisForce.z > maxSpeed)
        {
            normalizeThisForce.z = maxSpeed;
        }
        if (normalizeThisForce.z < -maxSpeed)
        {
            normalizeThisForce.z = -maxSpeed;
        }
        return normalizeThisForce;

    }

    private float playerXMovement(float addToMovement)
    {

        if (direction.x + addToMovement > 1)
        {
            return 1.0f;
        }
        if (direction.x + addToMovement < -1)
        {
            return -1.0f;
        }
        else
        {
            return direction.x + addToMovement;
        }
    }

    public void setDirection(Vector3 thisDirection)
    {
        direction = thisDirection;
    }

    private void moveInThisDirectionAtThisSpeed(Vector3 towardsThisDirection)
    {

        moveController.Move(towardsThisDirection * Time.deltaTime);
    }

    private float playerZMovement(float addToMovement)
    {

        if (direction.z + addToMovement > 1)
        {
            return 1.0f;
        }
        if (direction.z + addToMovement < -1)
        {
            return -1.0f;
        }
        else
        {
            return direction.z + addToMovement;
        }
    }

    public Vector3 getCurrentPositionOfPlayer()
    {
        return this.transform.position;
    }

    private void setMovementToCompletelyStill()
    {
        if (Mathf.Abs(currentForces.x) < 0.1f)
        {

            currentForces.x = 0.0f;
        }


    }




















}
