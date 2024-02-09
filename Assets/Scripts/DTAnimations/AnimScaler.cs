using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScaler : AnimConditionChanger
{
    [SerializeField] protected Vector3 _endScale;

    private void OnEnable()
    {
        _endScale += transform.localScale;
        base.OnEnable();
    }

    protected override void StartAnimation()
    {
        transform.DOScale(_endScale, _animDuration).
      SetLoops(_animRepeatNumber, _loopType).
      SetEase(Ease.Linear);
    }
}
