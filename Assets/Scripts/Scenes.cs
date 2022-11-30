using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    /// <summary>
    /// Метод для переключений между экранами (сценами) игры
    /// </summary>
    /// <param name="sceneNumber">Номер сцены по списку</param>
    public void NextScene(int sceneNumber) => SceneManager.LoadScene(sceneNumber);    
}
