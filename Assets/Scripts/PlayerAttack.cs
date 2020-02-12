using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float CoolDownAmount = 1;
    public float attackRange;
    public int damage;
   
    private float cd = 0;


    // Update is called once per frame
    void Update()
    {
        // Initiate attack based off Cool Down first
        if(cd <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // Use this to trigger player animation:
                // playerAnim.SetTrigger("attack");

                // Creates a collider at player attack position (which is created outside the
                // the script as a gameObject). Range is the radius of the circle. What is Enemies
                // detects a certain type of object as an enemy.
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                // Loop for each enemy detected, this could be modified to fit only a certain amount
                // at a time. Ex 4 enemies when it detected 5.
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    // First enemy measured, grabs the script associated with the enemy
                    // and deals damage to them.
                    enemiesToDamage[i]

                    .GetComponent<BasicEnemy>().TakeDamage(damage);
                }
                // reset cool down
                cd = CoolDownAmount;
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
