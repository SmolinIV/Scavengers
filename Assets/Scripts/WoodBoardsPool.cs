using System.Collections.Generic;
using UnityEngine;

public class WoodBoardsPool : MonoBehaviour
{
    [SerializeField] private Resource _woodboardPrefab;
    [SerializeField] private int _capacity;

    private List<Resource> _woodBoards;

    private void Start()
    {
        Resource _tempWoodBoard;
        _woodBoards = new List<Resource>();

        for (int i = 0; i < _capacity; i++)
        {
            _tempWoodBoard = Instantiate( _woodboardPrefab, transform.position, transform.rotation);
            _woodBoards.Add( _tempWoodBoard );
            _woodBoards[i].gameObject.SetActive(false);
        }
    }

    public void TryGetWoodBoard(out Resource woodBoard, Vector3 activatePosition)
    {
        woodBoard = null;

        foreach (Resource prevWoodBoard in _woodBoards)
        {
            if (prevWoodBoard.gameObject.activeSelf == false)
            {
                woodBoard = prevWoodBoard;
                woodBoard.transform.position = activatePosition;
                woodBoard.gameObject.SetActive(true);
            }
        }
    } 
}