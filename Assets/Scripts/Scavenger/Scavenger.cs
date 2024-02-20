using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Bringer))]
[RequireComponent (typeof(Builder))]

public class Scavenger : MonoBehaviour
{
    public Action<Scavenger> AlreadyFree;
    public Action WoodBoardBrought;

    private Bringer _bringer;
    private Builder _builder;

    public bool IsFree { get; private set; }

    private void Awake()
    {
        _bringer = GetComponent<Bringer>();
        _builder = GetComponent<Builder>();

        IsFree = true;
    }

    private void OnEnable()
    {
        _bringer.WoodBoardBrought += NotifyAboutBroughtWoodBoard;
        _bringer.TargetMissed += StopWorking;
        _builder.NewBaseCreated += JoinToBase;
    }

    private void OnDisable()
    {
        _bringer.WoodBoardBrought -= NotifyAboutBroughtWoodBoard;
        _bringer.TargetMissed -= StopWorking;
        _builder.NewBaseCreated -= JoinToBase;
    }

    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        if (woodBoard == null) 
            return;

        IsFree = false;
        _bringer.MoveToWoodBoard(woodBoard);
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
            _bringer.ReturnToStartPosition();
    }

    public void SetBasePosition(Vector3 position)
    {
        _bringer.SetBasePosition(position);
    }

    private void JoinToBase(Base newBase)
    {
        newBase.AcceptActiveScavenger(this);
    }

    private void NotifyAboutBroughtWoodBoard()
    {
        WoodBoardBrought?.Invoke();
        StopWorking();
    }
}