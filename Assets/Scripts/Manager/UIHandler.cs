using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

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
    [SerializeField] private Button quitButton;

    public GameObject StartPanel => startPanel;
    public Button ContinueButton => continueButton;
    public Button NewGameButton => newGameButton;
    public Button QuitButton => quitButton;

    private void Awake()
    {
        Instance = this;
    }
}