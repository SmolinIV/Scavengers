using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flag : MonoBehaviour
{
    public Action Set;

    private float _positionCorrectionY = 1.4f;

    private bool _isSetOnGround;
    private Coroutine _untilClickMoving;

    public void OnEnable()
    {
        _isSetOnGround = false;

        _untilClickMoving = StartCoroutine(MoveUntilClick());
    }

    private void OnDisable()
    {
        if (_untilClickMoving != null)
            StopCoroutine(_untilClickMoving);
    }

    private void MoveToCursor()
    {
        transform.position = Input.mousePosition;

        Vector3 newPosition = transform.position;
        newPosition.y = _positionCorrectionY;
        transform.position = newPosition;
    }

    private void SetOnGround()
    {
        StopCoroutine(_untilClickMoving);
        Set?.Invoke();
    }

    private IEnumerator MoveUntilClick()
    {
        while (!_isSetOnGround)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                SetOnGround();
                yield break;
            }
            else
            {
                MoveToCursor();
                yield return null;
            }
        }
    }
}