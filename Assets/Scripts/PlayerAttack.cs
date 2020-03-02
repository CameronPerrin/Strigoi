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
	private float clickTime = 0;
    private float cd = 0;

    void Update()
    {
        // Initiate attack based off Cool Down first
        if (cd <= 0)
        {

        	// first attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                //start attack anim
                playerAnimator.SetBool("isAttack", true);

                singleMove = true; // set to true to enable movement for first attack

                // First wave of attack
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                   	enemiesToDamage[i].GetComponent<BasicEnemy>().TakeDamage(damage);
                }

                // reset cool down
                cd = CoolDownAmount;

                if (Input.GetKeyDown(KeyCode.Mouse0)) //double click succeed
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
                if(true)//fail double click
                {
                    playerAnimator.SetBool("isAttackLonely", true);
                }
            }
            else
            {
                playerAnimator.SetBool("isAttack", false);
                playerAnimator.SetBool("isAttackDouble", false);
                playerAnimator.SetBool("isAttackLonely", false);
  
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
