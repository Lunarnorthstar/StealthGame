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
    public static bool silent = true;
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

    private AudioSource audio;

    private bool audioPlay = false;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
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
            if (!audioPlay) //If the audio isn't playing...
            {
                audio.Play(); //Play the audio
                audioPlay = true; //Tell the script the audio is playing.
            }
            silent = false; //Set silent to false
            stamina -= Time.deltaTime; //Reduce your stamina.
        }
        else //If you aren't sprinting...
        {
            audio.Stop(); //Stop playing the audio
            audioPlay = false; //Tell the script the audio isn't playing.
            silent = true; //Set silent to true
        }

        myRB.velocity = movementVelocity; //Apply that to your rigidbody

        if (!grounded) //If you're not grounded
        {
            myRB.AddForce(0, -movespeed * extraGravity, 0); //Apply force downwards. This is to keep the model grounded while also not having to mess with the gravity projectsetting.
        }

        if (Input.GetKeyDown(KeyCode.C)) //When you press C...
        {
            gameObject.transform.localScale /= 2; //Halve your scale.
            crouched = true; //Set crouch bool to true.
        }
        if (Input.GetKeyUp(KeyCode.C)) //When you release C...
        {
            gameObject.transform.localScale *= 2; //Double your scale.
            crouched = false; //Set crouch bool to false.
        }

        if (!Input.GetKey(KeyCode.LeftShift) && stamina < maxStamina) //If you're not trying to sprint and your stamina is below maximum...
        {
            stamina += Time.deltaTime; //Gain stamina based on time passed.
            
        }
    }

    public void Grounded(bool tf) //This variable is passed in via a different script.
    {
        grounded = tf;
    }

    
}
