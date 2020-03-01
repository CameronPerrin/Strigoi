using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipParticle_Instantiation : MonoBehaviour
{
	public float xScale = 1;

	private GameObject player;
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
    		theScale.x = xScale;
    	}
    	else
    		theScale.x = -xScale;

    	transform.localScale = theScale; 
    }
}
