using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float followForce;

    void LateUpdate()
    {
        Vector3 pos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, pos, followForce * Time.deltaTime);
        transform.position = smoothedPos;
    }

}
