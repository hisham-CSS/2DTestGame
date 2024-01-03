using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform target;
    private Vector3 vel = Vector3.zero;
    //per unity's documentation - camera movement should be done in LateUpdate
    private void LateUpdate()
    {
        Vector3 targetpos = target.position + offset;
        targetpos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref vel, damping);
    }
}
