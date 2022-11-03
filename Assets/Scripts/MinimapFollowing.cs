using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollowing : MonoBehaviour
{
    private Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        Vector3 newPosition = target.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }
}
