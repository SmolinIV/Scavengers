using DG.Tweening;
using UnityEngine;

[RequireComponent (typeof(AnimConditionChanger))]

public class WoodBoard : MonoBehaviour
{
    [SerializeField] private Vector3 _onHandsSize;
    private AnimConditionChanger _animations;

    private Vector3 _defaultScale;

    private void Start()
    {
        _defaultScale = transform.localScale;
        _animations = GetComponent<AnimConditionChanger>();
    }

    public void PutOnHands()
    {
        transform.localScale = _onHandsSize;
        transform.rotation = new Quaternion(0,0,0,0);
        _animations.StopAnimation();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        transform.localScale = _defaultScale;
    }
}
