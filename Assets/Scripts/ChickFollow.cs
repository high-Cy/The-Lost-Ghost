using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickFollow : MonoBehaviour
{
    public Rigidbody rb;
    private Transform target;
    private Vector3 direction;
    private float turnTime = 0.1f;
    private float turnSpeed;
    public bool startFollow = false;
    private float startFollowDist = 20f;
    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float dist = 5;



    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!startFollow && Vector3.Distance(target.position, transform.position) < startFollowDist)
        {
            startFollow = true;
            gameObject.AddComponent<ChickFollowAnimation>();
            gameObject.AddComponent<ChickSounds>();
        }
        if (!startFollow) return;

        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSpeed, turnTime);

        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        if (Vector3.Distance(transform.position, target.position) > dist)
        {
            transform.position = Vector3.Slerp(transform.position, target.position, Time.deltaTime * lerpSpeed);
        }
    }

    public float SelfToTargetDist()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    public float GetDist()
    {
        return dist;
    }

    public bool GetFollow()
    {
        return startFollow;
    }
}
