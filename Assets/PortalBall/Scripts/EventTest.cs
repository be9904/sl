using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3>
{
    
}

public class EventTest : MonoBehaviour
{
    public Vector3UnityEvent exposedEvent;
}
