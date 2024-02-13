using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bringer : MonoBehaviour
{
    public Action WoodBoardBrought;

    private WoodBoard _target;
    private NavMeshAgent _agent;

    private Vector3 _defaultPosition;
    private Vector3 _basePosition;

    private bool _isWithWoodBoard;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _basePosition = transform.parent.position;
        _defaultPosition = transform.position;
        _isWithWoodBoard = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out WoodBoard woodBoard))
        {
            if (woodBoard.gameObject == _target.gameObject)
            {
                _agent.velocity = Vector3.zero;
                BringWoodBoardToBase();
            }
        }
        else if (collision.TryGetComponent(out Base mainBase) && _isWithWoodBoard)
        {
            _agent.velocity = Vector3.zero;
            ThrowWoodBoard();
            ReturnToStartPosition();
        }
    }


    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        _target = woodBoard;
        _agent.SetDestination(_target.transform.position);
    }

    public void ReturnToStartPosition()
    {
        _agent.SetDestination(_defaultPosition);
    }

    private void BringWoodBoardToBase()
    {
        Vector3 woodBoardInHandsPosition = new Vector3(0, 1, 0.5f);

        _target.transform.SetParent(transform);
        _target.PutOnHands();
        _target.transform.localPosition = woodBoardInHandsPosition;

        _agent.SetDestination(_basePosition);

        _isWithWoodBoard = true;
    }

    private void ThrowWoodBoard()
    {
        _target.transform.SetParent(null);
        _target.Disable();
        _isWithWoodBoard = false;

        WoodBoardBrought?.Invoke();
    }
}
