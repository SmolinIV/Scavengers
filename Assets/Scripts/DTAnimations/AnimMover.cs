using DG.Tweening;
using UnityEngine;

public class AnimMover : AnimConditionChanger
{
    [SerializeField] protected Vector3 _distance;

    protected void OnEnable()
    {
        _distance += transform.position;
        base.OnEnable();
    }

    protected override void StartAnimation()
    {
        transform.DOMove(_distance, _animDuration).
            SetLoops(_animRepeatNumber, _loopType).
            SetEase(Ease.Linear);
    }
}