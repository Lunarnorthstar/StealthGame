using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6) //6 Is the "WhatIsGround" layer
        {
            gameObject.GetComponentInParent<PlayerControl>().SendMessage("Grounded", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) //6 Is the "WhatIsGround" layer
        {
            gameObject.GetComponentInParent<PlayerControl>().SendMessage("Grounded", false);
        }
    }
}
