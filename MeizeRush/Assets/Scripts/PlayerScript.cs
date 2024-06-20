using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Vector2 pointerPosition { get; set; }
    public Canvas canvas;
    public float life = 100;
    public float attackDamage = 25;     // dano de ataque do jogador
    public float attackCooldown = 0.5f;
    public AttackArea attackArea;
    private bool attacking = false;
    public float attackStaminaCost = 15;
    private float attackTimer = 0f;
    private float cooldownTimer = 0f; // Contador para o cooldown
    private float stamina = 100;
    private float staminaMax = 100;
    private float staminaRecover = 15;
    public int availableTraps = 0;
    private bool tired;
    public bool end = false;
    [SerializeField]
    CharMove cm;
    public float defaultSpeed;
    public float sprintSpeed;
    public EnemyBehaviour enemy;
    public GameObject[] targets;
    public GameControllerScript gc;
    private float enemiesRefreshCooldown = 1.0f;
    [SerializeField]
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; // tempo do jogo - passar para o gamecontroller depois
        targets = GameObject.FindGameObjectsWithTag("Enemy").ToArray();
        tired = false;
        defaultSpeed = 7;
        sprintSpeed = 12;
        cm.speed = defaultSpeed;
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        enemiesRefreshCooldown -= Time.deltaTime;
        if (enemiesRefreshCooldown <= 0.0f)
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy").ToArray();
            enemiesRefreshCooldown = 1.0f;
        }
        gc.sliderHealth.value = life;
        // Verifica se o botão esquerdo do mouse foi pressionado e o cooldown já
        // passou
        if (Input.GetMouseButtonDown(0) && stamina >= attackStaminaCost && !attacking)
        {
            Debug.Log("attacking");
            Attack();
            stamina = stamina - attackStaminaCost;
        }
        if (attacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0;
                attacking = false;
                attackArea.gameObject.SetActive(attacking);
            }
            attackArea.Attack(attacking);
        }
        // print(stamina);
        gc.sliderStamina.value = stamina;
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0) // corre
        {
            if (stamina < 1)
            {
                tired = true;
            }
            if (stamina < 15 && tired == true)
            {
                cm.speed = defaultSpeed;
                // rb.drag = normalDrag;
                stamina += Time.deltaTime * staminaRecover;
            }
            if (stamina >= 15)
            {
                tired = false;
            }
            if (tired == false)
            {
                cm.speed = sprintSpeed;
                // rb.drag = runDrag;
                stamina -= Time.deltaTime * 10; // diminui stamina
            }
        }
        else
        {
            cm.speed = defaultSpeed;
            // rb.drag = normalDrag;
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
    }
    private void Attack()
    {
        attacking = true;
        attackArea.gameObject.SetActive(attacking);

    }

    // void Attack() {
    //   print("attack!");
    //   if (Vector2.Distance(this.transform.position,
    //                        FindNearestEnemy().transform.position) <= 2) {
    //     print("gottem!");
    //     enemy.health -= attackDamage;
    //   }
    //   // tocar a animação
    // }

    EnemyBehaviour FindNearestEnemy()
    {
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < targets.Length; i++)
        {
            float distance = Vector2.Distance(this.transform.position,
                                              targets[i].transform.position);
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
