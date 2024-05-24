using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
  public float health = 100;
  public int distancia;
  public Transform enemyPos;
  public GameObject thisEnemy;
  private GameObject player;
  private PlayerScript playerScript;


  private float cooldownTimer = 1.0f;
  // Start is called before the first frame update
  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerScript = player.GetComponent<PlayerScript>();
  }

  // Update is called once per frame
  void Update()
  {

    // enemyPos = this.transform; // atualiza a posicao desse inimigo, eu acho
    cooldownTimer -= Time.deltaTime;
    if (health <= 0)
    {
      thisEnemy.SetActive(false);
      health = 100; // reseta para respawnar o inimigo depois
    }

    if (Vector3.Distance(transform.position, player.transform.position) <=
        distancia)
    {
      if (cooldownTimer <= 0.0f)
      {
        playerScript.life -= 5;
        // Debug.Log("Player hit: " + playerScript.life);
        cooldownTimer = 1.0f;
      }
    }
  }
}
