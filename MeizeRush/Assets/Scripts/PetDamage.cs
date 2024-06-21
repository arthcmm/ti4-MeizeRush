using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetDamage : MonoBehaviour {

  public int damage;
  private AudioSource audioSource;
  public AudioClip deathSound, enemySound;
  private EnemyBehaviour enemy;
  private float cooldown = 1f;

  private void OnTriggerEnter2D(Collider2D collision) {
    Debug.Log("Colidiu com: " + collision.name);
    if (collision.CompareTag("EnemyHitbox")) {
      // Tenta obter o componente EnemyBehaviour do próprio objeto ou de seus
      // pais
      enemy = collision.GetComponentInParent<EnemyBehaviour>();
    }
  }
  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.CompareTag("EnemyHitbox")) {
      // Tenta obter o componente EnemyBehaviour do próprio objeto ou de seus
      // pais
      enemy = null;
    }
  }

  // Start is called before the first frame update
  void Start() {

    audioSource = GameObject.FindGameObjectWithTag("EnemySounds")
                      .GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update() {

    if (enemy != null && cooldown <= 0.0f) {
      enemy.health -= damage;
      StartCoroutine(enemy.FlashRed());
      audioSource.clip = enemySound;
      audioSource.Play();
      if (enemy.health <= 0) {
        audioSource.clip = deathSound;
        audioSource.Play();
      }
      // Debug.Log("Hitou o inimigo: " + enemy.name + ", nova vida: " +
      // enemy.health);
    } else {
      Debug.LogWarning("EnemyBehaviour não encontrado no objeto ou seus " +
                       "pais com a tag 'EnemyHitbox'. Objeto: " +
                       GetComponent<Collider>().gameObject.name);
    }
    cooldown -= Time.deltaTime;
  }
}
