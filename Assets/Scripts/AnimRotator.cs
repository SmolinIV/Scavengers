using DG.Tweening;
using UnityEngine;

public class AnimRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;
    [SerializeField, Min(0)] private float _animDuration;
    [SerializeField, Min(-1)] private int _animRepeatNumber = -1;

    [SerializeField] private RotateMode _rotateMode;
    [SerializeField] private LoopType _loopType;

    private void OnEnable()
    {
        StartAnimation();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void StopAnimation()
    {
        transform.DOKill();
    }

    private void StartAnimation()
    {
        transform.DORotate(_direction, _animDuration, _rotateMode).
            SetLoops(_animRepeatNumber, _loopType).
            SetEase(Ease.Linear);
    }
}
