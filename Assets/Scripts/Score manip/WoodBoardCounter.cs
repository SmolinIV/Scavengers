using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBoardCounter : MonoBehaviour
{
    public Action WoodBoardCountChanged;
    private int _woodBoardCount;

    private void Start()
    {
        _woodBoardCount = 0;
    }

    public void GetWoodBoard()
    {
        ++_woodBoardCount;
    }
}
