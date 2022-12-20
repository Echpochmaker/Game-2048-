using UnityEngine;
using TMPro;


public class GameController : MonoBehaviour
{
    #region --Переменные--
    public static GameController Instance;
    [SerializeField] private TextMeshProUGUI gameResult;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI savePointsText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winningPanel;
    [SerializeField] GameObject gameMenuPanel;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject menuButton;
    #endregion

    #region --Свойства--

    /// <summary>
    /// Очки
    /// </summary>
    public static int Points { get; private set; }

    /// <summary>
    /// Лучшие очки
    /// </summary>
    public static int BestPoints { get; private set; }

    /// <summary>
    /// Флаг запуска/остановки игры
    /// </summary>
    public static bool GameStarted { get; private set; }
    #endregion

    #region --Методы--
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (!PlayerPrefs.HasKey("Score"))
            PlayerPrefs.SetInt("Score", 0);

    }
    private void Start()
    {
        BestPoints = PlayerPrefs.GetInt("Score");
        StartGame();
    }

    /// <summary>
    /// Вызывается при победе 
    /// </summary>
    public void Win()
    {
        winningPanel.SetActive(true);
        GameStarted = false;
        gameResult.text = "You Win!";
        restartButton.SetActive(false);
        menuButton.SetActive(false);
    }

    /// <summary>
    /// Вызывается при проигрыше 
    /// </summary>
    public void Lose()
    {
        gameOverPanel.SetActive(true);
        GameStarted = false;
        gameResult.text = "You Lose!";
        restartButton.SetActive(false);
        menuButton.SetActive(false);
    }

    /// <summary>
    /// Вызывается при запуске игры
    /// </summary>
    public void StartGame()
    {
        GameStarted = true;
        gameResult.text = "";

        SetPoints(0);
        BestSavePoints(BestPoints);
        PlayerPrefs.SetInt("Score", BestPoints);

        menuButton.SetActive(true);
        restartButton.SetActive(true);
        gameOverPanel.SetActive(false);
        winningPanel.SetActive(false);
        gameMenuPanel.SetActive(false);
        Filed.Instance.GenerateFiled();
    }

    /// <summary>
    /// Добавление очков к счету
    /// </summary>
    /// <param name="points">Счет</param>
    public void AddPoints(int points)
    {
        SetPoints(Points + points);
        if (BestPoints < Points)
        {
            BestSavePoints(BestPoints + points);
            PlayerPrefs.SetInt("Score", BestPoints);
        }
        else
            BestSavePoints(BestPoints);

    }

    /// <summary>
    /// Установка текущего счета 
    /// </summary>
    /// <param name="points">Счет</param>
    public void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
    }

    /// <summary>
    /// Установка лучшего рекорда в игре 
    /// </summary>
    /// <param name="points">Счет</param>
    public void BestSavePoints(int points)
    {
        BestPoints = points;
        savePointsText.text = BestPoints.ToString();
    }

    /// <summary>
    /// Вызывается при нажатии кнопки "Продолжить"
    /// </summary>
    public void KeepPlaing()
    {
        GameStarted = true;
        winningPanel.SetActive(false);
        restartButton.SetActive(true);
        menuButton.SetActive(true);
        SetPoints(Points);
        BestSavePoints(BestPoints);
    }

    /// <summary>
    /// Скрыть меню игры
    /// </summary>
    public void HideMenuPanel() 
    {
        GameStarted = true;
        gameMenuPanel.SetActive(false);        
    }

    /// <summary>
    /// Показать меню игры
    /// </summary>
    public void ShowMenuPanel() 
    {
        GameStarted = false;
        gameMenuPanel.SetActive(true);       
    }

    #endregion
}
