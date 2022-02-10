using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script goes on the player "Player". Objects to be picked up must be tagged as "Object" and posess a collider.
public class PlayerPickup : MonoBehaviour
{
    public int objects = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) //When the player bumps into something...
    {
        if (other.tag == "Object") //If it's an object to be picked up...
        {
            objects++; //Increment the object count
            other.gameObject.SetActive(false); //Remove the object from the scene.
        }
    }
}
