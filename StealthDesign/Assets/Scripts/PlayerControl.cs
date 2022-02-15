using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    private Vector3 moveDirection; //The direction the player is moving in
    private Rigidbody myRB; //The player's rigidbody
    public Transform cameraobject;
    [SerializeField] private bool grounded = false;


    public float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = cameraobject.forward * Input.GetAxis("Vertical");
        moveDirection += cameraobject.right * Input.GetAxis("Horizontal"); //These two lines get the horizontal and vertical components of movement based on player input
        moveDirection.Normalize(); //Normalize it so it's between 0 and 1
        moveDirection.y = 0; //Make sure you aren't going up any
        
        Vector3 movementVelocity = moveDirection * movespeed; //Multiply by speed to get velocity
        
        myRB.velocity = movementVelocity; //Apply that to your rigidbody

        if (!grounded)
        {
            myRB.AddForce(0, -movespeed * 3, 0);
        }
        
    }

    public void Grounded(bool tf)
    {
        grounded = tf;
    }
}
