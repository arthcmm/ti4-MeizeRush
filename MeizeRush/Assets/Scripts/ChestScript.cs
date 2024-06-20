using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject floatingTextPrefab;

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
                            Debug.Log("Você ganhou uma GEMA!");
                            gc.score += gemScore;
                            ShowFloatingText("GEMA!");
                            break;
                        case 5:
                            Debug.Log("Você ganhou um UPGRADE DE DANO!");
                            ps.attackDamage += 5;
                            ShowFloatingText("UPGRADE DE DANO!");
                            break;
                        case 6:
                        case 7:
                        case 8:
                            Debug.Log("Você ganhou MATERIAIS!");
                            gc.scrap += scrapFound;
                            ShowFloatingText("MATERIAIS!");
                            break;
                        case 9:
                            Debug.Log("Você ganhou um PET NOVO!");
                            ShowFloatingText("PET!");
                            break;
                        default:
                            break;
                    }
                    gameObject.SetActive(false);
                }
                else Debug.Log("Baú já aberto");
            }
        }
    }


    private void ShowFloatingText(string message)
    {
        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        TextMeshProUGUI textMesh = floatingText.GetComponent<TextMeshProUGUI>();
        textMesh.text = message;
    }

}
