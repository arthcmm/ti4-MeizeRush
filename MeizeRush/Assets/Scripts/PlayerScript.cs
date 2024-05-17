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
    public float attackStaminaCost = 15;
    private float cooldownTimer = 0f;    // Contador para o cooldown
    private float stamina = 100;
    private float staminaMax = 100;
    private float staminaRecover = 15;
    private bool tired;
    public bool hasRuby = false;
    [SerializeField] CharMove cm;
    public float defaultSpeed;
    public float sprintSpeed;
    public EnemyBehaviour enemy;
    public GameObject[] targets;
    public GameControllerScript gc;
    [SerializeField] Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //tempo do jogo - passar para o gamecontroller depois
        targets = GameObject.FindGameObjectsWithTag("Enemy").ToArray();
        tired = false;
        defaultSpeed = 7;
        sprintSpeed = 12;
        cm.speed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        cooldownTimer -= Time.deltaTime;
        // Verifica se o botão esquerdo do mouse foi pressionado e o cooldown já passou
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && stamina > 0)
        {
            Attack();
            stamina = stamina - attackStaminaCost;
            // Reinicia o contador de cooldown
            cooldownTimer = attackCooldown;
        }
        // print(stamina);
        gc.sliderStamina.value = stamina;
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0) //corre
        {
            if (stamina < 1)
            {
                tired = true;
            }
            if (stamina < 15 && tired == true)
            {
                cm.speed = defaultSpeed;
                //rb.drag = normalDrag;
                stamina += Time.deltaTime * staminaRecover;
            }
            if (stamina >= 15)
            {
                tired = false;
            }
            if (tired == false)
            {
                cm.speed = sprintSpeed;
                //rb.drag = runDrag;
                stamina -= Time.deltaTime * 10; //diminui stamina
            }
        }
        else
        {
            cm.speed = defaultSpeed;
            //rb.drag = normalDrag;
            if (stamina < staminaMax)
            {
                stamina += Time.deltaTime * staminaRecover;
                gc.sliderStamina.value = stamina;
            }
            else
            {
                stamina = staminaMax;
                gc.sliderStamina.value = stamina;
                tired = false;
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
            // print("gottem!");
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
