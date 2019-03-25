using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public float distance = 45f;

    public Transform target;
    private Transform CameraTransform;

    void Start()
    {
        CameraTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraTransform.position = target.position + new Vector3(0,0,distance);
    }
}
