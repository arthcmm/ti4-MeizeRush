using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //tempo do jogo - passar para o gamecontroller depois
    }

    // Update is called once per frame
    void Update()
    {

        cooldownTimer -= Time.deltaTime;
        // Verifica se o botão esquerdo do mouse foi pressionado e o cooldown já passou
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            // Faça aqui o código para o ataque do jogador
            attack();
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

    void attack()
    {
        print("attack!");
        //tocar a animação
        //if() se tiver um inimigo perto, da dano nele

    }
}
