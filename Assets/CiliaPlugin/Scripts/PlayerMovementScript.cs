﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate((Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f), 0, (Input.GetAxis("Vertical") * Time.deltaTime * 3.0f));
    }
}
