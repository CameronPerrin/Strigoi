using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    public GameObject attackCircle;
    public GameObject player;
    private Vector3 playerPos;
    private Vector3 tempVector;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            Attack();
        } 
    }

    void Attack()
    {
        tempVector = new Vector3(playerPos.x+1, playerPos.y, playerPos.z);
        Instantiate(attackCircle, tempVector, player.transform.rotation);
    }
}
