using System;
using UnityEngine;

public class NewBaseCreator : MonoBehaviour
{ 
    public Action FlagSet;

    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private PhantomBase _phantomBasePrefab;
    [SerializeField] private WoodBoardCounter _woodBoardCounter;
    [SerializeField] private int _newBaseCost = 5;

    private Flag _newBaseFlag;
    private PhantomBase _phantomBase;

    private bool _isFlagCreated;

    public void OnDisable()
    {
        if (_newBaseFlag != null)
            _newBaseFlag.Set -= NotifyAboutFlagSet;
    }

    public void SetFlag()
    {
        if (!_isFlagCreated)
        {
            _isFlagCreated = true;
            _newBaseFlag = Instantiate(_flagPrefab);
            _newBaseFlag.Set += NotifyAboutFlagSet;

            _phantomBase = Instantiate(_phantomBasePrefab);
            _phantomBase.transform.SetParent(_newBaseFlag.transform);
            _phantomBase.transform.localPosition = Vector3.zero;
        }
    }

    public bool TryCreateNewBase(Scavenger scavenger)
    {
        if (_woodBoardCounter.WoodBoardCount >= _newBaseCost)
        {
            _woodBoardCounter.DecreaseCountByNumber(_newBaseCost);
            scavenger.MoveToBuildNewBase(_newBaseFlag);
            return true;
        }

        return false;
    }

    private void NotifyAboutFlagSet()
    {
        FlagSet?.Invoke();
        Destroy(_phantomBase.gameObject);
    }
}
