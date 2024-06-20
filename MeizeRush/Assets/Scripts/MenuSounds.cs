using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSounds : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource src;
    public AudioClip sfx1;

    public void buttonPlay()
    {
        src.clip = sfx1;
        src.Play();
        StartCoroutine(startDelay());

        SceneManager.LoadScene("Game");


    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(5);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
