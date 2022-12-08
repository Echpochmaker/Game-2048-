using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region --Переменные--
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI points;
    private CellAnimation currentAnimation;
    public static int MaxValue = 11;
    #endregion

    #region --Свойства--

    /// <summary>
    /// Значение X 
    /// </summary>
    public int X { get; private set; }

    /// <summary>
    /// Значение Y
    /// </summary>
    public int Y { get; private set; }

    /// <summary>
    /// Значание Value
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Значение для очков
    /// </summary>
    public int Points => Value == 0 ? 0 : (int)Mathf.Pow(2, Value);

    /// <summary>
    /// Флаг пустых клеток
    /// </summary>
    public bool IsEmpty => Value == 0;

    /// <summary>
    /// Флаг соединения ячеик
    /// </summary>
    public bool HasMerged { get; private set; }
    #endregion

    #region --Методы--
    /// <summary>
    /// Установка значения 
    /// </summary>
    /// <param name="x">Значение "x" </param>
    /// <param name="y">Значение "y"</param>
    /// <param name="value">Значение "value"</param>
    /// <param name="updateUI">Флаг обновления клетки</param>
    public void SetValue(int x, int y, int value, bool updateUI = true) 
    {
        X = x;
        Y = y;  
        Value = value;

        if(updateUI)
            UpdateCell();
    }

    /// <summary>
    /// Увеличение значения
    /// </summary>
    public void IncreaseValue() 
    {
        Value++;
        HasMerged = true;   
        GameController.Instance.AddPoints(Points);
    }

    /// <summary>
    /// Сброс флага для соединения ячеик
    /// </summary>
    public void ResetFlags() => HasMerged = false;

    /// <summary>
    /// Объединение ячеик 
    /// </summary>
    /// <param name="otherCell">Другая ячейка</param>
    public void MergeWithCell(Cell otherCell) 
    {
        CellAnimationController.Instance.SmoothTransition(this, otherCell, true);
        otherCell.IncreaseValue();
        SetValue(X, Y, 0);
    }

    /// <summary>
    /// Перемещение в другую ячейку
    /// </summary>
    /// <param name="target">Цель (ячейка)</param>
    public void MoveToCell(Cell target) 
    {
        CellAnimationController.Instance.SmoothTransition(this, target, false);
        target.SetValue(target.X, target.Y, Value, false);
        SetValue(X, Y, 0);       
    }

    /// <summary>
    /// Обновление ячеик
    /// </summary>
    public void UpdateCell() 
    {
        points.text = IsEmpty ? string.Empty : Points.ToString();
        points.color = Value <= 2 ? CollorManegre.Instance.PointsDarkColor :
            CollorManegre.Instance.PointsLightColor;

        image.color = CollorManegre.Instance.CellColors[Value];
    }

    /// <summary>
    /// Установка анимации
    /// </summary>
    /// <param name="animation">Объект анимации</param>
    public void SetAnimation(CellAnimation animation) => currentAnimation = animation;

    /// <summary>
    /// Окончание анимации
    /// </summary>
    public void CancelAnimation() 
    {    
        if (currentAnimation != null)
            currentAnimation.Destroy();
    }
    #endregion
}
