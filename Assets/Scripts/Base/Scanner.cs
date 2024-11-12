using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Scanner : MonoBehaviour
{
    [SerializeField, Min(0)] private float _scanDelay = 2f;
    [SerializeField, Min(0)] private float _scanRadius = 60f;

    private ResourceDatabase _resourceDatabase;

    public event Action<List<Resource>> ResourcesDetected;

    private void OnEnable()
    {
        StartCoroutine(Scanning());
    }

    public void SetResourceDatabase(ResourceDatabase resourceDatabase) =>
        _resourceDatabase = resourceDatabase;

    private IEnumerator Scanning()
    {
        WaitForSeconds scanDelay = new(_scanDelay);

        while (enabled)
        {
            yield return scanDelay;

            Scan();
        }
    }

    private void Scan()
    {
        if (_resourceDatabase == null)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);
        List<Resource> resources = new();

        foreach (Collider collider in colliders)
            if (collider.TryGetComponent(out Resource resource))
                resources.Add(resource);

        if (resources.Count > 0)
        {
            _resourceDatabase.AddResourceData(resources);
            ResourcesDetected?.Invoke(resources);
        }
    }
}