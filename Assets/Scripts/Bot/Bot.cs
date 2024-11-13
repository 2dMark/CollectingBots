using System;
using UnityEngine;

[RequireComponent(typeof(BotMovement))]
[RequireComponent(typeof(BotPicker))]
public class Bot : MonoBehaviour, ICollector
{
    [SerializeField] private BotCollisionHandler _botCollisionHandler;

    private BotMovement _botMovement;
    private BotPicker _botPicker;
    private Base _homeBase;
    private Resource _target;

    public event Action WorkCompleted;
    
    public BotStates State { get; private set; } = BotStates.Idle;

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

    public void SetHomeBase(Base homeBase) =>
        _homeBase = homeBase;

    public void SetTarget(Resource resource)
    {
        if (_homeBase == null)
            return;

        State = BotStates.Work;
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

    private void PutTarget(Base homeBase)
    {
        if (_botPicker.IsTargetReached == false)
            return;

        _botPicker.PutIn(homeBase);

        _target = null;
        State = BotStates.Idle;

        WorkCompleted?.Invoke();
    }

    private void ReturnToBase()
    {
        if (_homeBase == null)
            return;

        _botMovement.MoveTo(_homeBase.transform.position);
    }
}