using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{


     void OnTriggerEnter(Collider other) //not working
    {
        if (other.tag == "Player")
        {

            other.GetComponent<MovementLogic>().setIsGroundedBool(true);
        }
    }
     void OnTriggerExit(Collider other) // not working.
    {
        if (other.tag == "Player")
        {


            other.GetComponent<MovementLogic>().setIsGroundedBool(false);
        }
    }
}
