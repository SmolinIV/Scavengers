using System;
using UnityEngine;

[RequireComponent(typeof(Bringer))]

public class Scavenger : MonoBehaviour
{
    public Action AlreadyFree;

    private Bringer _bringer;

    public bool IsFree { get; private set; }

    private void OnEnable()
    {
        _bringer.WoodBoardBrought += StopWorking;
    }

    private void OnDisable()
    {
        _bringer.WoodBoardBrought -= StopWorking;        
    }

    private void Start()
    {
        _bringer = GetComponent<Bringer>();
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
        AlreadyFree?.Invoke();
    }
}
