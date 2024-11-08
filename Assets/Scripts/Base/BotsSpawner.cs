using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base))]

public class BotsSpawner : MonoBehaviour
{
    [SerializeField] private Bot _prefab;
    [SerializeField] private Transform _spawnPoint;

    private const float Angle = 360f;

    private Base _base;
    private ObjectPool<Bot> _pool;
    private float _newBotSpacing = Angle * Mathf.Deg2Rad;

    public void Initialize(float starterBotsAmount, out Dictionary<Bot, Base.BotStates> bots)
    {
        _base = GetComponent<Base>();
        _pool = new(_prefab, transform);
        bots = new();

        Bot newBot;
        Vector3 circularPosition;
        Vector3 newPosition;
        float radius = _prefab.transform.localScale.magnitude;

        for (int i = 1; i <= starterBotsAmount; i++)
        {
            circularPosition = new(
                Mathf.Cos(_newBotSpacing / starterBotsAmount * i) * radius,
                0,
                Mathf.Sin(_newBotSpacing / starterBotsAmount * i) * radius);

            newPosition = _spawnPoint.transform.position + circularPosition;

            newBot = _pool.GetObject();
            newBot.transform.position = newPosition;

            newBot.SetDefaultBase(_base);
            newBot.gameObject.SetActive(true);
            bots.Add(newBot, Base.BotStates.Idle);
        }
    }
}