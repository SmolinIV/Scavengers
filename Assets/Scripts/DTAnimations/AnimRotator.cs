using DG.Tweening;
using UnityEngine;

public class AnimRotator : AnimConditionChanger
{
    [SerializeField] protected Vector3 _direction;
    [SerializeField] protected RotateMode _rotateMode;

    protected override void StartAnimation()
    {
        transform.DORotate(_direction, _animDuration, _rotateMode).
            SetLoops(_animRepeatNumber, _loopType).
            SetEase(Ease.Linear);
    }
}
