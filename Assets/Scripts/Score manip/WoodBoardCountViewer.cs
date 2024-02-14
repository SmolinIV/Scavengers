using TMPro;
using UnityEngine;

public class WoodBoardCountViewer : MonoBehaviour
{
    [SerializeField] private WoodBoardCounter _woodBoardCounter;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _woodBoardCounter.WoodBoardCountChanged += ChangeText;
    }

    private void ChangeText(int currentWoodBoardCount)
    {
        _text.text = currentWoodBoardCount.ToString();
    }
}
