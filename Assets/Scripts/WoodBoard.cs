using DG.Tweening;
using UnityEngine;

[RequireComponent (typeof(AnimConditionChanger))]

public class WoodBoard : MonoBehaviour
{
    private AnimConditionChanger _animations;

    private void Start()
    {
        _animations = GetComponent<AnimConditionChanger>();
    }

    public void StopAnimation()
    {
        transform.DOKill();
    }
}
