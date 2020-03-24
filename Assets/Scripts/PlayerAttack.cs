using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{    
    [Header("--ATTACK RANGE")]
    public float attackRange;
    public float attackRangeDoubleClick;
    [Header("--DAMAGE")]
    public int damage;
    public int damageDoubleClick;
    [Header("--ENEMY SELECTION")]
    public LayerMask whatIsEnemies;
    [Header("--ATTACK POSITION")]
    public Transform attackPos;
    [Header("--MISC.")]
    public float CoolDownAmount = 0.7f;

    [HideInInspector]
    public bool singleMove;
    [HideInInspector]
    public bool doubleMove;
    [HideInInspector]
    public Animator playerAnimator;


    private float clickTimer = 0; // use this for to delay damage after first click
    private float cd = 0;

    void Update()
    {
        // Initiate attack based off Cool Down first
        
            bool isAttack = playerAnimator.GetBool("isAttack");

        	// first attack
            //if(delayCheck < (Time.time-1)){
                        //unlock attackOne and Two
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Invoke("attackOne", 0.3f);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && isAttack) //double click succeed
                {
                    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
                    Invoke("attackTwo", 0.45f);         
                }

            //}
    }

    // Used to visually represent the size of the collision circle spawned.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // is located at game object position and is modified with the circle radius,
        // "attack range".
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    void attackOne()
    {
        // First wave of attack
        singleMove = true; // set to true to enable movement for first attack
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for(int i = 0; i < enemiesToDamage.Length; i++)
        {
             enemiesToDamage[i].GetComponent<BasicEnemy>().TakeDamage(damage);
        }
    }

    void attackTwo()
    {
        doubleMove = true;// set to true to enable movement for second attack
        Collider2D[] enemiesToDamageDA = Physics2D.OverlapCircleAll(attackPos.position, attackRangeDoubleClick, whatIsEnemies);
        for (int i = 0; i < enemiesToDamageDA.Length; i++)
        {
            enemiesToDamageDA[i].GetComponent<BasicEnemy>().TakeDamage(damageDoubleClick);
        }
    }
}
