using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipParticle : MonoBehaviour
{
	public GameObject player;
    private bool facingRight;

    void Start()
    {
    	player = GameObject.Find("player");
    	facingRight = player.GetComponent<Player>().m_FacingRight;
    }
    
    void Update()
    {
    	
    	Vector3 theScale = transform.localScale;
    	if(facingRight)
    	{
    		theScale.x = 1;
    	}
    	else
    		theScale.x = -1;

    	transform.localScale = theScale; 
    }
}
