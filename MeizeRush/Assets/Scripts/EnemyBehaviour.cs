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
  private GameObject controller;
  private RandomPlaceEnemies randomPlaceEnemies;
  private PlayerScript playerScript;
  public SpriteRenderer spriteRenderer;

  public AudioSource audioSource;
  public AudioClip trap,playerHit;

  public LayerMask
      placementMask; // Set up a LayerMask in the Inspector to check only the

  private float cooldownTimer = 1.0f;

  public IEnumerator FlashRed()
  {
    spriteRenderer.color = Color.red;
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.color = Color.white;
  }
  // Start is called before the first frame update
  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerScript = player.GetComponent<PlayerScript>();
    controller = GameObject.FindGameObjectWithTag("Controller");
    randomPlaceEnemies = controller.GetComponent<RandomPlaceEnemies>();
    audioSource = GameObject.FindGameObjectWithTag("TrapSound").GetComponent<AudioSource>();


  }

  // Update is called once per frame
  void Update()
  {

    // enemyPos = this.transform; // atualiza a posicao desse inimigo, eu acho
    cooldownTimer -= Time.deltaTime;
    if (health <= 0)
    {
      thisEnemy.SetActive(false);
      randomPlaceEnemies.enemyCount--;
      health = 100; // reseta para respawnar o inimigo depois
    }

    if (Vector3.Distance(transform.position, player.transform.position) <=
        distancia)
    {
      if (cooldownTimer <= 0.0f)
      {
        playerScript.life -= 5;
        audioSource.clip = playerHit;
        audioSource.Play();
        // Debug.Log("Player hit: " + playerScript.life);
        cooldownTimer = 1.0f;
      }
    }

    Vector3 positionToPlace = thisEnemy.transform.position;
    positionToPlace.z =
        positionToPlace.z + (float)0.01; // deixa a trap atraz do player

    // Define the size of the check area (this should match or be slightly
    // larger than the object's size)
    Vector3 checkSize = new Vector3(
        (float)1, (float)1,
        (float)1); // Adjust this size according to your sphere's size

    // Check if the area at the player's feet is clear
    Collider[] hits = Physics.OverlapBox(positionToPlace, checkSize / 16,
                                         Quaternion.identity, placementMask);
    // OverlapBox center is given by the first argument, half-extents by the
    // second, rotation by the third, and layerMask by the fourth.

    // If there is an object, destroy the first one found; otherwise, place a
    // new object
    if (hits.Length > 0)
    {
      if (hits[0].CompareTag("Trap"))
      {
        audioSource.clip = trap;
        audioSource.Play();
        thisEnemy.SetActive(false);
        // StartCoroutine(DeactivateAfterAudio());
        health = 100; // reseta para respawnar o inimigo depois
        // Log and remove the first interactable object encountered
        Destroy(hits[0].gameObject);
      }
    }
  }
  
}
