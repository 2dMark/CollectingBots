using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T _prefab;
    private Transform _container;
    private Queue<T> _pool;

    public ObjectPool(T prefab, Transform container)
    {
        _prefab = prefab;
        _container = container;
        _pool = new();
    }

    public T GetObject()
    {
        if (_pool.Count == 0)
        {
            T instance = Object.Instantiate(_prefab, _container);
            instance.gameObject.SetActive(false);

            return instance;
        }

        return _pool.Dequeue();
    }

    public void PutObject(T instance)
    {
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);

        instance.transform.parent = _container;
    }
}