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
            scavenger.WoodBoardBrought -= NotifyAboutBroughtWoodBoard;
        }
    }

    public int GetScavengersCount() => _scavengers.Count;

    public void CreateNewScavenger(out Scavenger scavenger)
    {
        scavenger = null;

        if (_scavengers.Count < _maxScavengerCount)
        {
            Vector3 newScavengerPosition = _firstScavengerPosition.position;

            if (_scavengers.Count != 0)
                newScavengerPosition.x += _scavengerSpawnDistanceX * _scavengers.Count;

            scavenger = Instantiate(_scavengerPrefab, newScavengerPosition, new Quaternion(0, 0, 0, 0));
            scavenger.transform.SetParent(this.transform);
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

    public void AddNewActiveScavenger(Scavenger scavenger)
    {
        _scavengers.Add(scavenger);
        NotifyAboutFreeScavenger(scavenger);

        Vector3 newScavengerPosition = _firstScavengerPosition.position;

        if (_scavengers.Count != 0)
            newScavengerPosition.x += _scavengerSpawnDistanceX * _scavengers.Count;

        scavenger.transform.SetParent(this.transform);
        scavenger.SetBasePosition(newScavengerPosition);

        scavenger.AlreadyFree += NotifyAboutFreeScavenger;
        scavenger.WoodBoardBrought += NotifyAboutBroughtWoodBoard;
    }

    public void RemoveScavenger(Scavenger scavenger)
    {
        return;
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