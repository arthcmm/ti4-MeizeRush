using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float health = 100;
    public Transform enemyPos;
    public GameObject thisEnemy;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = this.transform; //atualiza a posicao desse inimigo, eu acho
        if (health < 0)
        {
            thisEnemy.SetActive(false);
            health = 100; //reseta para respawnar o inimigo depois
        }
    }
}
