using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Scavenger> _scavengers;
    [SerializeField] private WoodBoardsSpawner _woodBoardsSpawner;

    private List<WoodBoard> _woodBoards;

    private void Awake()
    {
        _woodBoards = new List<WoodBoard>();
    }

    private void OnEnable()
    {
        _woodBoardsSpawner.WoodBoardSpawned += RegisterNewWoodBoard;

        foreach (Scavenger scavenger in _scavengers)
            scavenger.AlreadyFree += SendForNextWoodBoard;
    }

    private void OnDisable()
    {
        _woodBoardsSpawner.WoodBoardSpawned -= RegisterNewWoodBoard;

        foreach (Scavenger scavenger in _scavengers)
            scavenger.AlreadyFree -= SendForNextWoodBoard;
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

    private void RegisterNewWoodBoard(WoodBoard woodBoard)
    {
        if (FindFreeScavenger(out Scavenger scavenger))
            scavenger.MoveToWoodBoard(woodBoard);
        else
            _woodBoards.Add(woodBoard);
    }

    private void SendForNextWoodBoard()
    {
        if (_woodBoards.Count == 0)
            return;

        if (FindFreeScavenger(out Scavenger scavenger))
        {

            WoodBoard woodBoard = _woodBoards.Last();
            scavenger.MoveToWoodBoard(woodBoard);
            _woodBoards.Remove(woodBoard);
        }
    }
}