using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScavengersStaff : MonoBehaviour
{
    public event Action<Scavenger> SomeScavengerGotFree;
    public event Action WoodBoardBrought;

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
            scavenger = Instantiate(_scavengerPrefab, GetScavengerStartPosition(), new Quaternion(0,0,0,0));

            DetermineNewScavengerPosition(scavenger);
            DetermineNewScavengerSubscribes(scavenger);

            _scavengers.Add(scavenger);
        }
    }

    public void AddAlreadyCreatedScavenger(Scavenger scavenger)
    {
        DetermineNewScavengerPosition(scavenger);
        _scavengers.Add(scavenger);

        Vector3 newIdlePosition = GetScavengerStartPosition();

        scavenger.SetFreeIdlePosition(newIdlePosition);
        scavenger.transform.position = newIdlePosition;

        DetermineNewScavengerSubscribes(scavenger);
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

    public void RemoveScavenger(Scavenger scavenger)
    {
        scavenger.AlreadyFree -= NotifyAboutFreeScavenger;
        scavenger.WoodBoardBrought -= NotifyAboutBroughtWoodBoard;

        _scavengers.Remove(scavenger);
    }

    private void NotifyAboutFreeScavenger(Scavenger scavenger)
    {
        SomeScavengerGotFree?.Invoke(scavenger);
    }

    private void NotifyAboutBroughtWoodBoard()
    {
        WoodBoardBrought?.Invoke();
    }

    private Vector3 GetScavengerStartPosition()
    {
        Vector3 newScavengerPosition = _firstScavengerPosition.position;

        if (_scavengers.Count != 0)
            newScavengerPosition.x += _scavengerSpawnDistanceX * _scavengers.Count;

        return newScavengerPosition;
    }

    private void DetermineNewScavengerPosition(Scavenger scavenger)
    {
        scavenger.DetermineBasePosition(transform.position);
        scavenger.transform.SetParent(this.transform);

    }

    private void DetermineNewScavengerSubscribes(Scavenger scavenger)
    {
        scavenger.AlreadyFree += NotifyAboutFreeScavenger;
        scavenger.WoodBoardBrought += NotifyAboutBroughtWoodBoard;
    }
}