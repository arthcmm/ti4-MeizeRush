using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public GameObject player;
    public GameControllerScript gc;
    public GameObject pedestalPrefab;
    public GameObject rubyPrefab;
    private GameObject ruby;
    private GameObject pedestal;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public Vector2 spawnAreaMin = new Vector2(-35, -35); // Defina valores padrão se não for configurar via Inspector
    public Vector2 spawnAreaMax = new Vector2(35, 35);   // Defina valores padrão se não for configurar via Inspector

    // Start is called before the first frame update
    void Start()
    {
        SpawnPedestalAndRuby();
    }

    void SpawnPedestalAndRuby()
    {
        // Define a posição de spawn do pedestal dentro da área de spawn
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Instancia o pedestal na posição de spawn
        pedestal = Instantiate(pedestalPrefab, spawnPosition, Quaternion.identity);

        // Instancia o rubi como filho do pedestal e posiciona acima do pedestal
        ruby = Instantiate(rubyPrefab, pedestal.transform);
        ruby.transform.localPosition = new Vector3(0, 1, 0);

        // Adiciona o script de flutuação ao rubi e configura os parâmetros
        RubyFloat rubyFloat = ruby.AddComponent<RubyFloat>();
        rubyFloat.amplitude = floatAmplitude;
        rubyFloat.frequency = floatFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 2 && ruby.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerScript.hasRuby = true;
                gc.score += 5000;
                // Adicione aqui o código para tocar um som, se necessário
                ruby.SetActive(false);
            }
        }
    }
}

public class RubyFloat : MonoBehaviour
{
    private Vector3 startPos;
    public float amplitude = 0.2f; // Amplitude da flutuação
    public float frequency = 1f; // Frequência da flutuação

    void Start()
    {
        startPos = transform.localPosition; // Posição inicial do rubi
    }

    void Update()
    {
        // Calcular nova posição do rubi
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
