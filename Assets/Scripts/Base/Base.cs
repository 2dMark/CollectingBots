using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(Warehouse))]
[RequireComponent(typeof(BotsSpawner))]

public class Base : MonoBehaviour, ICollector
{
    [SerializeField, Min(0)] private int StarterBotsAmount = 3;

    private Scanner _scanner;
    private Warehouse _warehouse;
    private BotsSpawner _botSpawner;
    private Dictionary<Bot, BotStates> _botsStates;
    private Dictionary<Resource, ResourceStates> _foundedResourcesStates;

    public enum BotStates
    {
        Idle,
        Work,
    }

    public enum ResourceStates
    {
        Unassigned,
        Assigned,
    }

    private void Awake()
    {
        _foundedResourcesStates = new();

        _scanner = GetComponent<Scanner>();
        _botSpawner = GetComponent<BotsSpawner>();
        _warehouse = GetComponent<Warehouse>();

        _botSpawner.Initialize(StarterBotsAmount, out _botsStates);
    }

    private void OnEnable()
    {
        _scanner.ResourcesDetected += AddFoundedResources;

        foreach (Bot bot in _botsStates.Keys)
            bot.WorkCompleted += SetIdleState;

        StartCoroutine(BaseWork());
    }

    private void OnDisable()
    {
        _scanner.ResourcesDetected -= AddFoundedResources;

        foreach (Bot bot in _botsStates.Keys)
            bot.WorkCompleted -= SetIdleState;
    }

    private IEnumerator BaseWork()
    {
        WaitUntil waitUntil = new(() =>
            _foundedResourcesStates.ContainsValue(ResourceStates.Unassigned) &&
            _botsStates.ContainsValue(BotStates.Idle));

        while (enabled)
        {
            yield return waitUntil;

            if (TryGetIdleBot(out Bot bot))
            {
                if (TryGetCloserResource(bot, out Resource resource))
                {
                    _botsStates[bot] = BotStates.Work;
                    _foundedResourcesStates[resource] = ResourceStates.Assigned;

                    bot.SetTarget(resource);
                }
            }
        }
    }

    public void PutResourceOnWarehouse(Resource resource)
    {
        _warehouse.PutResource(resource);
        _foundedResourcesStates.Remove(resource);
    }

    private void AddFoundedResources(List<Resource> resources)
    {
        foreach (Resource resource in resources)
            _foundedResourcesStates.TryAdd(resource, ResourceStates.Unassigned);
    }

    private void SetIdleState(Bot bot) => _botsStates[bot] = BotStates.Idle;

    private bool TryGetIdleBot(out Bot idleBot)
    {
        idleBot =
            (from bot in _botsStates.Keys
             where _botsStates[bot] == BotStates.Idle
             select bot).FirstOrDefault();

        if (idleBot == null)
            return false;

        return true;
    }

    private bool TryGetCloserResource(Bot bot, out Resource closerResource)
    {
        closerResource =
            (from resource in _foundedResourcesStates.Keys
             where _foundedResourcesStates[resource] == ResourceStates.Unassigned
             orderby Vector3.Distance(bot.transform.position, resource.transform.position)
             select resource).FirstOrDefault();

        if (closerResource == null)
            return false;

        return true;
    }
}