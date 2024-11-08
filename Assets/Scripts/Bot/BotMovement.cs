using System.Collections;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private bool _isMoving = false;
    private Coroutine _coroutine;

    private IEnumerator Moving(Vector3 position)
    {
        while (enabled)
        {
            transform.LookAt(position);
            transform.position = Vector3.MoveTowards(transform.position, position, _speed * Time.deltaTime);

            if (transform.position == position)
                break;

            yield return null;
        }

        _isMoving = false;
    }

    public void MoveTo(Vector3 position)
    {
        if (_isMoving)
            return;
        
        _isMoving = true;

        _coroutine = StartCoroutine(Moving(position));
    }

    public void StopMoving()
    {
        _isMoving = false;

        StopCoroutine(_coroutine);
    }
}