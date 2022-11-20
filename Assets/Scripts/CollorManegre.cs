using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollorManegre : MonoBehaviour
{
    #region --Переменные--
    public static CollorManegre Instance;
    public Color[] CellColors;

    [Space(5)]
    public Color PointsDarkColor;
    public Color PointsLightColor;
    #endregion

    #region --Методы--
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    #endregion
}
