using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForceObject 
{
    public Vector3 forceOfThisObject;
    public string name;
  
    public ForceObject(Vector3 force, string name){

        this.forceOfThisObject = force;
        this.name = name;
    }

    public ForceObject returnObjectBasedOnName(string name){

        return this;
    }
}
