using System;
using UnityEngine;

[RequireComponent(typeof(BotMovement))]
[RequireComponent(typeof(BotPicker))]

public class Bot : MonoBehaviour, ICollector
{
    [SerializeField] private BotCollisionHandler _botCollisionHandler;
    
    private BotMovement _botMovement;
    private BotPicker _botPicker;
    private Base _defaultBase;
    private Resource _target;

    public event Action<Bot> WorkCompleted;

    private void Awake()
    {
        _botMovement = GetComponent<BotMovement>();
        _botPicker = GetComponent<BotPicker>();
    }

    private void OnEnable()
    {
        _botCollisionHandler.ResourceReached += PickUpTarget;
        _botCollisionHandler.WarehouseReached += PutTarget;
    }

    private void OnDisable()
    {
        _botCollisionHandler.ResourceReached -= PickUpTarget;
        _botCollisionHandler.WarehouseReached -= PutTarget;
    }

    public void SetDefaultBase(Base defaultBase) => _defaultBase = defaultBase;

    public void SetTarget(Resource resource)
    {
        if (_defaultBase == null)
            return;

        _target = resource;

        _botMovement.MoveTo(_target.transform.position);
    }

    private void PickUpTarget(Resource resource)
    {
        if (resource != _target)
            return;

        _botPicker.PickUp(resource);

        ReturnToBase();
    }

    private void PutTarget(Base closerBase)
    {
        if (_botPicker.IsTargetReached == false)
            return;

        _botMovement.StopMoving();
        _botPicker.PutIn(closerBase);

        _target = null;

        WorkCompleted?.Invoke(this);
    }

    private void ReturnToBase()
    {
        if (_defaultBase == null)
            return;

        _botMovement.StopMoving();
        _botMovement.MoveTo(_defaultBase.transform.position);
    }
}