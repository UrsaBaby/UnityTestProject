using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerTrigger : MonoBehaviour
{
    // Start is called before the first frame update
  void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Ground")
        {

            Debug.Log("HIT");
            this.gameObject.GetComponent<MovementLogic>().setIsGroundedBool(true);
        }
    }
     void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Ground")
        {

            Debug.Log("HIT");
            this.gameObject.GetComponent<MovementLogic>().setIsGroundedBool(false);
        }
    }
}
