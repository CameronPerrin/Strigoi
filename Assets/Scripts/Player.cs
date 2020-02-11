using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	private bool m_FacingRight = true;
	private float move;
	private float health;

	public Image healthOrb;
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	public float maxHealth = 50;
	
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		health = maxHealth;
		
	}

	void Update() 
	{
		move = Input.GetAxisRaw("Horizontal");
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below)
		{
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		if (move > 0 && !m_FacingRight)
      	{
        	Flip();
      	}
      	else if(move < 0 && m_FacingRight)
      	{
      		Flip();
      	}

      	healthOrb.fillAmount = health / maxHealth;
	}

	private void Flip()
 	{
    	// Switch the way the player is labelled as facing.
    	m_FacingRight = !m_FacingRight;

    	// Multiply the player's x local scale by -1.
    	Vector3 theScale = transform.localScale;
    	theScale.x *= -1;
    	transform.localScale = theScale;
  	}

  	// Take damage
  	public void TakeDamage(int damage)
	{
		health -= damage;
		// Use to spawn blood on hit: 
		// Instantiate(bloodVFX, transform.position, Quaternion,identity);
	}
}