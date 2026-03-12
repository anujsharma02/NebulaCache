using System;

[Serializable]
public class GameSaveData
{
    public int rows;
    public int cols;

    public int score;
    public int turns;
    public int matches;

    public int[] cardIDs;
    public bool[] matchedCards;
}