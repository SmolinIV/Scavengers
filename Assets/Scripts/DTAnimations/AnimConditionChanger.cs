using DG.Tweening;
using UnityEngine;

public abstract class AnimConditionChanger : MonoBehaviour
{
    [SerializeField] protected LoopType _loopType;
    [SerializeField, Min(0)] protected float _animDuration;
    [SerializeField, Min(-1)] protected int _animRepeatNumber = -1;

    protected void OnEnable()
    {
        StartAnimation();
    }

    protected abstract void StartAnimation();

}
