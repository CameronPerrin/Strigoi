using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
	public float destroyTimer = 1;
 
    // Update is called once per frame
    void Update()
    {
        Object.Destroy(this.gameObject, destroyTimer);
    }
}
