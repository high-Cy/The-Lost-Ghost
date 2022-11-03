using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float offsetY = 5;
    [SerializeField] float offsetZ = -10;
    private Vector3 offsetVecX;
    private Vector3 offsetVecY;
    private Transform target;
    public float smoothSpeed = 10f;
    public float turnSpeed = 4.0f;
    private float minAngleX = -30f;
    private float maxAngleX = 0f;
    private float angleX = 0;
    private float angleY = 0;
    private Quaternion cameraRot;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offsetVecX = new Vector3(0, offsetY, offsetZ);
        offsetVecY = new Vector3(0, 0, offsetZ);
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            // rotation around y axis
            angleY += Input.GetAxis("Mouse X") * turnSpeed;

            // rotation around x axis
            angleX -= Input.GetAxis("Mouse Y") * turnSpeed;
            angleX = Mathf.Clamp(angleX, minAngleX, maxAngleX);
        }

        cameraRot = Quaternion.Slerp(cameraRot, Quaternion.Euler(angleX, angleY, 0), smoothSpeed);
        Vector3 camPos = target.position + cameraRot * offsetVecX;

        // Prevent cam going into objects
        RaycastHit hit;
        if (Physics.Linecast(target.position, camPos, out hit))
        {
            camPos = hit.point;
        }

        transform.position = camPos;
        transform.LookAt(target);
    }

}
