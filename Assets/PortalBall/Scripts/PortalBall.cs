using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBall : MonoBehaviour
{
    public float maxScale;
    public float expandSpeed = 1;

    private float _dropTime;
    private Rigidbody _ballRigidbody;
    private bool _isDropped;
    
    // Start is called before the first frame update
    void Start()
    {
        _ballRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(_isDropped) UpdateScale();
    }

    void UpdateScale()
    {
        transform.localScale = Vector3.one * (maxScale * Mathf.InverseLerp(0, maxScale, (Time.time - _dropTime) * expandSpeed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        _dropTime = Time.time;
        _ballRigidbody.isKinematic = true;
        _isDropped = true;
    }
}
