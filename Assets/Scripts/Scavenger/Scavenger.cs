using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Bringer))]
[RequireComponent (typeof(Builder))]

public class Scavenger : MonoBehaviour
{
    public event Action<Scavenger> AlreadyFree;
    public event Action WoodBoardBrought;

    private Bringer _bringer;
    private Builder _builder;

    private Vector3 _basePosition;
    public Vector3 FreeIdlePosition {  get; private set; }
    public bool IsFree { get; private set; }

    private void Awake()
    {
        IsFree = true;

        FreeIdlePosition = transform.position;
    }

    private void OnEnable()
    {
        _bringer = GetComponent<Bringer>();
        _builder = GetComponent<Builder>();

        _bringer.WoodBoardBrought += NotifyAboutBroughtWoodBoard;
        _bringer.TargetMissed += StopWorking;
        _bringer.WoodBoardTaken += BringWoodBoardToBase;
        _builder.BecomeFree += StopWorking;
    }

    private void OnDisable()
    {
        _bringer.WoodBoardBrought -= NotifyAboutBroughtWoodBoard;
        _bringer.TargetMissed -= StopWorking;
        _bringer.WoodBoardTaken -= BringWoodBoardToBase;
        _builder.BecomeFree -= StopWorking;
    }

    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        if (woodBoard == null) 
            return;

        IsFree = false;
        _bringer.MoveToWoodBoard(woodBoard);
    }

    public void BringWoodBoardToBase()
    {
        _bringer.BringWoodBoardToBase(_basePosition);
    }

    public void MoveToBuildNewBase(Flag flag)
    {
        IsFree = false;
        _builder.MoveToBuildNewBase(flag);
    }

    public void StopWorking()
    {
        IsFree = true;
        AlreadyFree?.Invoke(this);

        if (IsFree)
            _bringer.ReturnToFreeIdlePosition(FreeIdlePosition);
    }

    public void DetermineBasePosition(Vector3 position)
    {
        _basePosition = position;
    }

    public void SetFreeIdlePosition(Vector3 position)
    {
        FreeIdlePosition = position;
    }

    private void NotifyAboutBroughtWoodBoard()
    {
        WoodBoardBrought?.Invoke();
        StopWorking();
    }
}