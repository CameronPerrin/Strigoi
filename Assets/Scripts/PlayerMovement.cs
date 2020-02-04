using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public PhysicsMaterial2D myMaterial;
    public CharacterController2D controller;
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Incline" || other.gameObject.name == "Incline")
		{
			Debug.Log("Hit Detected");
		}
	    	
	}

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        // Move character
        // Using Time.fixedDeltaTime always ensures that our character
        // moves the same amount no matter what the framerate is
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        jump = false;
        
    }

   
}
