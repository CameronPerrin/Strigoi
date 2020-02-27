using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	private bool isDashing = false;
	private bool m_FacingRight = true;
	private bool jumpCD;
	private float move;
	private float health;
	private float cdPAScript; // Cool down from player Attack script value
	private float cdMove;
	private float cdDash;
	private float cdHolyLight;
	private float attackCounter = 0;
	private float invinTime;
	private float tempHealth;

	public Animator animator;
	public GameObject HolyLightVFX;
	public Image healthOrb;
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	public float maxHealth = 50;
	public float moveAttackRange = 5;
	public float dashCoolDown = 6;
	public float dashStrength = 30;
	public float holyLightHealingStrength = 10;
	public float holyLightCoolDown = 5;
	public float invincibilityTimer = 2;

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

		// Movement Animator
		animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
		// Check if player is grounded to reset cooldown for jump
		if(controller.collisions.below)
			jumpCD = true;
		// Grabbing this information for player CD + movement while attacking
		// You could place this in Start(), but I put in update in case we change
		// the attack speed in-game, so now it can update automatically.
		cdPAScript = GetComponent<PlayerAttack>().CoolDownAmount;

		// Player movement code
		move = Input.GetAxisRaw("Horizontal");
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		// Jumping
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
		{
			velocity.y = jumpVelocity;	
		}

		// Double jumping
		if(Input.GetKeyDown (KeyCode.Space) && jumpCD == true && !controller.collisions.below)
		{
			velocity.y = jumpVelocity;
			jumpCD = false;
			Debug.Log("Double jump");
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
		(controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

	
		
		// Move the player when attacking + double attack combo
		if(GetComponent<PlayerAttack>().doubleMove == true)
		{
			velocity.x -= moveAttackRange;
			GetComponent<PlayerAttack>().doubleMove = false;
		}
		else if(GetComponent<PlayerAttack>().singleMove == true)
		{
			velocity.x += moveAttackRange;
			GetComponent<PlayerAttack>().singleMove = false;
		}

		// Dash ability
		if(Input.GetKeyDown(KeyCode.Mouse1) && cdDash <= 0)
		{
			isDashing = true;
			if(Input.GetAxisRaw("Horizontal") != 0)
				velocity.x = 0;
			if(m_FacingRight)
			{
				velocity.x += dashStrength;
			}
			else if(!m_FacingRight)
			{
				velocity.x -= dashStrength;
			}
			cdDash = dashCoolDown;
		}
		else
		{
			isDashing = false;
			cdDash -= Time.deltaTime;
		}
	
		if(isDashing == true)
		{
			invinTime = invincibilityTimer;
			tempHealth = health;
		}
		else
		{
			invinTime -= Time.deltaTime;
		}
		if(invinTime > 0)
		{
			health = tempHealth;
		}

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

      	// Holy Light Ability
      	if(Input.GetKey("e") && cdHolyLight <= 0 && health <= maxHealth)
      	{
      		health += holyLightHealingStrength;
      		Instantiate(HolyLightVFX, transform.position, Quaternion.identity);
      		cdHolyLight = holyLightCoolDown;
      	}
      	else
      	{
      		cdHolyLight -= Time.deltaTime;
      	}
      	// Make sure player does not have more than max health
      	if(health > maxHealth)
      		health = maxHealth;
      	


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
