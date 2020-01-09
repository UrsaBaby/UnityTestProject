using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class setColorOnThisTag
{

   
    public static void setThisColorOnThisTag(string tag){
        GameObject[] gameObjectCatcher = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject checker in gameObjectCatcher){
            checker.GetComponent<Renderer>().material.color = Color.red;
        }
    }

}
