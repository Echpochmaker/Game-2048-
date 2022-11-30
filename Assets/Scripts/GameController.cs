using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region --Переменные--
    public static GameController Instance;
    [SerializeField] private TextMeshProUGUI gameResult;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI savePointsText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winningPanel;
    [SerializeField] GameObject  restartButton;
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
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
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

        restartButton.SetActive(true);
        gameOverPanel.SetActive(false);
        winningPanel.SetActive(false);
        Filed.Instance.GenerateFiled();
    }  

    /// <summary>
    /// Добавление очков к счету
    /// </summary>
    /// <param name="points">Счет</param>
    public void AddPoints(int points) 
    {    
        SetPoints(Points + points);
        if(BestPoints < Points)
            BestSavePoints(BestPoints + points);
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
        SetPoints(Points);
        BestSavePoints(BestPoints);      
    }

    #endregion
}
