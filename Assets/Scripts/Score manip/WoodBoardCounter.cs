using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBoardCounter : MonoBehaviour
{
    public Action<int> WoodBoardCountChanged;

    public int WoodBoardCount { get; private set; }

    private void Start()
    {
        WoodBoardCount = 0;
    }

    public void IncreaseCount()
    {
        ++WoodBoardCount;
        WoodBoardCountChanged?.Invoke(WoodBoardCount);
    }

    public void DecreaseCountByNumber(int number)
    {
        WoodBoardCount -= number;
        WoodBoardCountChanged?.Invoke(WoodBoardCount);
    }
}
