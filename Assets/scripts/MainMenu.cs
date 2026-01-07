using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (LevelManager.instance != null)
        {
            LevelManager.instance.ResetLevels();
        }

        SceneManager.LoadScene("Level Intro");
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }
}