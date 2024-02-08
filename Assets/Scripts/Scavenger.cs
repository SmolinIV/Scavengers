using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenger : MonoBehaviour
{
    [SerializeField] private WoodBoard _targetingWoodBoard;
    [SerializeField] private int _movingSpeed = 100;


    private void Start()
    {
        MoveToTarget();
    }

    private void Update()
    {
        if (transform.position != _targetingWoodBoard.transform.position)
            MoveToTarget();
    }

    public void SetTargetWoodBoard(WoodBoard woodBoard)
    {
        _targetingWoodBoard = woodBoard;
    }

    private void MoveToTarget()
    {
        Vector3.MoveTowards(transform.position, _targetingWoodBoard.transform.position, _movingSpeed * Time.deltaTime);
    }
}
