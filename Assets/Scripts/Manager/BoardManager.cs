using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
  [SerializeField] private Transform boardParent;
  [SerializeField] private GameObject cardPrefab;

  [SerializeField] private int rows;
  [SerializeField] private int cols;

  [SerializeField] private Sprite[] cardSprites;

  private List<CardController> cards = new List<CardController>();
  public List<CardController> Cards => cards;

  public int Rows => rows;
  public int Cols => cols;

  // ---------- NEW GAME ----------
  public void GenerateBoard()
  {
    int total = rows * cols;
    if (total % 2 != 0)
    {
      Debug.LogError("Rows × Columns must be an EVEN number for card pairs.");
      return;
    }

    ClearBoard();

    List<int> ids = new List<int>();

    for (int i = 0; i < total / 2; i++)
    {
      int spriteIndex = i % cardSprites.Length;

      ids.Add(spriteIndex);
      ids.Add(spriteIndex);
    }

    Shuffle(ids);
    CreateCards(ids.ToArray());
  }

  // ---------- LOAD SAVED BOARD ----------
  public void GenerateBoard(int[] savedIDs)
  {
    ClearBoard();

    CreateCards(savedIDs);
  }

  // ---------- COMMON CARD CREATION ----------
  void CreateCards(int[] ids)
  {
    GridLayoutGroup grid = boardParent.GetComponent<GridLayoutGroup>();

    grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    grid.constraintCount = cols;

    ResizeGrid(grid);

    for (int i = 0; i < ids.Length; i++)
    {
      int id = ids[i];

      GameObject obj = Instantiate(cardPrefab, boardParent);

      CardView view = obj.GetComponent<CardView>();

      CardModel model = new CardModel(id);

      CardController controller = new CardController(model, view);

      view.Init(controller, cardSprites[id]);

      cards.Add(controller);
    }
  }

  // ---------- CLEAR BOARD ----------
  void ClearBoard()
  {
    foreach (Transform child in boardParent)
    {
      Destroy(child.gameObject);
    }

    cards.Clear();
  }

  void ResizeGrid(GridLayoutGroup grid)
  {
    RectTransform rect = boardParent.GetComponent<RectTransform>();

    float boardWidth = rect.rect.width;
    float boardHeight = rect.rect.height;

    float spacingX = grid.spacing.x;
    float spacingY = grid.spacing.y;

    float cellWidth =
        (boardWidth - (cols - 1) * spacingX) / cols;

    float cellHeight =
        (boardHeight - (rows - 1) * spacingY) / rows;

    float cellSize = Mathf.Min(cellWidth, cellHeight);
    grid.cellSize = new Vector2(cellSize, cellSize);

  }

  void Shuffle(List<int> list)
  {
    for (int i = 0; i < list.Count; i++)
    {
      int r = Random.Range(i, list.Count);

      int t = list[i];
      list[i] = list[r];
      list[r] = t;
    }
  }
}
