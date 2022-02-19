using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerthrow : MonoBehaviour
{
    public GameObject projectile;

    public Transform shotpoint;

    public float shootpower = 3;

    public int ammo = 0;

    public Text ammoUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && ammo > 0)
        {
            GameObject createdprojectile = Instantiate(projectile, shotpoint.position, shotpoint.rotation);
            createdprojectile.GetComponent<Rigidbody>().velocity = shotpoint.transform.forward * shootpower;
            ammo--;
        }

        ammoUI.text = "Throwables:" + ammo;
    }
    
    private void OnTriggerEnter(Collider other) //When the player bumps into something...
    {
        if (other.tag == "Ammo" && ammo < 1) //If it's an object that can be thrown...
        {
            ammo++; //Increment the ammo count
            other.gameObject.SetActive(false); //Remove the object from the scene.
        }
    }
}
