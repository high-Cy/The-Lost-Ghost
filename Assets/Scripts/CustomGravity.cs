using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private float gravityScale = 10;
    private float gravity = -9.81f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 newGravity = gravity * gravityScale * Vector3.up;
        rb.AddForce(newGravity, ForceMode.Acceleration);
    }
}