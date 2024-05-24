using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Start is called before the first frame update
    private int damage = 10;
    public EnemyBehaviour enemy;
    public Animator animator;
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
                enemy.health -= damage;
                Debug.Log("Hitou o inimigo: " + enemy.name + ", nova vida: " + enemy.health);
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
