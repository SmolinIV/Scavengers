using DG.Tweening;
using UnityEngine;

[RequireComponent (typeof(AnimRotator))]

public class WoodBoard : MonoBehaviour
{
    [SerializeField] private Vector3 _onHandsSize;
    private AnimRotator _animations;

    private Vector3 _defaultScale;

    private void Start()
    {
        _defaultScale = transform.localScale;
        _animations = GetComponent<AnimRotator>();
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
