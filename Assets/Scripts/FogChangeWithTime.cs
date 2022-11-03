using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChangeWithTime : MonoBehaviour
{
    [SerializeField] private float startTime;
    [SerializeField] private float thresholdDensity;
    [SerializeField] private float speed;

    private float density = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Main Camera").GetComponent<GenerateFogEffect>().setFogDensity(density);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= startTime)
        {
            density += this.speed * Time.deltaTime;
            if (density >= thresholdDensity)
            {
                this.density = thresholdDensity;
            }
            GameObject.Find("Main Camera").GetComponent<GenerateFogEffect>().setFogDensity(density);
        }

    }
}
