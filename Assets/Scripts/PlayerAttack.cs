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

    [HideInInspector]
    public Animator playerAnimator;

    private float timeSinceLastClick;
	private const float DOUBLE_CLICK_TIME = 0.4f;
	private float clickTime = 0;
    private float cd = 0;

    void Update()
    {
        // Initiate attack based off Cool Down first
        
        	bool isAttack = playerAnimator.GetBool("isAttack");
        	

        	// first attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                singleMove = true; // set to true to enable movement for first attack

                // First wave of attack
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                   	enemiesToDamage[i].GetComponent<BasicEnemy>().TakeDamage(damage);
                }

                // reset cool down
                cd = CoolDownAmount;
                Debug.Log("ATTACK GOING THROUGH");

            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && isAttack) //double click succeed
            {

                doubleMove = true;// set to true to enable movement for second attack
                Collider2D[] enemiesToDamageDA = Physics2D.OverlapCircleAll(attackPos.position, attackRangeDoubleClick, whatIsEnemies);
                for (int i = 0; i < enemiesToDamageDA.Length; i++)
                {
                    enemiesToDamageDA[i].GetComponent<BasicEnemy>().TakeDamage(damageDoubleClick);
                }

                 Debug.Log("SECOND ATTACK HOE");
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
