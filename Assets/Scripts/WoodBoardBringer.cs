using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WoodBoardBringer : MonoBehaviour
{
    public Action WoodBoardBrought;

    [SerializeField] private float _movingSpeed;

    private WoodBoard _target;
    private NavMeshAgent _agent;

    private Vector3 _basePosition;

    private bool _isWithWoodBoard;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out WoodBoard woodBoard))
        {
            _agent.isStopped = true;
            BringWoodBoardToBase();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Base mainBase) && _isWithWoodBoard)
        {
            _agent.isStopped = true;
            _isWithWoodBoard = false;
            _target.transform.SetParent(null);

            WoodBoardBrought?.Invoke();
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _basePosition = transform.parent.position;
        _isWithWoodBoard = false;
    }

    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        _target = woodBoard;
        _agent.SetDestination(_target.transform.position);
    }

    private void BringWoodBoardToBase()
    {
        Vector3 woodBoardInHandsPosition = new Vector3(0, 6, -3);

        _target.transform.SetParent(transform);
        _target.StopAnimation();
        _target.transform.localPosition = woodBoardInHandsPosition;  

        _isWithWoodBoard = true;
        _agent.SetDestination(_basePosition);
        _agent.isStopped = false;
    }
}
