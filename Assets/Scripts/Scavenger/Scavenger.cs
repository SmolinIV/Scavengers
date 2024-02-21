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

    public Vector3 FreeIdlePosition {  get; private set; }
    public Vector3 BasePosition { get; private set; }
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
        _bringer.MoveToWoodBoard(woodBoard, BasePosition);
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

    public void SetBasePosition(Vector3 position)
    {
        BasePosition = position;
    }

    public void SetFreeWaitingPosition(Vector3 position)
    {
        FreeIdlePosition = position;
    }

    private void JoinToBase(Base newBase)
    {
        newBase.AcceptActiveScavenger(this);
        gameObject.SetActive(true);
    }

    private void NotifyAboutBroughtWoodBoard()
    {
        WoodBoardBrought?.Invoke();
        StopWorking();
    }
}