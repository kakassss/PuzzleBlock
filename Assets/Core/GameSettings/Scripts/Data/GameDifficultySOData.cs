using UnityEngine;

[CreateAssetMenu(fileName = "GameDifficultyData", menuName = "ScriptableObjects/GameDifficultyDataSO")]
public class GameDifficultySOData : ScriptableObject
{
    public string DifficultyName;
    [Range(4,6)] public int GridSize;
    [Range(5,12)] public int PieceAmount;
}
