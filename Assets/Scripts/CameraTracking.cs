using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3 (player.transform.position.x, player.transform.position.y, -10);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, playerPos, ref velocity, smoothTime);
    }
}
