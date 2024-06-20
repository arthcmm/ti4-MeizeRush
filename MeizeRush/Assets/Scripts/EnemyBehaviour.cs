using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
  public float health = 100;
  public int distancia;
  public Transform enemyPos;
  public GameObject thisEnemy;
  private GameObject player;
  private PlayerScript playerScript;

  public LayerMask
      placementMask; // Set up a LayerMask in the Inspector to check only the

  private float cooldownTimer = 1.0f;
  // Start is called before the first frame update
  void Start() {
    player = GameObject.FindGameObjectWithTag("Player");
    playerScript = player.GetComponent<PlayerScript>();
  }

  // Update is called once per frame
  void Update() {

    // enemyPos = this.transform; // atualiza a posicao desse inimigo, eu acho
    cooldownTimer -= Time.deltaTime;
    if (health <= 0) {
      thisEnemy.SetActive(false);
      health = 100; // reseta para respawnar o inimigo depois
    }

    if (Vector3.Distance(transform.position, player.transform.position) <=
        distancia) {
      if (cooldownTimer <= 0.0f) {
        playerScript.life -= 5;
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
    if (hits.Length > 0) {
      if (hits[0].CompareTag("Trap")) {
        thisEnemy.SetActive(false);
        health = 100; // reseta para respawnar o inimigo depois
        // Log and remove the first interactable object encountered
        Destroy(hits[0].gameObject);
      }
    }
  }
}
