using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CellAnimationController : MonoBehaviour
{
    #region --Переменные--
    public static CellAnimationController Instance { get; private set; }
    [SerializeField] CellAnimation animationPrefabs;
    #endregion

    #region --Методы--
    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        DOTween.Init();
    }

    /// <summary>
    /// Метод для планой анимации слияния ячеик
    /// </summary>
    /// <param name="from">Из кого переходим</param>
    /// <param name="to">В кого переходим</param>
    /// <param name="isMerging">Флаг слияния ячеик</param>
    public void SmoothTransition(Cell from, Cell to, bool isMerging) => Instantiate(animationPrefabs, transform, false).Move(from, to, isMerging);

    /// <summary>
    /// Метод для гладкого внешнего вида
    /// </summary>
    /// <param name="cell">Ячейка</param>
    public void SmoothAppear(Cell cell) => Instantiate(animationPrefabs, transform, false).Appear(cell);
    #endregion
}
