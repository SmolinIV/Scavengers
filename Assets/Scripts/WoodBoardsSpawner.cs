using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WoodBoardsPool))]
public class WoodBoardsSpawner : MonoBehaviour
{
    public Action<WoodBoard> WoodBoardSpawned;

    [SerializeField] private float _spawnSphereRadius = 25f;
    [SerializeField] private float _spawnFrequency = 2f;

    private WoodBoardsPool _woodBoardsPool;
    private float _passedTime;

    private void Start()
    {
        _woodBoardsPool = GetComponent<WoodBoardsPool>();
        _passedTime = 0;
    }

    private void Update()
    {
        if (_passedTime < _spawnFrequency)
        {
            _passedTime += Time.deltaTime;
            return;
        }

        _passedTime = 0;

        if (_woodBoardsPool.TryGetWoodBoard(out WoodBoard woodBoard, GetRandomPointOnMesh()))
            WoodBoardSpawned?.Invoke(woodBoard);
    }

    private Vector3 GetRandomPointOnMesh()
    {
        NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * _spawnSphereRadius + transform.position, out NavMeshHit hit, _spawnSphereRadius, NavMesh.AllAreas);
        return hit.position;
    }
}
