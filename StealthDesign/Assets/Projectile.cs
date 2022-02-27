using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Vector3 impactPos;
    public static bool makeNoise = false;
    public AudioSource aS;
    private bool playonce = true;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
            Debug.Log("Projectile Collision");
            makeNoise = true;
            impactPos = gameObject.transform.position;
            if (playonce)
            {
                aS.Play();
                playonce = false;
            }
            
            //destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
