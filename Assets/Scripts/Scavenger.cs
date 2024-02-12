using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WoodBoardBringer))]
public class Scavenger : MonoBehaviour
{
    private WoodBoard _targetWoodBoard;
    private WoodBoardBringer _bringer;

    private void Start()
    {
        _bringer = GetComponent<WoodBoardBringer>();
    }

    public void MoveToWoodBoard(WoodBoard target)
    {
        if (target == null) 
            return;

        _targetWoodBoard = target;
        _bringer.MoveToWoodBoard(_targetWoodBoard);
    }
}
