using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WoodBoardsPool))]
[RequireComponent(typeof(NavMeshSurface))]
public class WoodBoardsSpawner : MonoBehaviour
{

    [SerializeField] private float _spawnSphereRadius = 25f;
    [SerializeField] private float _spawnFrequency = 2f;

    public event Action<WoodBoard> WoodBoardSpawned;

    private NavMeshSurface _navMeshSurface;
    private WoodBoardsPool _woodBoardsPool;

    private Coroutine _woodBoardSpawning;

    private float _passedTime;

    private void Awake()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _woodBoardsPool = GetComponent<WoodBoardsPool>();
        _passedTime = 0;
    }

    //private void Start()
    //{
    //    _woodBoardSpawning = StartCoroutine(SpawnWoodBoard());
    //}

    private void OnDisable()
    {
        if ( _woodBoardSpawning != null )
            StopCoroutine( _woodBoardSpawning );
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
        Vector3 spawnPosition = UnityEngine.Random.insideUnitSphere * _spawnSphereRadius + transform.position;

        NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, _spawnSphereRadius, NavMesh.AllAreas);
        return hit.position;
    }

    public void UpdateNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }

    //private IEnumerator SpawnWoodBoard()
    //{
    //    WaitForSeconds delay = new WaitForSeconds(_spawnFrequency);

    //    while (gameObject.activeSelf)
    //    {
    //        if (_woodBoardsPool.TryGetWoodBoard(out WoodBoard woodBoard, GetRandomPointOnMesh()))
    //            WoodBoardSpawned?.Invoke(woodBoard);

    //        yield return delay;
    //    }
    //}
}
