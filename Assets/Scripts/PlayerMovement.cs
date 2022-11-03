using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Camera camera;
    public CustomGravity gravity;
    public Animator animator;
    [SerializeField] private float velocity = 1f;
    [SerializeField] private float turnTime = 0.1f;
    private float turnSpeed;
    private RaycastHit hit;
    private float maxSlopeAngle = 40;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = GetComponent<CustomGravity>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        //camera = FindObjectOfType<Camera>();

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        direction = camera.transform.TransformDirection(direction);

        if (OnSlope())
        {
            // adjust direction to match slope
            direction = Vector3.ProjectOnPlane(direction, hit.normal).normalized;
            if (rb.velocity.magnitude > velocity)
            {
                rb.velocity = rb.velocity.normalized * velocity;
            }
        }

        if (direction.magnitude >= 0.1f)
        {
            animator.Play("Base Layer.Ghost Move");
            // move and rotate smoothly
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSpeed, turnTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            rb.AddForce(direction * velocity - rb.velocity, ForceMode.VelocityChange);
        } 
        else animator.Play("Base Layer.Ghost Idle");
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
}
