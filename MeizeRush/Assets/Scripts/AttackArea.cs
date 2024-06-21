using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerScript player; //puxo o dano daqui
    public EnemyBehaviour enemy;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip deathSound, enemySound;
    public GameControllerScript gc;
    public Vector2 pointerPosition { get; set; }

    // public Vector2 movement;



    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("EnemyHitbox"))
        {
            // Tenta obter o componente EnemyBehaviour do próprio objeto ou de seus pais
            EnemyBehaviour enemy = collider.GetComponentInParent<EnemyBehaviour>();

            if (enemy != null)
            {
                enemy.health -= player.attackDamage;
                StartCoroutine(enemy.FlashRed());
                audioSource.clip = enemySound;
                audioSource.Play();
                if (enemy.health <= 0)
                {
                    audioSource.clip = deathSound;
                    audioSource.Play();
                    gc.score += 5;
                }
                // Debug.Log("Hitou o inimigo: " + enemy.name + ", nova vida: " + enemy.health);
            }
            else
            {
                Debug.LogWarning("EnemyBehaviour não encontrado no objeto ou seus pais com a tag 'EnemyHitbox'. Objeto: " + collider.gameObject.name);
            }
        }
    }
    public void Attack(bool attack)
    {
        if (attack)
        {
            animator.SetTrigger("Attack");
        }
    }

}
