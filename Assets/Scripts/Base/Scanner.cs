using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Scanner : MonoBehaviour
{
    [SerializeField, Min(0)] private float _scanDelay = 1f;

    private ResourceDatabase _resourceDatabase;
    private Collider _groundCollider;

    public event Action<List<Resource>> ResourcesDetected;

    private void Awake()
    {
        GetGroundCollider();
    }

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

    private void GetGroundCollider()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            if (hit.collider.TryGetComponent(out Ground _))
                _groundCollider = hit.collider;
    }

    private void Scan()
    {
        if (_resourceDatabase == null || _groundCollider == null)
            return;

        Collider[] colliders =
            Physics.OverlapBox(_groundCollider.bounds.center, _groundCollider.bounds.extents);
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