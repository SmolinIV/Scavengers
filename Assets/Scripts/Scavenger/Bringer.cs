using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class Bringer : MonoBehaviour
{
    public readonly string AnimFreeRunningPermit = "RunFree";
    public readonly string AnimRunWithWoodBoardPermit = "RunWithWoodBoard";

    public event Action WoodBoardTaken;
    public event Action WoodBoardBrought;
    public event Action TargetMissed;

    private NavMeshAgent _agent;
    private Animator _animator;
    private WoodBoard _target;

    private bool _isWithWoodBoard;
    private bool _isFree;

    private Coroutine _reachingStartPosition;
    private Coroutine _targetControl;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _isWithWoodBoard = false;
        _isFree = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out WoodBoard woodBoard))
        {
            if (woodBoard.gameObject == _target.gameObject)
            {
                _agent.velocity = Vector3.zero;
                WoodBoardTaken?.Invoke();
            }
        }
        else if (collision.TryGetComponent(out Base mainBase) && _isWithWoodBoard)
        {
            if (mainBase.transform == transform.parent.transform)
            {
                _agent.velocity = Vector3.zero;
                ThrowWoodBoard();
            }
        }
    }

    public void OnDisable()
    {
        if (_reachingStartPosition != null)
            StopCoroutine(_reachingStartPosition);

        if (_targetControl != null)
            StopCoroutine(_targetControl);
    }

    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        _isFree = false;
        _target = woodBoard;

        _targetControl = StartCoroutine(CheckTargetRelevance());
        _agent.SetDestination(_target.transform.position);
        _animator.SetBool(AnimFreeRunningPermit, true);
    }

    public void ReturnToFreeIdlePosition(Vector3 position)
    {
        _isFree = true;

        _agent.SetDestination(position);
        _reachingStartPosition = StartCoroutine(ReachStartPosition(position));
    }

    public void BringWoodBoardToBase(Vector3 basePosition)
    {
        Vector3 woodBoardInHandsPosition = new Vector3(0, 1, 0.5f);

        _target.transform.SetParent(transform);
        _target.PutOnHands();
        _target.transform.localPosition = woodBoardInHandsPosition;

        _agent.SetDestination(basePosition);
        _animator.SetBool(AnimRunWithWoodBoardPermit, true);

        _isWithWoodBoard = true;
    }

    private void ThrowWoodBoard()
    {
        _target.transform.SetParent(null);
        _target.Disable();
        _isWithWoodBoard = false;

        _animator.SetBool(AnimRunWithWoodBoardPermit, false);

        WoodBoardBrought?.Invoke();
    }

    private IEnumerator ReachStartPosition(Vector3 freeIdlePosition)
    {
        float difference = 0.5f;

        while (Vector3.Distance(transform.position, freeIdlePosition) > difference && _isFree)
            yield return null;


        if (_isFree)
        {
            _animator.SetBool(AnimFreeRunningPermit, false);
            _animator.SetBool(AnimRunWithWoodBoardPermit, false);
        }
    }

    private IEnumerator CheckTargetRelevance()
    {
        float delayInSeconds = 0.2f;
        WaitForSeconds delay = new WaitForSeconds(delayInSeconds);

        while (_isWithWoodBoard == false)
        {
            if (_target.Parent != null)
            {
                TargetMissed?.Invoke();
                yield break;
            }
            else
            {
                yield return delay;
            }
        }
    }
}