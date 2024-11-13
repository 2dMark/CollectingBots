using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(Warehouse))]
[RequireComponent(typeof(BotsSpawner))]
public class Base : MonoBehaviour, ICollector
{
    [SerializeField, Min(0)] private int _starterBotsAmount = 3;
    [SerializeField] private ResourceDatabase _resourceDatabase;

    private Scanner _scanner;
    private Warehouse _warehouse;
    private BotsSpawner _botSpawner;
    private List<Bot> _bots;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _botSpawner = GetComponent<BotsSpawner>();
        _warehouse = GetComponent<Warehouse>();

        _scanner.SetResourceDatabase(_resourceDatabase);
        _botSpawner.Spawn(_starterBotsAmount, out _bots);
    }

    private void OnEnable()
    {
        StartCoroutine(BaseWork());
    }

    public void PutResourceOnWarehouse(Resource resource) =>
        _warehouse.PutResource(resource);

    private IEnumerator BaseWork()
    {
        WaitUntil waitUntil = new(() =>
        _bots.Any(bot => bot.State == BotStates.Idle) &&
        _resourceDatabase.IsResourceUnassigned);

        while (enabled)
        {
            yield return waitUntil;

            if (TryGetIdleBot(out Bot bot))
                if (_resourceDatabase.TryGetCloserResource(bot, out Resource resource))
                    bot.SetTarget(resource);
        }
    }

    private bool TryGetIdleBot(out Bot idleBot)
    {
        idleBot = _bots.FirstOrDefault(bot => bot.State == BotStates.Idle);

        return idleBot != null;
    }
}