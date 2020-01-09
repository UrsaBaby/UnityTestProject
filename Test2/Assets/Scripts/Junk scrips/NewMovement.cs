using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    //current speed
    //max speed
    //Acceleration
    //Current acceleration
        //limited by max acceleration
    //max acceleration
    //current input
    //
    //current movement direction vector3
    //list of force objects added to this or maybe hashmap?

    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
    //create movement

        //create player movement    
            //get player input
                //!check if movement keys is down
                //get input and put in a vector 3.
            //Update Acceleration
                //check if movement key is down
                    //if yes
                        //check if max acceleration is achieved
                        //if yes
                        //check if direction has changed
                        //if yes
                            //slightly  deaccelerate
                        //if no    
                            //do nothing
                        //if no
                            //increase acceleration
                    //if no

            //Make player input a force
                //multiply by current acceleration.
                    //check if this force has been changed.
                    //if yes
                        //if yes
                            //Add difference to this forceobject.
                        //if no
                            //create force with name appropriate for this ability (ie thisgameobject name Input movement)
                            //add this  force to list of forceobjects/add to current force

        //calculate current forces for this gameobject
            //add to direction.


    }
}
