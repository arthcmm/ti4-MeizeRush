using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public GameObject player;
    public GameObject ruby;
    public GameControllerScript gc;
    // Start is called before the first frame update
    void Start()
    {
        ruby.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, this.transform.position) < 2 && ruby.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerScript.hasRuby = true;
                gc.score += 5000;
                //barulinho legal xddd
                ruby.SetActive(false);
            }
        }
    }
}
