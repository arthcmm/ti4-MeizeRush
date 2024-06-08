using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyScript : MonoBehaviour
{
    private PlayerScript playerScript;
    private GameObject player;
    public GameControllerScript gc;
    public GameObject pedestalPrefab;
    public GameObject rubyPrefab;
    private GameObject ruby;
    private GameObject pedestal;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public Vector2 spawnAreaMin = new Vector2(
        -35, -35); // Defina valores padr�o se n�o for configurar via Inspector
    public Vector2 spawnAreaMax = new Vector2(
        35, 35); // Defina valores padr�o se n�o for configurar via Inspector
    private BoardManager boardManager;
    private float spawnCooldown = 1.0f;
    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        boardManager =
            GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }


    private bool isCloseToWall(byte[,] placedMatrix, int x, int y)
    {
        if (placedMatrix[x, y] >= 1 || placedMatrix[x + 1, y] >= 1 ||
            placedMatrix[x, y + 1] >= 1 || placedMatrix[x + 1, y + 1] >= 1)
        {
            return true;
        }
        else if (placedMatrix[x - 1, y] >= 1 || placedMatrix[x, y - 1] >= 1 ||
                   placedMatrix[x - 1, y - 1] >= 1)
        {
            return true;
        }
        return false;
    }

    private Vector3 getRandomPosition()
    {
        byte[,] placedMatrix =
            new byte[boardManager.boardRows, boardManager.boardColumns];

        for (int i = 0; i < boardManager.boardRows; i++)
        {
            for (int j = 0; j < boardManager.boardColumns; j++)
            {
                placedMatrix[i, j] = boardManager.map[i, j];
            }
        }

        int indexX = 0;
        int indexY = 0;
        do
        {
            indexX = Random.Range(0, boardManager.boardRows - 1);
            indexY = Random.Range(0, boardManager.boardColumns - 1);
        } while (isCloseToWall(placedMatrix, indexX, indexY));

        Vector3 position = new Vector3(indexX, indexY, 0);
        Debug.Log("RANDOM RUBY POS:  " + position);
        placedMatrix[indexX, indexY] = 1;
        return position;
    }
    void SpawnPedestalAndRuby()
    {
        // Define a posi��o de spawn do pedestal dentro da �rea de spawn
        // Vector2 spawnPosition = new Vector2(
        //     Random.Range(spawnAreaMin.x, spawnAreaMax.x),
        //     Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        // );

        // Instancia o pedestal na posi��o de spawn
        pedestal =
            Instantiate(pedestalPrefab, getRandomPosition(), Quaternion.identity);

        // Instancia o rubi como filho do pedestal e posiciona acima do pedestal
        ruby = Instantiate(rubyPrefab, pedestal.transform);
        ruby.transform.localPosition = new Vector3(0, 1, 0);

        // Adiciona o script de flutua��o ao rubi e configura os par�metros
        RubyFloat rubyFloat = ruby.AddComponent<RubyFloat>();
        rubyFloat.amplitude = floatAmplitude;
        rubyFloat.frequency = floatFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCooldown -= Time.deltaTime;
        if (spawnCooldown <= 0.0f && !spawned)
        {
            SpawnPedestalAndRuby();
            spawned = true;
            spawnCooldown = 2000.0f;
        }
        if (spawned == true && Vector3.Distance(player.transform.position, ruby.transform.position) <
                2 &&
            ruby.activeSelf)
        {
            Debug.Log("Pressione E para pegar o rubi");
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerScript.hasRuby = true;
                gc.score += 5000;
                // Adicione aqui o c�digo para tocar um som, se necess�rio
                ruby.SetActive(false);
                gerarSaida();
            }
        }
    }

    private void gerarSaida()
    {
        HashSet<Vector2Int> positions = boardManager.walls;
        int randomIndex = Random.Range(0, positions.Count);

        // Convertendo o HashSet para uma lista para poder acessar o elemento pelo índice
        List<Vector2Int> positionsList = new List<Vector2Int>(positions);
        Vector2Int randomPosition = positionsList[randomIndex];

        // Chamada para pintar a posição aleatória no floorTileMap com tamanho 7
        boardManager.tileMapVisualizer.PaintFloorTiles(new List<Vector2Int> { randomPosition }, 7);
    }
}

public class RubyFloat : MonoBehaviour
{
    private Vector3 startPos;
    public float amplitude = 0.2f; // Amplitude da flutua��o
    public float frequency = 1f;   // Frequ�ncia da flutua��o

    void Start()
    {
        startPos = transform.localPosition; // Posi��o inicial do rubi
    }

    void Update()
    {
        // Calcular nova posi��o do rubi
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
