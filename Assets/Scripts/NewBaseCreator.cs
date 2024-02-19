using System;
using UnityEngine;

public class NewBaseCreator : MonoBehaviour
{ 
    public Action<Flag> FlagSet;

    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private PhantomBase _phantomBasePrefab;
    [SerializeField] private WoodBoardCounter _woodBoardCounter;
    [SerializeField] private int _newBaseCost = 5;

    private Flag _flag;
    private PhantomBase _phantomBase;

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

            _phantomBase = Instantiate(_phantomBasePrefab);
            _phantomBase.transform.SetParent(_flag.transform);
            _phantomBase.transform.localPosition = Vector3.zero;
        }
    }

    private void NotifyAboutFlagSet()
    {
        FlagSet?.Invoke(_flag);
        Destroy(_phantomBase.gameObject);
    }
}
