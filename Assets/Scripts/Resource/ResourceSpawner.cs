using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private Collider _groundCollider;
    [SerializeField] private float _spawnDelay = 3f;
    [SerializeField] private float _spawnRadius = 15f;

    private ObjectPool<Resource> _resorcePool;
    private float _coordinateYDivisor = 2f;

    private void Awake()
    {
        _resorcePool = new(_prefab, transform);
    }

    private void OnEnable()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        WaitForSeconds spawnDelay = new(_spawnDelay);

        while (enabled)
        {
            yield return spawnDelay;

            Spawn();
        }
    }

    private void Spawn()
    {
        Resource resource = _resorcePool.GetObject();
        resource.transform.position = GetFreePosition();

        resource.Destroyed += ReturnToPool;

        resource.gameObject.SetActive(true);
    }

    private void ReturnToPool(Resource resource)
    {
        _resorcePool.PutObject(resource);

        resource.Destroyed -= ReturnToPool;
    }

    private Vector3 GetFreePosition()
    {
        Vector3 randomPosition;

        do
            randomPosition = GetRandomPosition();
        while (IsPositionFree(randomPosition) == false);

        return randomPosition;
    }

    private Vector3 GetRandomPosition()
    {
        float randomCoordinateX = Random.Range(_groundCollider.bounds.min.x, _groundCollider.bounds.max.x);
        float coordinateY = _groundCollider.bounds.center.y + (_prefab.transform.localScale.y / _coordinateYDivisor);
        float randomCoordinateZ = Random.Range(_groundCollider.bounds.min.z, _groundCollider.bounds.max.z);

        return new(randomCoordinateX, coordinateY, randomCoordinateZ);
    }

    private bool IsPositionFree(Vector3 position)
    {
        Collider[] closerObjects = Physics.OverlapSphere(position, _spawnRadius);

        foreach (Collider collider in closerObjects)
            if (collider.TryGetComponent(out ICollector _))
                return false;

        return true;
    }
}