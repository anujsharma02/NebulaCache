using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

  private int score;
  private int turns;
  private int matches;

  void start()
  {
    score = 0;
    turns = 0;
    matches = 0;
  }

  public void AddTurn()
  {
    turns++;

    UIHandler.Instance.TurnText.text = "Turns: " + turns;
  }

  public void AddMatch()
  {
    matches++;

    UIHandler.Instance.MatchText.text = "Matches: " + matches;
  }

  public void AddScore(int points)
  {
    score += points;

    UIHandler.Instance.ScoreText.text = "Score: " + score;
  }

  public int GetScore()
  {
    return score;
  }

  public int GetTurns()
  {
    return turns;
  }

  public int GetMatches()
  {
    return matches;
  }

  public void SetValues(int savedScore, int savedTurns, int savedMatches)
  {
    score = savedScore;
    turns = savedTurns;
    matches = savedMatches;

    UIHandler.Instance.ScoreText.text = "Score: " + score;
    UIHandler.Instance.TurnText.text = "Turns: " + turns;
    UIHandler.Instance.MatchText.text = "Matches: " + matches;
  }
}