using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WoodBoardCounter))]
[RequireComponent(typeof(ScavengersStaff))]

public class Base : MonoBehaviour
{
    [SerializeField] private WoodBoardsSpawner _woodBoardsSpawner;

    private ScavengersStaff _scavengersController;
    private WoodBoardCounter _woodBoardCounter;
    private Queue<WoodBoard> _woodBoards;

    private bool _isNeedToBuildNewBase;
    private void Awake()
    {
        _woodBoardCounter = GetComponent<WoodBoardCounter>();
        _scavengersController = GetComponent<ScavengersStaff>();

        _woodBoards = new Queue<WoodBoard>();

        _scavengersController.CreateNewScavenger(out Scavenger scavenger);

        _isNeedToBuildNewBase = false;
    }

    private void OnEnable()
    {
        _woodBoardsSpawner.WoodBoardSpawned += RegisterNewWoodBoard;
        _scavengersController.SomeScavengerGotFree += GiveTaskToFreeScavenger;
        _scavengersController.WoodBoardBrought += IncreaseWoodBoardCount;
    }

    private void OnDisable()
    {
        _woodBoardsSpawner.WoodBoardSpawned -= RegisterNewWoodBoard;
        _scavengersController.SomeScavengerGotFree -= GiveTaskToFreeScavenger;
        _scavengersController.WoodBoardBrought -= IncreaseWoodBoardCount;
    }

    private void RegisterNewWoodBoard(WoodBoard woodBoard)
    {
        if (_scavengersController.FindFreeScavenger(out Scavenger scavenger))
            scavenger.MoveToWoodBoard(woodBoard);
        else
            _woodBoards.Enqueue(woodBoard);
    }

    private void GiveTaskToFreeScavenger(Scavenger scavenger)
    {
        if (!scavenger.IsFree)
            return;

        if (_isNeedToBuildNewBase)
            return;
        else if (_woodBoards.Count > 0)
            scavenger.MoveToWoodBoard(_woodBoards.Dequeue());
    }

    public void AddNewScavenger()
    {
        _scavengersController.CreateNewScavenger(out Scavenger scavenger);

        GiveTaskToFreeScavenger(scavenger);  
    }

    private void IncreaseWoodBoardCount()
    {
        _woodBoardCounter.IncreaseCount();
    }
}