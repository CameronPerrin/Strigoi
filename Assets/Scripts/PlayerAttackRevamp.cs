using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRevamp : MonoBehaviour
{

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.4f;
    float doubleDelay;
    public Animator playerAnimator;
    //private bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetBool("isAttack", true);
            Debug.Log("Start");
            clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked == 2 && Time.time - clicktime < clickdelay)
            {
                playerAnimator.SetBool("isAttackDouble", true);
                playerAnimator.SetBool("isAttack", false);
                clicked = 0;
                clicktime = 0;
                doubleDelay = Time.time;
                Debug.Log("DOUBLE");

            }

        }

        if ((clicked != 0) && (clicked == 2 || Time.time - clicktime > clickdelay))
        {
            Debug.Log("SINGLE");
            playerAnimator.SetBool("isAttackLonely", true);
            playerAnimator.SetBool("isAttack", false);

            clicked = 0;
        }

        if ((playerAnimator.GetBool("isAttackLonely")) && (Time.time - clicktime > 0.7f))
        {
            playerAnimator.SetBool("isAttackLonely", false);
        }

        if (playerAnimator.GetBool("isAttackDouble") && (Time.time - doubleDelay > 0.3f))
        {
            playerAnimator.SetBool("isAttackDouble", false);
            doubleDelay = 0;
        }

    }
}
