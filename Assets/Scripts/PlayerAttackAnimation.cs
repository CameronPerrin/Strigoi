﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{

    // float clicked = 0;
    // float clicktime = 0;
    // float clickdelay = 0.4f;
    // float doubleDelay;
    // float attackDelay;
    // bool canDo = true;
    public Animator playerAnimator;
    //private bool waiting = false;

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.GameIsPaused == false)
        {
        if (Input.GetKeyDown(KeyCode.Mouse0) && 
            !playerAnimator.GetBool("isAttackDouble") && 
            !playerAnimator.GetBool("isAttackLonely") &&
            !playerAnimator.GetBool("isAttack"))
        {
            playerAnimator.SetBool("isAttack", true);
            //Debug.Log("Start");
/*
            clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked == 2 && Time.time - clicktime < clickdelay)
            {
                //playerAnimator.SetBool("isAttackDouble", true);
                //playerAnimator.SetBool("isAttack", false);
                clicked = 0;
                clicktime = 0;
                doubleDelay = Time.time;
                //Debug.Log("DOUBLE");

            }

            */
        }

/*
        if ((clicked != 0) && (clicked == 2 || Time.time - clicktime > clickdelay))
        {
            //Debug.Log("SINGLE");
            //playerAnimator.SetBool("isAttackLonely", true);
            //playerAnimator.SetBool("isAttack", false);

            clicked = 0;
        }

        if ((playerAnimator.GetBool("isAttackLonely")) && (Time.time - clicktime > 0.7f))
        {
            //playerAnimator.SetBool("isAttackLonely", false);
            //playerAnimator.SetBool("isAttack", false);
        }

        if (playerAnimator.GetBool("isAttackDouble") && (Time.time - doubleDelay > 0.3f))
        {
            //playerAnimator.SetBool("isAttackDouble", false);
            //playerAnimator.SetBool("isAttack", false);
            doubleDelay = 0;
        }



        //stop all movement and flipping
        //COMMIT THE PLAYER TO HIS ATTACK
        //heck UP OR NOT
        if(playerAnimator.GetBool("isAttackDouble") || playerAnimator.GetBool("isAttackLonely") || playerAnimator.GetBool("isAttack"))
        {
            GetComponent<Player>().moveSpeed = 0;
            //velocity.x += moveAttackRange;
            if(canDo){
                if(GetComponent<Player>().m_FacingRight)
                    GetComponent<Player>().velocity.x += GetComponent<Player>().moveAttackRange;
                if(!GetComponent<Player>().m_FacingRight)
                    GetComponent<Player>().velocity.x -= GetComponent<Player>().moveAttackRange;
                
                canDo = false;
            }
            
            GetComponent<Player>().canFlip = false;
        }
        else
        {
            canDo = true;
            GetComponent<Player>().moveSpeed = 7;
            GetComponent<Player>().canFlip = true;
        }
        */
    }

    }

}
