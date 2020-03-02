﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class BasicEnemy : MonoBehaviour 
{
	public int health;
	public Animator zombAnimator;
	public SpriteRenderer enemyImage;
	[SerializeField]
	GameObject player;
	[SerializeField]
	float agroRange;
	[SerializeField]
	float moveSpeed;
	[SerializeField]
	GameObject agroView;
	[SerializeField]
	int damage;
	[SerializeField]
	float attackRange;
	[SerializeField]
	LayerMask whatIsPlayer;
	[SerializeField]
	float attackTimer;
	
	

	
	Vector3 velocity;
	Vector3 scale;

	float gravity;
	float velocityXSmoothing;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float timeToJumpApex = .4f;
	float cd;
	float changeColor = 0;
	bool isFacingRight = true;
	
	Controller2D controller;

	void Start()
	{
		gravity = -(2 * 4) / Mathf.Pow (timeToJumpApex, 2);
		controller = GetComponent<Controller2D>();
		enemyImage = GetComponent<SpriteRenderer>();
		
	}

	void Update()
	{
		if(changeColor <= 0)
			enemyImage.color = new Color (255,255,255,255);
		else
			changeColor -= Time.deltaTime;

		scale = transform.localScale;	
		// Directional movement -- BEGINNING
		float distToPlayer = Vector2.Distance(agroView.transform.position, player.transform.position);
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
		if(distToPlayer < agroRange)
		{
			// chase the player
			ChasePlayer();
			//start walk animation
			zombAnimator.SetBool("isWalking", true);
			
		}	
		else
		{
			// stop chasing player
			StopChasingPlayer();
			//end walk anim
			zombAnimator.SetBool("isWalking", false);

		}
		float enemyDistToPlayer = Vector2.Distance(transform.position, player.transform.position);
		if(enemyDistToPlayer < 2)
		{
			StopChasingPlayer();
			zombAnimator.SetBool("isWalking", false);
		}
		


		// check for HP
		if(health <= 0)
		{
			// Destroy if under 0
			// Destroys (this current object, instantly)
			Object.Destroy(this.gameObject);
		}
		// Direction movement -- END


		// Deal damage to player -- BEGINNING
		Collider2D[] playerTakeDamage = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsPlayer);
		
		for(int i = 0; i < playerTakeDamage.Length; i++)
        {
            // First enemy measured, grabs the script associated with the enemy
			// and deals damage to them.
			if(cd <= 0)
            {
            	playerTakeDamage[i].GetComponent<Player>().TakeDamage(damage);
                cd = attackTimer;
            }
        }
        cd -= Time.deltaTime;
        // Deal damage to player -- END
	}



	private void ChasePlayer()
	{
		// Detect if player is in a certain direction from the enemy
		float targetVelocityX = moveSpeed;
		if(transform.position.x < player.transform.position.x)
		{	
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
			// Enemy flip
			if(isFacingRight != true)
			{
				scale.x *= -1;
				transform.localScale = scale;
				isFacingRight = true;
			}
		}

		else if (transform.position.x > player.transform.position.x)
		{
			velocity.x = Mathf.SmoothDamp (velocity.x, -targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
			if(isFacingRight == true)
			{
				scale.x *= -1;
				transform.localScale = scale;
				isFacingRight = false;
			}
		}
	}

	// Function to stop chasing player once they have left the aggro range
	private void StopChasingPlayer()
	{
		velocity.x = 0;
	}

	// this is where the enemy takes damage
	public void TakeDamage(int damage)
	{
		health -= damage;
		enemyImage.color = new Color (255,0,0,255);
		changeColor = 0.2f;
		// Use to spawn blood on hit: 
		// Instantiate(bloodVFX, transform.position, Quaternion,identity);
	}

	// Used to help visualize aggro range (yellow colored circle)
	void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(agroView.transform.position, agroRange);
    }
}
