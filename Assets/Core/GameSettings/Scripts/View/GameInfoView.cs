using System;
using TMPro;
using UnityEngine;
using Zenject;

public class GameInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelDifficult;
    [SerializeField] private TMP_Text _gridSize;
    [SerializeField] private TMP_Text _pieceAmount;

    private IGameDifficultyController _gameDifficultyController;

    private string _pieceText = "Pieces:";
    private string _gridText = "Grid:";
    
    [Inject]
    private void Construct(IGameDifficultyController gameDifficultyController)
    {
        _gameDifficultyController = gameDifficultyController;
    }

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        var gameInfo = _gameDifficultyController.GetDifficultyData();

        _levelDifficult.text = gameInfo.DifficultyName;
        _gridSize.text = _gridText + gameInfo.GridSize + "x" + gameInfo.GridSize;
        _pieceAmount.text = _pieceText + gameInfo.PieceAmount;
    }
    

}
