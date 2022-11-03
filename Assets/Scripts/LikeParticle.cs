using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikeParticle : MonoBehaviour
{
    public GameObject Like;
    private Transform playerPosition;
    private Transform goalPosition;
    
    private Vector3 offset;
    private GameObject particles;
    private bool begin = true;
    [SerializeField] private float Threshold;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        goalPosition = GameObject.FindGameObjectWithTag("Goal").transform;
        double playerX = playerPosition.position.x;
        double playerY = playerPosition.position.y;
        double playerZ = playerPosition.position.z;
        double goalX = goalPosition.position.x;
        double goalY = goalPosition.position.y;
        double goalZ = goalPosition.position.z;
        double distance = (playerX - goalX) * (playerX - goalX) +
        (playerY - goalY) * (playerY - goalY)
        + (playerZ - goalZ) * (playerZ - goalZ);

        if (distance <= Threshold && begin)
        {
            particles = Instantiate(Like, playerPosition.position + offset, Quaternion.identity);
            begin = false;
        
        }
        if(particles != null && distance <= Threshold)
        {

            particles.transform.position = playerPosition.position + offset;
        }
        
        if(particles != null && distance > Threshold)
        {
            // particles.transform.position.y = 1000;
            begin = true;
            Destroy(particles.gameObject);
        }
        
        
     
    }
}
