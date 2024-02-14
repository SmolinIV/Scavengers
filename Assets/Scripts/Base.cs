using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WoodBoardCounter))]

public class Base : MonoBehaviour
{
    [SerializeField] private List<Scavenger> _scavengers;
    [SerializeField] private WoodBoardsSpawner _woodBoardsSpawner;

    private WoodBoardCounter _woodBoardCounter;
    private Queue<WoodBoard> _woodBoards;

    private void Awake()
    {
        _woodBoardCounter = GetComponent<WoodBoardCounter>();

        _woodBoards = new Queue<WoodBoard>();
    }

    private void OnEnable()
    {
        _woodBoardsSpawner.WoodBoardSpawned += RegisterNewWoodBoard;

        foreach (Scavenger scavenger in _scavengers)
        {
            scavenger.AlreadyFree += SendForNextWoodBoard;
            scavenger.AlreadyFree += IncreaseWoodBoardCount;
        }
    }

    private void OnDisable()
    {
        _woodBoardsSpawner.WoodBoardSpawned -= RegisterNewWoodBoard;

        foreach (Scavenger scavenger in _scavengers)
        {
            scavenger.AlreadyFree -= SendForNextWoodBoard;
            scavenger.AlreadyFree -= IncreaseWoodBoardCount;
        }
    }

    private void RegisterNewWoodBoard(WoodBoard woodBoard)
    {
        if (FindFreeScavenger(out Scavenger scavenger))
            scavenger.MoveToWoodBoard(woodBoard);
        else
            _woodBoards.Enqueue(woodBoard);
    }

    private void SendForNextWoodBoard()
    {
        if (_woodBoards.Count == 0)
            return;

        if (FindFreeScavenger(out Scavenger scavenger))
            scavenger.MoveToWoodBoard(_woodBoards.Dequeue());
    }

    private bool FindFreeScavenger(out Scavenger freeScavenger)
    {
        freeScavenger = null;

        foreach (Scavenger scavenger in _scavengers)
        {
            if (scavenger.IsFree)
            {
                freeScavenger = scavenger;
                return true;
            }
        }

        return false;
    }

    private void IncreaseWoodBoardCount()
    {
        _woodBoardCounter.IncreaseCount();
    }
}