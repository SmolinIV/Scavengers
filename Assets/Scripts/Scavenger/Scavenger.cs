using System;
using UnityEngine;

[RequireComponent(typeof(Bringer))]

public class Scavenger : MonoBehaviour
{
    public Action<Scavenger> AlreadyFree;
    public Action WoodBoardBrought;

    private Bringer _bringer;

    public bool IsFree { get; private set; }

    private void Awake()
    {
        _bringer = GetComponent<Bringer>();

        IsFree = true;
    }

    private void OnEnable()
    {
        _bringer.WoodBoardBrought += NotifyAboutBroughtWoodBoard;
    }

    private void OnDisable()
    {
        _bringer.WoodBoardBrought -= NotifyAboutBroughtWoodBoard;
    }

    public void MoveToWoodBoard(WoodBoard woodBoard)
    {
        if (woodBoard == null) 
            return;

        IsFree = false;
        _bringer.MoveToWoodBoard(woodBoard);
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

    private void NotifyAboutBroughtWoodBoard()
    {
        WoodBoardBrought?.Invoke();
        StopWorking();
    }
}