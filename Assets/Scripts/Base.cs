using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Scavenger> _scavengers;
    [SerializeField] private WoodBoardsSpawner _woodBoardsSpawner;

    private List<KeyValuePair<Scavenger, WoodBoard>> _busyScavengers;

    private void Start()
    {
        _busyScavengers = new List<KeyValuePair<Scavenger, WoodBoard>>();

        for (int i = 0; i < _scavengers.Count; i++)
            _busyScavengers.Add(new KeyValuePair<Scavenger, WoodBoard>(_scavengers[i], null));
    }

    private void Update()
    {
        
    }
}