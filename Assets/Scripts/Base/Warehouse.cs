using System;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    private float _resourcesAmount;

    public event Action<float> ResourcesAmountChanged;

    public float ResoursesAmount => _resourcesAmount;

    public void PutResource(Resource resource)
    {
        ResourcesAmountChanged?.Invoke(++_resourcesAmount);

        resource.Destroy();
    }
}