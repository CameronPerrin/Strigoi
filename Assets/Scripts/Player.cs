using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	private bool m_FacingRight = true;
	private float move;
	private float health;
	private float cdPAScript; // Cool down from player Attack script value
	private float cdMove;

	public Image healthOrb;
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	public float maxHealth = 50;
	public float moveAttackRange = 100;
	
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
		
		// Grabbing this information for player CD + movement while attacking
		// You could place this in Start(), but I put in update in case we change
		// the attack speed in-game, so now it can update automatically.
		cdPAScript = GetComponent<PlayerAttack>().CoolDownAmount;

		// Player movement code
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
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
			(controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		// Move the player when he attacks and no input from movement
		if(Input.GetKey(KeyCode.Mouse0) && cdMove <= 0 && Input.GetAxisRaw("Horizontal") == 0)
		{
			if(m_FacingRight)
			{
				velocity.x = Mathf.SmoothDamp (0, moveAttackRange, ref velocityXSmoothing, 
					(controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
			}
			else if(!m_FacingRight)
			{
				velocity.x = Mathf.SmoothDamp (0, -moveAttackRange, ref velocityXSmoothing, 
					(controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
			}
			cdMove = cdPAScript;
			Debug.Log(velocity.x);
		}
		else
		{
			cdMove -= Time.deltaTime;
		}

		// This an attempted fix to a bug that causes the player to jolt forward an extremely large amount
		// if the player alt-tabs out then clicks back into the game
		if(velocity.x > 10 && Input.GetAxisRaw("Horizontal") == 0)
			velocity.x = 0;
		if(velocity.x < -10 && Input.GetAxisRaw("Horizontal") == 0)
			velocity.x = 0;
		
		// Flip the player when facing a different direction
		if (move > 0 && !m_FacingRight)
      	{
        	Flip();
      	}
      	else if(move < 0 && m_FacingRight)
      	{
      		Flip();
      	}

      	// change orb size based off of health
      	healthOrb.fillAmount = health / maxHealth;
      	// Death
      	if(health <= 0)
      	{
      		Object.Destroy(this.gameObject);
      	}


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