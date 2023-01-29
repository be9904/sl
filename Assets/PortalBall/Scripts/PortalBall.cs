using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

    [Header("VFX")] 
    public VisualEffect blackHole;
    public float delay;
    public MeshRenderer shell;
    
    // Start is called before the first frame update
    void Start()
    {
        _ballRigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        if (_isDropped) StartCoroutine(StartExpandCoroutine());

        if (_timeOut) ShrinkBall();

        if (Time.time - _dropTime > _timeOutTime) _timeOut = true;
    }

    IEnumerator StartExpandCoroutine()
    {
        yield return new WaitForSeconds(delay);
        
        ExpandBall();
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
        if(transform.localScale.magnitude < 0.01)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _dropTime = Time.time + delay;
        _ballRigidbody.isKinematic = true;
        _sphereCollider.isTrigger = true;
        _isDropped = true;
        shell.enabled = false;
        blackHole.transform.position = transform.position;
        blackHole.Play();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            if(!other.gameObject.activeSelf)
                other.gameObject.SetActive(true);
        }
    }
}
