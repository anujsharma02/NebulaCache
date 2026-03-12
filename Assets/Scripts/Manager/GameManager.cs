using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;

  [SerializeField] private BoardManager board;

  private Queue<CardController> queue = new Queue<CardController>();
  private List<CardController> selected = new List<CardController>();

  private bool processing = false;

  private int totalPairs;

  private void Awake()
  {
    Instance = this;
  }

  private void Start()
  {
    GameSaveData data = SaveManager.Instance.LoadGame();

    if (data != null)
    {
      LoadSavedGame(data);
    }
    else
    {
      board.GenerateBoard();
      StartCoroutine(PreviewCards());
    }

    totalPairs = (board.Rows * board.Cols) / 2;
  }

  public void SelectCard(CardController card)
  {
    if (selected.Contains(card))
      return;

    queue.Enqueue(card);

    if (!processing)
      StartCoroutine(ProcessQueue());
  }

  private IEnumerator ProcessQueue()
  {
    processing = true;

    while (queue.Count > 0)
    {
      CardController card = queue.Dequeue();

      card.Flip();
      selected.Add(card);

      if (selected.Count == 2)
        yield return StartCoroutine(CheckMatch());

      yield return new WaitForSeconds(0.1f);
    }

    processing = false;
  }

  private IEnumerator CheckMatch()
  {
    CardController a = selected[0];
    CardController b = selected[1];

    ScoreManager.Instance.AddTurn();

    if (a.model.id == b.model.id)
    {
      a.model.matched = true;
      b.model.matched = true;

      a.view.DisableInteraction();
      b.view.DisableInteraction();

      ScoreManager.Instance.AddMatch();
      ScoreManager.Instance.AddScore(10);

      if (ScoreManager.Instance.GetMatches() >= totalPairs)
        yield return new WaitForSeconds(1f);
      GameCompleted();
    }
    else
    {
      yield return new WaitForSeconds(1f);

      a.Flip();
      b.Flip();
    }

    selected.Clear();
  }

  private void GameCompleted()
  {
    UIHandler.Instance.GameOverPanel.SetActive(true);

    UIHandler.Instance.FinalScoreText.text =
        "Final Score: " + ScoreManager.Instance.GetScore();
  }

  private IEnumerator PreviewCards()
  {
    yield return new WaitForSeconds(2f);

    foreach (var card in board.Cards)
    {
      card.Flip();
      yield return new WaitForSeconds(0.03f);
    }

    foreach (var card in board.Cards)
    {
      card.view.EnableAnimator();
      card.view.EnableInteraction();
    }
  }
  public void SaveProgress()
  {
    GameSaveData data = new GameSaveData();

    data.rows = board.Rows;
    data.cols = board.Cols;

    data.score = ScoreManager.Instance.GetScore();
    data.turns = ScoreManager.Instance.GetTurns();
    data.matches = ScoreManager.Instance.GetMatches();

    int count = board.Cards.Count;

    data.cardIDs = new int[count];
    data.matchedCards = new bool[count];

    for (int i = 0; i < count; i++)
    {
      data.cardIDs[i] = board.Cards[i].model.id;
      data.matchedCards[i] = board.Cards[i].model.matched;
    }

    SaveManager.Instance.SaveGame(data);
  }

  private void OnApplicationQuit()
  {
    SaveProgress();
  }

  private void OnApplicationPause(bool pause)
  {
    if (pause)
      SaveProgress();
  }
  private void LoadSavedGame(GameSaveData data)
  {
    board.GenerateBoard(data.cardIDs);

    int count = board.Cards.Count;

    for (int i = 0; i < count; i++)
    {
      if (data.matchedCards[i])
      {
        board.Cards[i].model.matched = true;
        board.Cards[i].view.ShowFrontInstant();
        board.Cards[i].view.DisableInteraction();
      }
      else
      {
        board.Cards[i].view.ShowBackInstant();
        // ENABLE interaction for playable cards
        board.Cards[i].view.EnableInteraction();
      }
    }

    // Restore score / turns / matches
    ScoreManager.Instance.SetValues(
        data.score,
        data.turns,
        data.matches
    );
  }

  // Restart the current game (same scene)
  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }


  // Start a completely new game
  public void NewGame()
  {
    SaveManager.Instance.ClearSave();
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  // Quit the application
  public void QuitGame()
  {
    Debug.Log("Quit Game");
    Application.Quit();
  }
}