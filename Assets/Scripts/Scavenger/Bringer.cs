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

    public Action WoodBoardBrought;
    public Action TargetMissed;

    private NavMeshAgent _agent;
    private Animator _animator;
    private WoodBoard _target;

    private Vector3 _defaultPosition;
    private Vector3 _basePosition;

    private bool _isWithWoodBoard;
    private bool _isFree;

    private Coroutine _returningControl;
    private Coroutine _targetControl;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _defaultPosition = transform.position;
        _isWithWoodBoard = false;
        _isFree = true;
    }

    public void OnDisable()
    {
        if (_returningControl != null)
            StopCoroutine(_returningControl);

        if (_targetControl != null)
            StopCoroutine(_targetControl);
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
        }
    }

    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        _isFree = false;
        _target = woodBoard;

        _targetControl = StartCoroutine(CheckTargetRelevance());
        _agent.SetDestination(_target.transform.position);
        _animator.SetBool(AnimFreeRunningPermit, true);
    }

    public void ReturnToStartPosition()
    {
        _isFree = true;

        _agent.SetDestination(_defaultPosition);
        _returningControl = StartCoroutine(ControlReturning());
    }

    public void SetBasePosition(Vector3 position)
    {
        _basePosition = position;
    }

    private void BringWoodBoardToBase()
    {
        Vector3 woodBoardInHandsPosition = new Vector3(0, 1, 0.5f);

        _target.transform.SetParent(transform);
        _target.PutOnHands();
        _target.transform.localPosition = woodBoardInHandsPosition;

        _agent.SetDestination(_basePosition);
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

    private IEnumerator ControlReturning()
    {
        float difference = 0.5f;

        while (Vector3.Distance(transform.position, _defaultPosition) > difference && _isFree)
            yield return null;


        if (_isFree)
        {
            _animator.SetBool(AnimFreeRunningPermit, false);
            _animator.SetBool(AnimRunWithWoodBoardPermit, false);
        }

        yield break;
    }

    private IEnumerator CheckTargetRelevance()
    {
        float delayInSeconds = 0.5f;
        WaitForSeconds delay = new WaitForSeconds(delayInSeconds);

        while (!_isWithWoodBoard)
        {
            if (_target.HaveParent())
            {
                TargetMissed?.Invoke();
                yield break;
            }
            else
            {
                yield return delay;
            }
        }

        yield break;
    }
}