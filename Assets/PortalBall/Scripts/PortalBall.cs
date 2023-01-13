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
    private SphereCollider _sphereCollider;
    private bool _isDropped;

    private bool isFirstTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        _ballRigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
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
        _sphereCollider.isTrigger = true;
        _isDropped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            if (!isFirstTarget)
            {
                isFirstTarget = true;
                // Debug.Log("First Target: " + other.gameObject.name);
            }
            else
            {
                MeshRenderer renderer = other.gameObject.GetComponent<MeshRenderer>();
                if (renderer)
                {
                    // Debug.Log(other.gameObject.name + " not rendering");
                    renderer.enabled = false;
                }
            }
        }
    }
}
