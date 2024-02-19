using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomBase : MonoBehaviour
{
    [SerializeField] private Color _possiblePlaceColor;
    [SerializeField] private Color _impossiblePlaceColor;
    [SerializeField] private Material _roofMaterial;

    public bool IsPossibleToPlace { get; private set; }

    private void OnEnable()
    {
        _roofMaterial.color = _possiblePlaceColor;
        IsPossibleToPlace = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Base baseObject))
        {
            _roofMaterial.color = _impossiblePlaceColor;
            IsPossibleToPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _roofMaterial.color = _possiblePlaceColor;
        IsPossibleToPlace = true;
    }
}
