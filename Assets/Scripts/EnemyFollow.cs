using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Rigidbody rb;
    private GameObject[] enemies;
    private Transform target;
    private float turnTime = 0.1f;
    private float turnSpeed;
    private float velocity;
    private RaycastHit hit;
    private Vector3 currentDir;
    private float maxSlopeAngle = 70;
    private Vector3 rangeCenter;
    [SerializeField] private float spawnRange = 10f; // territory range
    [SerializeField] private float stopMoveRange = 10f;

    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float spaceBetweenOther = 2f;
    [SerializeField] private bool toFollow;

    // Start is called before the first frame update
    void Start()
    {
        rangeCenter = GetComponentInParent<ObjectsGenerator>().GetCenter();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        velocity = Random.Range(3f, maxVelocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    
        if (!toFollow) return;
        Vector3 direction;
        // Prevent colliding with other enemies
        foreach (GameObject obj in enemies)
        {
            if (obj != null && obj != gameObject)
            {
                float dist = Vector3.Distance(obj.transform.position, transform.position);
                if (dist <= spaceBetweenOther)
                {
                    direction = (transform.position - obj.transform.position).normalized;
                    Move(direction, 0.1f); // slow movement
                    return;
                }
            }
        }
        if (Vector3.Distance(transform.position, rangeCenter) < stopMoveRange
                && !InRange()) return;

        if (!InRange())
        {
            direction = (rangeCenter - transform.position).normalized;
        }
        else if (InRange())
        {   // follow player
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            return;
        }

        Move(direction, velocity);

    }

    private void Move(Vector3 direction, float velocity)
    {
        if (OnSlope()) direction = SlopeDirection(direction);


        if (direction.magnitude >= Mathf.Epsilon)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSpeed, turnTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            rb.AddForce(direction * velocity * transform.localScale.x - rb.velocity, ForceMode.VelocityChange);
        }
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            var angle = Vector3.Angle(Vector3.up, hit.normal);
            // cant go up too steep slopes
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 SlopeDirection(Vector3 direction)
    {
        // adjust direction to match slope
        direction = Vector3.ProjectOnPlane(direction, hit.normal).normalized;
        if (rb.velocity.magnitude > velocity)
        {
            rb.velocity = rb.velocity.normalized * velocity;
        }
        return direction;
    }

    // Checks if player in range, follow if so
    public bool InRange()
    {
        return Vector3.Distance(target.position, rangeCenter) < spawnRange;
    }

    public float SelfToTargetDist()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    public bool GetToFollow()
    {
        return toFollow;
    }
}
