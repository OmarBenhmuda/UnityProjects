using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private GameObject _leaderboard;
    public void StartGame()
    {
        SceneManager.LoadScene("Maze");
    }

    public void OpenLeaderboard()
    {
        _leaderboard.SetActive(true);
        _menu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void GoBack()
    {
        _leaderboard.SetActive(false);
        _menu.SetActive(true);
    }
}
