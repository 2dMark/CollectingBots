using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class BotCollisionHandler : MonoBehaviour
{
    private Collider _collider;

    public event Action<Base> WarehouseReached;
    public event Action<Resource> ResourceReached;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Base closerBase))
            WarehouseReached?.Invoke(closerBase);
        else if (collider.TryGetComponent(out Resource resource))
            ResourceReached?.Invoke(resource);
    }
}