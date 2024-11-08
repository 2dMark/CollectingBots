using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Destroyed;
    
    public void Destroy() => Destroyed?.Invoke(this);
}