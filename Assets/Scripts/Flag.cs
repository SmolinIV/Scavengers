using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Flag : MonoBehaviour
{
    public Action Set;

    private Ray _mousRay;
    private RaycastHit[] _hits;

    private Coroutine _untilClickMoving;
    private bool _isSetOnGround;

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
        _mousRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        _hits = Physics.RaycastAll(_mousRay);

        RaycastHit hit = _hits.Where(hit => hit.collider.TryGetComponent(out Terrain component)).FirstOrDefault();

        if (hit.point != Vector3.zero)
            transform.position = hit.point;
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