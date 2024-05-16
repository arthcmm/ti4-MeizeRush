using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float life = 100;
    public float attackDamage = 35; //dano de ataque do jogador
    public float attackCooldown = 0.5f;  // Tempo de cooldown entre os ataques
    private float cooldownTimer = 0f;    // Contador para o cooldown
    private float stamina = 100;
    private float staminaMax = 100;
    private float staminaRecover = 15;
    public EnemyBehaviour enemy;
    public GameObject[] targets;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //tempo do jogo - passar para o gamecontroller depois
        targets = GameObject.FindGameObjectsWithTag("Enemy").ToArray();
    }

    // Update is called once per frame
    void Update()
    {

        cooldownTimer -= Time.deltaTime;
        // Verifica se o botão esquerdo do mouse foi pressionado e o cooldown já passou
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && stamina > 0)
        {
            // Faça aqui o código para o ataque do jogador
            Attack();
            // Reinicia o contador de cooldown
            cooldownTimer = attackCooldown;
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0) //corre
        {
            stamina -= Time.deltaTime * 10; //diminui stamina
            //aumenta a velocidade do player (sprint)
        }
        else
        {
            if (stamina < staminaMax)
            {
                stamina += Time.deltaTime * staminaRecover;
            }
            if (stamina > staminaMax)
            {
                stamina = staminaMax;
            }
        }


        if (life <= 0) //para o jogo quando o jogador perde, passar essa verificação para o gamecontroller depois
        {
            Time.timeScale = 0;
        }
    }

    void Attack()
    {
        //print("attack!");
        if (Vector2.Distance(this.transform.position, FindNearestEnemy().transform.position) <= 2)
        {
            print("gottem!");
            enemy.health -= attackDamage;
        }
        //tocar a animação

    }

    EnemyBehaviour FindNearestEnemy()
    {
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < targets.Length; i++)
        {
            float distance = Vector2.Distance(this.transform.position, targets[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                if (!targets[i].activeSelf)
                {
                    continue;
                }
                enemy = targets[i].GetComponent<EnemyBehaviour>();
            }
        }
        return enemy;
    }

}
