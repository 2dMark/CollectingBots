using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class BotsSpawner : MonoBehaviour
{
    private const float Angle = 360f;

    [SerializeField] private Bot _prefab;
    [SerializeField] private Transform _spawnPoint;

    private Base _base;
    private float _newBotSpacing = Angle * Mathf.Deg2Rad;

    public List<Bot> Spawn(float botsAmount)
    {
        _base = GetComponent<Base>();
        List<Bot> bots = new();

        Bot newBot;
        Vector3 circularPosition;
        Vector3 newPosition;
        float radius = _prefab.transform.localScale.magnitude;

        for (int i = 1; i <= botsAmount; i++)
        {
            circularPosition = new(
                Mathf.Cos(_newBotSpacing / botsAmount * i) * radius,
                0,
                Mathf.Sin(_newBotSpacing / botsAmount * i) * radius);

            newPosition = _spawnPoint.transform.position + circularPosition;

            newBot = Instantiate(_prefab, transform);
            newBot.transform.position = newPosition;

            newBot.SetHomeBase(_base);
            newBot.gameObject.SetActive(true);
            bots.Add(newBot);
        }

        return bots;
    }
}