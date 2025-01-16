using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mini_games_main : MonoBehaviour
{
    // Метод для загрузки новой сцены
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
