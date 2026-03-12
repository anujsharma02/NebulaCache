using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : Singleton<UIHandler>
{
    [Header("Top UI")]

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI matchText;

    [Header("Panels")]

    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over UI")]

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;

    public TextMeshProUGUI ScoreText => scoreText;
    public TextMeshProUGUI TurnText => turnText;
    public TextMeshProUGUI MatchText => matchText;

    public GameObject GameOverPanel => gameOverPanel;
    public TextMeshProUGUI FinalScoreText => finalScoreText;

    [Header("Starting Panel UI")]
    [SerializeField] private GameObject startPanel;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TMP_InputField rowInput;
    [SerializeField] private TMP_InputField columnInput;
    public GameObject StartPanel => startPanel;
    public Button ContinueButton => continueButton;
    public Button NewGameButton => newGameButton;
    public Button PlayButton => playButton;
    public Button QuitButton => quitButton;
    public TMP_InputField RowInput => rowInput;
    public TMP_InputField ColumnInput => columnInput;
    public TextMeshProUGUI ErrorMsg;

    [SerializeField] private GameObject Header;
    [SerializeField] private GameObject GameBoard;
    [SerializeField] private GameObject Footer;

    public void StartNewGame()
    {
        NewGameButton.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(true);
        RowInput.gameObject.SetActive(true);
        ColumnInput.gameObject.SetActive(true);
    }

    public void EnableDisableObject(bool _value)
    {
        Header.SetActive(_value);
        GameBoard.SetActive(_value);
        Footer.SetActive(_value);
    }

    public void DisableGameOver(bool _value)
    {
        GameOverPanel.SetActive(_value);
        StartPanel.SetActive(!_value);
        EnableDisableObject(_value);
    }
}