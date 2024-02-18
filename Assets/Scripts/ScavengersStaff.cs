using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScavengersStaff : MonoBehaviour
{
    public Action<Scavenger> SomeScavengerGotFree;
    public Action WoodBoardBrought;

    [SerializeField] private Scavenger _scavengerPrefab;
    [SerializeField] private Transform _firstScavengerPosition;
    [SerializeField] private float _scavengerSpawnDistanceX = 5f;
    [SerializeField] private int _maxScavengerCount = 5;

    private List<Scavenger> _scavengers;


    private void Awake()
    {
        _scavengers = new List<Scavenger>();

    }

    private void OnDisable()
    {
        foreach (Scavenger scavenger in _scavengers)
        {
            scavenger.AlreadyFree -= NotifyAboutFreeScavenger;
            scavenger.WoodBoardBrought -= WoodBoardBrought;
        }
    }

    public int GetScavengersCount() => _scavengers.Count;

    public void CreateNewScavenger(out Scavenger scavenger)
    {
        scavenger = null;

        if (_scavengers.Count < _maxScavengerCount)
        {
            Vector3 newScavengerPosition;

            if (_scavengers.Count == 0)
                newScavengerPosition = _firstScavengerPosition.position;
            else
                newScavengerPosition = _scavengers.Last().transform.position + new Vector3(_scavengerSpawnDistanceX, 0, 0);

            scavenger = Instantiate(_scavengerPrefab, newScavengerPosition, new Quaternion(0, 0, 0, 0));
            scavenger.SetBasePosition(transform.position);
            _scavengers.Add(scavenger);

            scavenger.AlreadyFree += NotifyAboutFreeScavenger;
            scavenger.WoodBoardBrought += NotifyAboutBroughtWoodBoard;
        }
    }

    public bool FindFreeScavenger(out Scavenger freeScavenger)
    {
        freeScavenger = null;

        foreach (Scavenger scavenger in _scavengers)
        {
            if (scavenger.IsFree)
            {
                freeScavenger = scavenger;
                return true;
            }
        }

        return false;
    }

    private void NotifyAboutFreeScavenger(Scavenger scavenger)
    {
        SomeScavengerGotFree?.Invoke(scavenger);
    }

    private void NotifyAboutBroughtWoodBoard()
    {
        WoodBoardBrought?.Invoke();
    }
}
