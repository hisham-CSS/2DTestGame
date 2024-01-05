using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 offset;

    public float damping;

    public Transform target;

    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 targetpos = target.position + offset;
        targetpos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref vel, damping);
    }
}
