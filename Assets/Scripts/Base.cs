using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WoodBoardCounter))]
[RequireComponent(typeof(ScavengersStaff))]

public class Base : MonoBehaviour
{
    [SerializeField] private WoodBoardsSpawner _woodBoardsSpawner;
    [SerializeField] private int _startScavengerCount = 2;
    [SerializeField] private int _scavengerCost = 3;

    private ScavengersStaff _scavengersStaff;
    private NewBaseCreator _newBaseCreator;

    private WoodBoardCounter _woodBoardCounter;
    private Queue<WoodBoard> _woodBoards;


    private bool _isNeedToBuildNewBase;

    private void Awake()
    {
        _woodBoardCounter = GetComponent<WoodBoardCounter>();
        _scavengersStaff = GetComponent<ScavengersStaff>();
        _newBaseCreator = GetComponent<NewBaseCreator>();

        _woodBoards = new Queue<WoodBoard>();

        for (int i = 0; i < _startScavengerCount; i++)
            _scavengersStaff.CreateNewScavenger(out Scavenger scavenger);

        _isNeedToBuildNewBase = false;
    }

    private void OnEnable()
    {
        _woodBoardsSpawner.WoodBoardSpawned += RegisterNewWoodBoard;
        _scavengersStaff.SomeScavengerGotFree += GiveTaskToFreeScavenger;
        _scavengersStaff.WoodBoardBrought += IncreaseWoodBoardCount;
        _newBaseCreator.FlagSet += ChangePriorityToCreateNewBase;
    }

    private void OnDisable()
    {
        _woodBoardsSpawner.WoodBoardSpawned -= RegisterNewWoodBoard;
        _scavengersStaff.SomeScavengerGotFree -= GiveTaskToFreeScavenger;
        _scavengersStaff.WoodBoardBrought -= IncreaseWoodBoardCount;
        _newBaseCreator.FlagSet -= ChangePriorityToCreateNewBase;
    }

    public void AddNewScavenger()
    {
        if (_woodBoardCounter.WoodBoardCount >= _scavengerCost)
        {
            _scavengersStaff.CreateNewScavenger(out Scavenger scavenger);

            if (scavenger != null)
            {
                GiveTaskToFreeScavenger(scavenger);
                _woodBoardCounter.DecreaseCountByNumber(_scavengerCost);
            }
        }
    }

    public void AcceptActiveScavenger(Scavenger scavenger)
    {
        _scavengersStaff.AddNewActiveScavenger(scavenger);
    }

    public void SetFlagForNewBase()
    {
        _newBaseCreator.SetFlag();
    }

    private void RegisterNewWoodBoard(WoodBoard woodBoard)
    {
        if (_scavengersStaff.FindFreeScavenger(out Scavenger scavenger))
            scavenger.MoveToWoodBoard(woodBoard);
        else
            _woodBoards.Enqueue(woodBoard);
    }

    private void GiveTaskToFreeScavenger(Scavenger scavenger)
    {
        if (!scavenger.IsFree)
            return;

        if (_isNeedToBuildNewBase && _newBaseCreator.TryCreateNewBase(scavenger))
            _scavengersStaff.RemoveScavenger(scavenger);
        else if (_woodBoards.Count > 0)
            scavenger.MoveToWoodBoard(_woodBoards.Dequeue());
    }

    private void IncreaseWoodBoardCount()
    {
        _woodBoardCounter.IncreaseCount();
    }

    private void ChangePriorityToCreateNewBase()
    {
        _isNeedToBuildNewBase = true;
    }
}