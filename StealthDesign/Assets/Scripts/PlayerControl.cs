using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    //Variables needed for enemy AI
    public static Vector3 playerPos;
    public static bool crouched = false;
    public float crouchSpeedMult = 0.5f;

    public float maxStamina = 5;
    private float stamina;
    public float runSpeedMult = 2;
    
    private Vector3 moveDirection; //The direction the player is moving in
    private Rigidbody myRB; //The player's rigidbody
    public Transform cameraobject;
    [SerializeField] private bool grounded = false;
    public float extraGravity = 6;

    public float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        stamina = maxStamina;

        //Getting player position
        StartCoroutine(TrackPlayer());
    }
    //Tracking player position
    IEnumerator TrackPlayer()
    {
        while (true)
        {
            playerPos = gameObject.transform.position;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = cameraobject.forward * Input.GetAxis("Vertical");
        moveDirection += cameraobject.right * Input.GetAxis("Horizontal"); //These two lines get the horizontal and vertical components of movement based on player input
        moveDirection.Normalize(); //Normalize it so it's between 0 and 1
        moveDirection.y = 0; //Make sure you aren't going up any
        
        Vector3 movementVelocity = moveDirection * movespeed; //Multiply by speed to get velocity
        if(crouched)
        {
            movementVelocity *= crouchSpeedMult; //If you're crouching, multiply your speed by the relevant multiplier.
        }

        if (Input.GetKey(KeyCode.LeftShift) && !crouched && stamina > 0)//If you're sprinting and not crouching and have stamina...
        {
            movementVelocity *= runSpeedMult; //Multiply your speed by the relevant multiplier.
            stamina -= Time.deltaTime; //Reduce your stamina.
        }

        myRB.velocity = movementVelocity; //Apply that to your rigidbody

        if (!grounded)
        {
            myRB.AddForce(0, -movespeed * extraGravity, 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            gameObject.transform.localScale /= 2;
            crouched = true;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            gameObject.transform.localScale *= 2;
            crouched = false;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && stamina < maxStamina)
        {
            stamina += Time.deltaTime;
        }
    }

    public void Grounded(bool tf)
    {
        grounded = tf;
    }

    
}
