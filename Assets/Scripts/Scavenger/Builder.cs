using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class Builder : MonoBehaviour
{
    public readonly string AnimFreeRunningPermit = "RunFree";

    public Action<Base> NewBaseCreated;

    [SerializeField] private Base _basePrefab;

    private NavMeshAgent _agent;
    private Animator _animator;

    private Flag _flag;

    private Coroutine _distanceControling;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        if (_distanceControling != null)
            StopCoroutine( _distanceControling );
    }

    public void MoveToBuildNewBase(Flag flag)
    {
        _flag = flag;

        _agent.SetDestination(flag.transform.position);
        _animator.SetBool(AnimFreeRunningPermit, true);

        _distanceControling = StartCoroutine(ControlDistanceToFlag());
    }

    private void BuildNewBase()
    {
        Vector3 buildingPosition = _flag.transform.position;

        _flag.gameObject.SetActive(false);
        gameObject.SetActive(false);

        Base newBase = Instantiate(_basePrefab, buildingPosition, new Quaternion(0,0,0,0));

        Destroy(gameObject);
    }

    private IEnumerator ControlDistanceToFlag()
    {
        float difference = 10f;

        while (Vector3.Distance(transform.position, _flag.transform.position) > difference)
            yield return null;


        BuildNewBase();
        yield break;
    }
}
