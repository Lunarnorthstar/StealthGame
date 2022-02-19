using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script goes on the player "Player". Objects to be picked up must be tagged as "Object" and posess a collider.
public class PlayerPickup : MonoBehaviour
{
   
    public int objects = 0;

    public Text uiText; //The UI element that displays the player's object count.

    public GameObject[] objectsToActivate = new GameObject[6];

    // Start is called before the first frame update
    void Start()
    {
        objectPicker();
        objectPicker();
        objectPicker();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = "Objects:" + objects; //Display on UI
        
    }

    private void OnTriggerEnter(Collider other) //When the player bumps into something...
    {
        if (other.tag == "Object") //If it's an object to be picked up...
        {
            objects++; //Increment the object count
            other.gameObject.SetActive(false); //Remove the object from the scene.
        }
    }

    public void objectPicker()
    {
        while (true)
        {
            int random = UnityEngine.Random.Range(0, 6);
            if (objectsToActivate[random].activeSelf == false)
            {
                objectsToActivate[random].SetActive(true);
                break;
            }
        }
    }
    
    public void LoseObject()
    {
        if (objects > 0)
        {
            objects--;
            objectPicker();
        }
    }
}
