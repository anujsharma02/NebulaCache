using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
  [SerializeField] private BoardManager board;
  private Queue<CardController> queue = new Queue<CardController>();
  private List<CardController> selected = new List<CardController>();

  private bool processing = false;
  private int totalPairs;

  private void Start()
  {
    UIHandler.Instance.StartPanel.SetActive(true);
    GameSaveData data = SaveManager.Instance.LoadGame();
    UIHandler.Instance.ContinueButton.gameObject.SetActive(data != null);
  }

  // Continue saved game
  public void ContinueGame()
  {
    GameSaveData data = SaveManager.Instance.LoadGame();
    UIHandler.Instance.EnableDisableObject(true);
    if (data == null) return;
    LoadSavedGame(data);
    totalPairs = (board.Rows * board.Cols) / 2;
    StartCoroutine(PreviewLoadedGame());
    UIHandler.Instance.StartPanel.SetActive(false);
  }

  // Start new game with custom rows/cols
  public void StartNewGame()
  {
    UIHandler.Instance.ErrorMsg.gameObject.SetActive(false);

    string rowText = UIHandler.Instance.RowInput.text;
    string colText = UIHandler.Instance.ColumnInput.text;

    // Check empty input
    if (string.IsNullOrWhiteSpace(rowText) || string.IsNullOrWhiteSpace(colText))
    {
      UIHandler.Instance.ErrorMsg.text = "Row and Column cannot be empty";
      UIHandler.Instance.ErrorMsg.gameObject.SetActive(true);
      return;
    }

    UIHandler.Instance.EnableDisableObject(true);
    int rows = int.Parse(rowText);
    int cols = int.Parse(colText);
    // Validate board size
    if (((rows * cols) % 2 != 0) || (rows * cols) < 4)
    {
      UIHandler.Instance.ErrorMsg.text = "Rows × Columns must be EVEN and ≥ 4";
      UIHandler.Instance.ErrorMsg.gameObject.SetActive(true);
      return;
    }

    board.SetBoardSize(rows, cols);
    SaveManager.Instance.ClearSave();
    ScoreManager.Instance.SetValues(0, 0, 0);
    board.GenerateBoard();
    totalPairs = board.Cards.Count / 2;
    UIHandler.Instance.StartPanel.SetActive(false);
    StartCoroutine(PreviewCards());
  }

  // Player selects card
  public void SelectCard(CardController card)
  {
    if (selected.Contains(card)) return;

    queue.Enqueue(card);

    if (!processing)
      StartCoroutine(ProcessQueue());
  }

  IEnumerator ProcessQueue()
  {
    processing = true;

    while (queue.Count > 0)
    {
      CardController card = queue.Dequeue();

      card.Flip();
      selected.Add(card);

      if (selected.Count == 2)
        yield return CheckMatch();

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
      AudioManager.Instance.PlayMatch();
      if (ScoreManager.Instance.GetMatches() == totalPairs)
      {
        yield return new WaitForSeconds(1f);
        GameCompleted();
      }

    }
    else
    {
      AudioManager.Instance.PlayMismatch();
      yield return new WaitForSeconds(1f);

      a.Flip();
      b.Flip();
    }

    selected.Clear();
  }

  private void GameCompleted()
  {
    UIHandler.Instance.GameOverPanel.SetActive(true);
    AudioManager.Instance.PlayGameOver();
    UIHandler.Instance.FinalScoreText.text = "Total Turns: " + ScoreManager.Instance.GetTurns() + "\nFinal Score: " + ScoreManager.Instance.GetScore();
    SaveManager.Instance.ClearSave();
  }

  // Preview for new game
  private IEnumerator PreviewCards()
  {
    yield return new WaitForSeconds(2f);

    foreach (var card in board.Cards)
    {
      card.Flip();
      AudioManager.Instance.PlayFlip();
      yield return new WaitForSeconds(0.03f);
    }

    foreach (var card in board.Cards)
    {
      card.view.EnableAnimator();
      card.view.EnableInteraction();
    }
  }

  // Preview when continuing game
  IEnumerator PreviewLoadedGame()
  {
    yield return new WaitForSeconds(0.05f);

    // Show preview instantly
    foreach (var card in board.Cards)
    {
      if (!card.model.matched)
      {
        card.view.ShowFrontInstant();
      }
    }

    yield return new WaitForSeconds(1.5f);

    // Flip back with animation
    foreach (var card in board.Cards)
    {
      if (!card.model.matched)
      {
        card.view.EnableAnimator();
        card.Flip();
        AudioManager.Instance.PlayFlip();
      }
    }

    // Enable gameplay
    foreach (var card in board.Cards)
    {
      if (!card.model.matched)
      {
        card.view.EnableInteraction();
      }
    }
  }

  // Save game state
  public void SaveProgress()
  {
    if (board.Cards.Count == 0)
      return;

    // Do not save if player never played a turn
    if (ScoreManager.Instance.GetTurns() == 0)
      return;

    // Don't save if game completed
    if (ScoreManager.Instance.GetMatches() == totalPairs)
      return;

    GameSaveData data = new GameSaveData
    {
      rows = board.Rows,
      cols = board.Cols,
      score = ScoreManager.Instance.GetScore(),
      turns = ScoreManager.Instance.GetTurns(),
      matches = ScoreManager.Instance.GetMatches()
    };

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

  void OnApplicationQuit() => SaveProgress();

  private void OnApplicationPause(bool pause)
  {
    if (pause) SaveProgress();
  }
  private void LoadSavedGame(GameSaveData data)
  {
    // Restore board size first
    board.SetBoardSize(data.rows, data.cols);
    // Generate board with saved card order
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

  // Quit the application
  public void QuitGame()
  {
    Application.Quit();
  }
}