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
            // GameObject obj = GameObject.Find("Enemy");
            
            EnemyBehaviour enemy = collider.GetComponent<EnemyBehaviour>();
            enemy.health -= damage;
            Debug.Log("hitou o inimigo: ");

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
