using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuChangeScene : MonoBehaviour
{
    public void StartGame()
    {
        // StartCoroutine(startDelay());
        // SceneManager.LoadScene("Game");
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(2f);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
