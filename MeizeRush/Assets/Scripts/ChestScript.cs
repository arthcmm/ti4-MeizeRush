using System.Collections;
using UnityEngine;
using TMPro;

public class ChestScript : MonoBehaviour
{
    private Transform player;
    [SerializeField] int gemScore = 100;
    [SerializeField] int scrapFound = 30;
    public AudioSource audioSource;
    public AudioClip gemAudio;
    private GameControllerScript gc;
    public PlayerScript ps;
    public float distancia; //1.2 parece um bom valor
    private bool aberto;
    public Sprite damageChest;
    public Sprite diamondChest;
    public Sprite gearChest;
    public Sprite petChest;
    public Sprite heartChest;
    public PetNavMesh pnm;


    public GameObject floatingTextPrefab;
    private Canvas mainCanvas; // Adicione uma referência ao Canvas principal

    void Awake()
    {
        audioSource = GameObject.FindGameObjectWithTag("ChestSound").GetComponent<AudioSource>();
        mainCanvas = GameObject.FindObjectOfType<Canvas>(); // Encontre o Canvas principal na cena
        pnm = GameObject.FindGameObjectWithTag("NavMesh").GetComponent<PetNavMesh>();
    }

    void Start()
    {
        aberto = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        gc = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControllerScript>();
    }

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
                    audioSource.clip = gemAudio;
                    audioSource.Play();
                    switch (item)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            Debug.Log("Você ganhou uma GEMA!");
                            gc.score += gemScore;
                            gameObject.GetComponent<SpriteRenderer>().sprite = diamondChest;
                            break;
                        case 4:
                            Debug.Log("Você recuperou VIDA!");
                            if (ps.life + 10 >= 100)
                            {
                                ps.life = 100;
                            }
                            else if (ps.life < 100)
                            {
                                ps.life += 10;
                            }
                            break;
                        case 5:
                            Debug.Log("Você ganhou um UPGRADE DE DANO!");
                            ps.attackDamage += 5;
                            gameObject.GetComponent<SpriteRenderer>().sprite = damageChest;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            Debug.Log("Você ganhou MATERIAIS!");
                            gc.scrap += scrapFound;
                            gameObject.GetComponent<SpriteRenderer>().sprite = gearChest;
                            break;
                        case 9:
                            Debug.Log("Você ganhou um PET NOVO!");
                            gameObject.GetComponent<SpriteRenderer>().sprite = petChest;
                            pnm.comecar();
                            break;
                        default:
                            break;
                    }
                    StartCoroutine(DeactivateAfterAudio());
                }
                else Debug.Log("Baú já aberto");
            }
        }
    }

    private void ShowFloatingText(string message)
    {
        if (floatingTextPrefab)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            TextMeshPro textMesh = floatingText.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = message;
            }
            else
            {
                Debug.LogError("TextMeshPro component not found on the floating text prefab!");
            }
        }
        else
        {
            Debug.LogError("FloatingTextPrefab is not assigned in the inspector!");
        }
    }


    private IEnumerator DeactivateAfterAudio()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
