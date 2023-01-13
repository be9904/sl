using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBall : MonoBehaviour
{
    public float maxScale;
    public float expandSpeed = 1;

    private Rigidbody _ballRigidbody;
    private SphereCollider _sphereCollider;
    private bool _isDropped;
    private float _dropTime;
    private bool _timeOut;
    [SerializeField] private float _timeOutTime;

    private bool isFirstTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        _ballRigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        if(_isDropped) ExpandBall();

        if (_timeOut) ShrinkBall();

        if (Time.time - _dropTime > _timeOutTime) _timeOut = true;
    }

    void ExpandBall()
    {
        transform.localScale = 
            Vector3.one * (maxScale * Mathf.InverseLerp(0, maxScale, (Time.time - _dropTime) * expandSpeed));
    }

    void ShrinkBall()
    {
        transform.localScale = 
            Vector3.one * (maxScale * Mathf.InverseLerp(maxScale, 0, (Time.time - _dropTime - _timeOutTime) * expandSpeed));
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
            }
            else
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
