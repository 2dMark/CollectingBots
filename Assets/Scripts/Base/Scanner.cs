using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanDelay = 2f;
    [SerializeField] private float _scanRadius = 60f;

    public event Action<List<Resource>> ResourcesDetected;

    private void OnEnable()
    {
        StartCoroutine(Scanning());
    }

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);
        List<Resource> resources = new();

        foreach (Collider collider in colliders)
            if (collider.TryGetComponent(out Resource resource))
                resources.Add(resource);

        if (resources.Count > 0)
            ResourcesDetected?.Invoke(resources);
    }
}