using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChangeWithDistance : MonoBehaviour
{
    [SerializeField] private float fogDistance = 100;
    [SerializeField] private float speed;
    [SerializeField] private float thresholdDensity;
    [SerializeField] private float minDensity = 0.05f;

    private Transform playerPosition;
    private Transform goalPosition;
    private float decreasing = 0.0001f;
    private float density = 0;

    // Update is called once per frame
    void Update()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        goalPosition = GameObject.FindGameObjectWithTag("Goal").transform;
        float distance = Vector3.Distance(playerPosition.position, goalPosition.position);

        if (distance >= fogDistance)
        {
            float exactDis = distance - fogDistance;
            float rate = exactDis / fogDistance;
            decreasing = Mathf.Exp(-(5 - rate));
            //decreasing = 0.0001f + 0.0001f * rate;
            density = this.speed * exactDis * decreasing;
            if (density >= thresholdDensity)
            {
                this.density = thresholdDensity;
            }
            GameObject.Find("Main Camera").GetComponent<GenerateFogEffect>().setFogDensity(minDensity + density);
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<GenerateFogEffect>().setFogDensity(minDensity);
        }
    }
}
