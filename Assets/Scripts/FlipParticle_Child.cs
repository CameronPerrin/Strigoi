using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipParticle_Child : MonoBehaviour
{
    public float xScale = 1;
	private GameObject player;
    private bool facingRight;

    void Start()
    {
    	player = GameObject.Find("player");
    	
    }
    
    void Update()
    {
    	facingRight = player.GetComponent<Player>().m_FacingRight;
    	Vector3 theScale = transform.localScale;
    	if(facingRight)
    	{
    		theScale.x = xScale;
    	}
    	else if(!facingRight)
    		theScale.x = -xScale;

    	transform.localScale = theScale; 
    }
}
