using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public float animationSpeed;
    private MovementLogic objectMovementLogic;
    private Animator animBody;
    // Start is called before the first frame update
    void Start()
    {
        objectMovementLogic = this.gameObject.GetComponent<MovementLogic>();
        animBody = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movementAnimation();
    }

    private void movementAnimation(){
  if (isPlayerInputingXAndZMovement() && objectMovementLogic.isGrounded)
        {
            animBody.SetBool("Running", true);
        }
        if (isObjectStill())
        {
            animBody.SetBool("Running", false);
        }
        if (Input.GetButtonDown("Jump"))
        {
            animBody.SetBool("Jump", true); //starts animations
        }
        if (Input.GetButtonUp("Jump"))
        {
            animBody.SetBool("Jump", false);
        }

        if (objectMovementLogic.isGrounded)
        {
            animBody.SetBool("IsGrounded", true);

        }
        else
        {
            animBody.SetBool("IsGrounded", false);
        }

  


        if (animBody.GetCurrentAnimatorStateInfo(0).IsName("Run") && !animBody.IsInTransition(0)) //Slowly increases animation speed if were running and gonna change animation soon.
        {
            animBody.speed = 0.55f * objectMovementLogic.currentSpeed / objectMovementLogic.maxSpeed + 0.45f;
        }
        else
        {
            
            animBody.speed = 1;
        }
        
        animationSpeed = animBody.speed;

    }

    private bool isPlayerInputingXAndZMovement(){
        if(Mathf.Abs(objectMovementLogic.objectInput.x) > 0.0f   || Mathf.Abs(objectMovementLogic.objectInput.z) > 0.0f){
            return true;
        }
        return false;
    }

    private bool isObjectStill(){
        if(objectMovementLogic.currentSpeed == 0.0f){
            return true;
        }
        return false;
    }
}
