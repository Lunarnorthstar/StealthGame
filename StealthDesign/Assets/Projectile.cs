using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Vector3 impactPos;
    public static bool makeNoise = false;

    void OnCollisionEnter(Collision collision)
    {
            Debug.Log("Projectile Collision");
            makeNoise = true;
            impactPos = gameObject.transform.position;
            //destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
