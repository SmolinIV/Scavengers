using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceBringer : MonoBehaviour
{
    [SerializeField] private float _movingSpeed;

    private Resource _target;
    private Coroutine _movingToTarget;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Resource woodBoard))
            StopCoroutine(_movingToTarget);
    }

    private void OnDisable()
    {
        StopCoroutine(_movingToTarget);
    }

    public void SetTargetResource(Resource resource)
    {
        _target = resource;
    }

    public void StartMovingToTarget()
    {
        _movingToTarget = StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _movingSpeed * Time.deltaTime);
            yield return null;
        }

        yield break;
    }
}
