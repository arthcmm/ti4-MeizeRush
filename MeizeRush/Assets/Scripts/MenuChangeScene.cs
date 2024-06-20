using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuChangeScene : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip playSound;
    public void StartGame()
    {
        audioSource.clip = playSound;
        audioSource.Play();
        StartCoroutine(DeactivateAfterAudio());
        SceneManager.LoadScene("Game");
    }
    private IEnumerator DeactivateAfterAudio()
{
    // Wait for the audio to finish playing
    yield return new WaitWhile(() => audioSource.isPlaying);

    // Deactivate the GameObject
    gameObject.SetActive(false);
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
