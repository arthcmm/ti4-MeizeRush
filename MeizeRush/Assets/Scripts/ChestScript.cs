using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private Transform player;
    public float distancia; //1.2 parece um bom valor
    public bool aberto;

    // Start is called before the first frame update
    void Start()
    {
        aberto = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                    int item = Random.Range(0, 9);
                    switch (item)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            Debug.Log("Você ganhou uma GEMA!");
                            // aumenta o score do jogador
                            break;
                        case 5:
                            Debug.Log("Você ganhou um UPGRADE DE DANO!");
                            // aumenta o dano do chicote
                            break;
                        case 6:
                        case 7:
                        case 8:
                            Debug.Log("Você ganhou MATERIAIS!");
                            // aumenta o contador de materiais
                            break;
                        case 9:
                            Debug.Log("Você ganhou um PET NOVO!");
                            // função que pergunta se o player deseja trocar o pet antigo pelo novo, se sim, faz a troca
                            break;
                        default:
                            //etc
                            break;
                    }
                }
                else Debug.Log("Bau ja aberto");
            }
        }

    }
}
