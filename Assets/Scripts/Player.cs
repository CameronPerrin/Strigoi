using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	private bool isDashing = false;
	public bool m_FacingRight = true;
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
	private float bfTimer;
	private float dashTrailCD;
	private Material matWhite;
	private Material matDefault;

	public Renderer playerImage;
	public Transform launchPos;	
	public Animator animator;
	public GameObject jumpDust;
	public GameObject dashTrail;
	public GameObject launchPuff;
	public GameObject footDust;
	public GameObject HolyLightVFX;
	public Image healthOrb;
	public Image button1Image;
	public Image button2Image;
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
	public float abilityBufferTimer = 0.1f; // this should always be lower than any ability CD.

	[HideInInspector]
	public bool canFlip = true;

	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	

	float gravity;
	float jumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	void Start()
	 {
		controller = GetComponent<Controller2D> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		health = maxHealth;
		matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
		matDefault = playerImage.material;
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
			Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
			Instantiate(jumpDust, pos, Quaternion.identity);
		}

		// Double jumping
		if(Input.GetKeyDown (KeyCode.Space) && jumpCD == true && !controller.collisions.below)
		{
			velocity.y = jumpVelocity;
			jumpCD = false;
			Debug.Log("Double jump");
		}
		if(!controller.collisions.below)
		{
			footDust.SetActive(false);
		}
		else
		{
			footDust.SetActive(true);
		}
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
		(controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
		
		if(Input.GetKeyDown("r"))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
		// Buffer Timer lower cd
		if(bfTimer > 0)
			bfTimer -= Time.deltaTime;	
		// Move the player when attacking + double attack combo
		if(GetComponent<PlayerAttack>().singleMove == true && m_FacingRight)
		{
			velocity.x += moveAttackRange;
			GetComponent<PlayerAttack>().singleMove = false;
		}
		else if(GetComponent<PlayerAttack>().singleMove == true && !m_FacingRight)
		{
			velocity.x -= moveAttackRange;
			GetComponent<PlayerAttack>().singleMove = false;
		}
		else if(GetComponent<PlayerAttack>().doubleMove == true && m_FacingRight)
		{
			velocity.x -= moveAttackRange;
			GetComponent<PlayerAttack>().doubleMove = false;
		}
		else if(GetComponent<PlayerAttack>().doubleMove == true && !m_FacingRight)
		{
			velocity.x -= moveAttackRange;
			GetComponent<PlayerAttack>().doubleMove = false;
		}
		//set fill amount for Dash
		button2Image.fillAmount = cdDash / dashCoolDown;
		// Dash ability
		if(Input.GetKeyDown(KeyCode.Mouse1) && cdDash <= 0 && bfTimer <= 0)
		{
			isDashing = true;
			bfTimer = abilityBufferTimer;
			invinTime = invincibilityTimer;


			dashTrail.SetActive(true);
			dashTrailCD = 0.75f;
			Instantiate(launchPuff, launchPos.position, Quaternion.identity);
			button2Image.fillAmount = 1;
			
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
			dashTrailCD -= Time.deltaTime;
		}
		// De-active the dash trail
		if(dashTrailCD <= 0)
			dashTrail.SetActive(false);


		// "I" frames from dashing
		if(isDashing)
		{ 	
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
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
			Object.Destroy(this.gameObject);
      	}
	
		// Change cd based off of cool down
		button1Image.fillAmount = cdHolyLight / holyLightCoolDown;
      	// Holy Light Ability
      	if(Input.GetKey("e") && cdHolyLight <= 0 && bfTimer <= 0)
      	{
      		bfTimer = abilityBufferTimer;
		health += holyLightHealingStrength;
      		Instantiate(HolyLightVFX, transform.position, Quaternion.identity);
      		cdHolyLight = holyLightCoolDown;
		button1Image.fillAmount = 1;	
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
        if (canFlip) { 
    		// Switch the way the player is labelled as facing.
    		m_FacingRight = !m_FacingRight;

    		// Multiply the player's x local scale by -1.
    		Vector3 theScale = transform.localScale;
    		theScale.x *= -1;
    		transform.localScale = theScale;
		}
	}

  	// Take damage
  	public void TakeDamage(int damage)
	{
		health -= damage;
		if(invinTime > 0);
		else
		{
			playerImage.material = matWhite;
			Invoke("ResetMaterial", 0.2f);
		}
		// Use to spawn blood on hit: 
		// Instantiate(bloodVFX, transform.position, Quaternion,identity);
	}

	public void ResetMaterial()
	{
		playerImage.material = matDefault;
	}

}
