using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private Transform player;
    [SerializeField] int gemScore = 100;
    [SerializeField] int scrapFound = 30;
    private GameControllerScript gc;
    public PlayerScript ps;
    public float distancia; //1.2 parece um bom valor
    private bool aberto;
    public Sprite openChest;

    // Start is called before the first frame update
    void Start()
    {
        aberto = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        gc = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= distancia)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!aberto)
                {
                    aberto = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = openChest;
                    int item = Random.Range(0, 9);
                    switch (item)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            Debug.Log("Voc� ganhou uma GEMA!");
                            gc.score += gemScore;
                            // texto na tela
                            break;
                        case 5:
                            Debug.Log("Voc� ganhou um UPGRADE DE DANO!");
                            ps.attackDamage += 5;
                            // aumenta o dano do chicote
                            break;
                        case 6:
                        case 7:
                        case 8:
                            Debug.Log("Voc� ganhou MATERIAIS!");
                            gc.scrap += scrapFound;
                            // aumenta o contador de materiais
                            break;
                        case 9:
                            Debug.Log("Voc� ganhou um PET NOVO!");
                            // fun��o que pergunta se o player deseja trocar o pet antigo pelo novo, se sim, faz a troca
                            break;
                        default:
                            //etc
                            break;
                    }
                    gameObject.SetActive(false);
                }
                else Debug.Log("Bau ja aberto");
            }
        }

    }
}
