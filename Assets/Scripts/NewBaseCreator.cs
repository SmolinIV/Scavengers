using System;
using UnityEngine;

public class NewBaseCreator : MonoBehaviour
{ 
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private PhantomBase _phantomBasePrefab;
    [SerializeField] private WoodBoardCounter _woodBoardCounter;
    [SerializeField] private int _newBaseCost = 5;


    private Flag _newBaseFlag;
    private Flag _changingPositionFlag;
    private PhantomBase _phantomBase;

    private bool _isFlagCreated = false;
    private bool _isFlagSet = false;

    public event Action FlagSet;

    private void Awake()
    {
        _phantomBase = Instantiate(_phantomBasePrefab);
        _newBaseFlag = Instantiate(_flagPrefab);
        _changingPositionFlag = Instantiate(_flagPrefab);

        _newBaseFlag.SetPhantomBase(_phantomBase);
        _changingPositionFlag.SetPhantomBase(_phantomBase);

        _phantomBase.gameObject.SetActive(false);
        _newBaseFlag.gameObject.SetActive(false);
        _changingPositionFlag.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        if (_newBaseFlag != null)
            _newBaseFlag.Set -= NotifyAboutFlagSet;

        if (_changingPositionFlag != null)
            _changingPositionFlag.Set -= NotifyAboutFlagSet;
    }

    public void SetFlag()
    {
        if (_isFlagCreated == false)
        {
            _isFlagCreated = true;
            CreateBuildingBaseCursor(_newBaseFlag);
        }
        else if (_isFlagSet)
        {
            CreateBuildingBaseCursor(_changingPositionFlag);
        }
    }

    public bool TryCreateNewBase(Scavenger scavenger)
    {
        if (_woodBoardCounter.WoodBoardCount >= _newBaseCost)
        {
            _woodBoardCounter.DecreaseCountByNumber(_newBaseCost);
            scavenger.MoveToBuildNewBase(_newBaseFlag);

            _isFlagSet = false;
            _isFlagCreated = false;

            return true;
        }

        return false;
    }

    private void CreateBuildingBaseCursor(Flag creatingFlag)
    {
        creatingFlag.Set += NotifyAboutFlagSet;
        _phantomBase.gameObject.SetActive(true);
        _phantomBase.transform.SetParent(creatingFlag.transform);
        _phantomBase.transform.localPosition = Vector3.zero;
        creatingFlag.gameObject.SetActive(true);
    }

    private void NotifyAboutFlagSet()
    {
        if (_changingPositionFlag.isActiveAndEnabled)
        {
            Vector3 newPosition = _changingPositionFlag.transform.position;
            _changingPositionFlag.gameObject.SetActive(false);
           _newBaseFlag.transform.position = newPosition;
        }

        _phantomBase.gameObject.SetActive(false);
        _isFlagSet = true;

        FlagSet?.Invoke();
    }
}