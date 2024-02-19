using System;
using UnityEngine;

public class NewBaseCreator : MonoBehaviour
{ 
    public Action<Flag> FlagSet;

    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private WoodBoardCounter _woodBoardCounter;
    [SerializeField] private int _newBaseCost = 5;

    private Flag _flag;

    private bool _isFlagCreated;

    public void OnDisable()
    {
        if (_flag != null)
            _flag.Set -= NotifyAboutFlagSet;
    }

    public void CreateFlagToAddNewBase()
    {
        if (!_isFlagCreated)
        {
            _isFlagCreated = true;
            _flag = Instantiate(_flagPrefab);
            _flag.Set += NotifyAboutFlagSet;
        }
    }

    private void NotifyAboutFlagSet()
    {
        FlagSet?.Invoke(_flag);
    }
}