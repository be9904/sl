using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldShift : MonoBehaviour
{
    [SerializeField] private GameObject nearPlane;
    [SerializeField] private GameObject farPlane;

    private Camera _playerCam;
    
    private bool _isBallActive;
    private Vector3 _explosionPoint;
    private Vector3 _playerToExplosion;
    private Vector3 _grabPoint;
    private Vector3 _playerToGrabPoint;

    // Start is called before the first frame update
    void Start()
    {
        _playerCam = Camera.main;
        nearPlane.SetActive(false);
        farPlane.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBallActive)
        {
            UpdatePlayerToExplosion();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable"))
        {
            if (other.transform.localScale.magnitude > other.GetComponent<PortalBall>().maxScale - 1)
            {
                nearPlane.SetActive(true);
                other.gameObject.SetActive(false);
            }
        }
    }

    // update player cam position to explosion point
    public void UpdatePlayerToExplosion()
    {
        _playerToExplosion = _playerCam.transform.position - _explosionPoint;
    }

    // Call this function on explosion event
    public void Explosion(Vector3 explosionPoint)
    {
        _explosionPoint = explosionPoint;
    }
    
    // Call this function on world grab event
    public void WorldGrab(Vector3 handPos)
    {
        _isBallActive = true;
        _grabPoint = handPos;
        nearPlane.SetActive(false);
        farPlane.SetActive(true);
    }
}
