using TMPro;
using UnityEngine;

public class WarehouseInfo : MonoBehaviour
{
    [SerializeField] private Warehouse _warehouse;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _warehouse.ResourcesAmountChanged += SetResourceAmount;

        SetResourceAmount(_warehouse.ResoursesAmount);
    }

    private void OnDisable()
    {
        _warehouse.ResourcesAmountChanged -= SetResourceAmount;
    }

    private void SetResourceAmount(float value) => _text.text = $"Resources: {value}";
}