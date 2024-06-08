using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public GameObject LoadingScreen;
    //public Image LoadingBarFill;
    void Start()
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(InitializePlayer());
    }

    IEnumerator InitializePlayer()
    {
        // Loading falso xd
        yield return new WaitForSeconds(2f);
        LoadingScreen.SetActive(false);
    }
}
