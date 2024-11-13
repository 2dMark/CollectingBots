using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Bot))]
public class BotMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Coroutine _coroutine;
    private Bot _bot;

    private void Awake()
    {
        _bot = GetComponent<Bot>();
    }

    private void OnEnable()
    {
        _bot.WorkCompleted += StopMoving;
    }

    private void OnDisable()
    {
        _bot.WorkCompleted -= StopMoving;
    }

    public void MoveTo(Vector3 position)
    {
        StopMoving();

        _coroutine = StartCoroutine(Moving(position));
    }

    private void StopMoving()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator Moving(Vector3 position)
    {
        while (enabled)
        {
            transform.LookAt(position);
            transform.position = Vector3.MoveTowards
                (transform.position, position, _speed * Time.deltaTime);

            if (transform.position == position)
                break;

            yield return null;
        }
    }
}