using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float CoolDownAmount = 1;
    public float attackRange;
    public float attackRangeDoubleClick;
    public int damage;
    public int damageDoubleClick;
    public bool singleMove;
    public bool doubleMove;
    public Animator playerAnimator;

    private float timeSinceLastClick;
	private const float DOUBLE_CLICK_TIME = 0.4f;
	private float lastClickTime;
    private float cd = 0;

    //[HideInInspector]
    //private bool hasDoubleAttacked = false;

    void Update()
    {
        // Initiate attack based off Cool Down first
        if(cd <= 0)
        {

        	// first attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                
                //start attack anim
                playerAnimator.SetBool("isAttack", true);

                singleMove = true; // set to true to enable movement for first attack
                //lastClickTime = Time.time;
                timeSinceLastClick = Time.time - lastClickTime;
            	
                // Use this to trigger player animation:
                // playerAnim.SetTrigger("attack");

            	// First wave of attack
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                   	enemiesToDamage[i].GetComponent<BasicEnemy>().TakeDamage(damage);
                }

                // reset cool down
                cd = CoolDownAmount;

                // measure the last time the player clicked
                lastClickTime = Time.time;
                if (Input.GetKeyDown(KeyCode.Mouse0) && ((timeSinceLastClick - CoolDownAmount) < DOUBLE_CLICK_TIME))
                {
                    //hasDoubleAttacked = true;
                    Debug.Log("Double");
                    //double attack finish
                    playerAnimator.SetBool("isAttackDouble", true);
                    doubleMove = true;// set to true to enable movement for second attack
                    Collider2D[] enemiesToDamageDA = Physics2D.OverlapCircleAll(attackPos.position, attackRangeDoubleClick, whatIsEnemies);
                    for (int i = 0; i < enemiesToDamageDA.Length; i++)
                    {
                        enemiesToDamageDA[i].GetComponent<BasicEnemy>().TakeDamage(damageDoubleClick);
                    }
                }
                if(((timeSinceLastClick - CoolDownAmount) > DOUBLE_CLICK_TIME))
                {
                    playerAnimator.SetBool("isAttackLonely", true);
                }
            }
            else
            {
                playerAnimator.SetBool("isAttack", false);
                playerAnimator.SetBool("isAttackDouble", false);
                playerAnimator.SetBool("isAttackLonely", false);
                //hasDoubleAttacked = false;
            }

        }
        else
        {
            // after cool down is set, start counting down in real seconds.
            cd -= Time.deltaTime;
        }
    }

    // Used to visually represent the size of the collision circle spawned.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // is located at game object position and is modified with the circle radius,
        // "attack range".
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
