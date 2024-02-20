using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhantomBase : MonoBehaviour
{
    [SerializeField] private Color _possiblePlaceColor;
    [SerializeField] private Color _impossiblePlaceColor;
    [SerializeField] private Material _roofMaterial;
    [SerializeField] private float _freeBuildingRadius = 30f;
    public bool IsPossibleToPlace { get; private set; }
    private enum IgnoringColliders
    {
        Flag,
        Terrain,
        TotalNumber,
    }

    private void OnEnable()
    {
        _roofMaterial.color = _possiblePlaceColor;
        IsPossibleToPlace = true;
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _freeBuildingRadius);

        if (colliders.Length > (int)IgnoringColliders.TotalNumber)
        {
            _roofMaterial.color = _impossiblePlaceColor;
            IsPossibleToPlace = false;
        }
        else
        {
            _roofMaterial.color = _possiblePlaceColor;
            IsPossibleToPlace = true;
        }
    }
}
