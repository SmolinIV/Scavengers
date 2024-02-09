using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceBringer))]
public class Scavenger : MonoBehaviour
{
    [SerializeField] private Resource _targetWoodBoard;

    private ResourceBringer _bringer;

    private void Start()
    {
        _bringer = GetComponent<ResourceBringer>();
        MoveToResource(_targetWoodBoard);
    }

    private void MoveToResource(Resource target)
    {
        if (target == null) 
            return;

        _targetWoodBoard = target;
        _bringer.SetTargetResource(_targetWoodBoard);

        _bringer.StartMovingToTarget();
    }
}
