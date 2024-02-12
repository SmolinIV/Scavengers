using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Scavenger> _scavengers;
    [SerializeField] private WoodBoardsSpawner _woodBoardsSpawner;

    private List<KeyValuePair<Scavenger, WoodBoard>> _busyScavengers;
    private List<WoodBoard> _woodBoards;

    private void OnEnable()
    {
        _woodBoardsSpawner.WoodBoardSpawned += RegisterNewWoodBoard;
    }

    private void OnDisable()
    {
        _woodBoardsSpawner.WoodBoardSpawned -= RegisterNewWoodBoard;        
    }

    private void Start()
    {
        _busyScavengers = new List<KeyValuePair<Scavenger, WoodBoard>>();

        for (int i = 0; i < _scavengers.Count; i++)
            _busyScavengers.Add(new KeyValuePair<Scavenger, WoodBoard>(_scavengers[i], null));
    }

    private void RegisterNewWoodBoard(WoodBoard woodBoard)
    {
        for(int i = 0; i < _scavengers.Count; ++i)
        {
            if (_busyScavengers[i].Value == null)
            {
                _busyScavengers[i] = new KeyValuePair<Scavenger, WoodBoard>(_busyScavengers[i].Key, woodBoard);
                _busyScavengers[i].Key.MoveToWoodBoard(woodBoard);
                return;
            }
        }
    }
}