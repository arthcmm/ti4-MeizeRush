using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RubyScript : MonoBehaviour
{
    private bool podeSair = false;
    public Vector2Int randomPosition = new Vector2Int(90000, 90000);
    public AudioSource audioSource;
    public AudioClip victory;
    private PlayerScript playerScript;
    private GameObject player;
    public GameControllerScript gc;
    public GameObject pedestalPrefab;
    public GameObject rubyPrefab;
    private GameObject ruby;
    private GameObject pedestal;
    public float floatAmplitude = 0.5f;
    public byte[,] placedMatrix;
    public float floatFrequency = 1f;
    public Vector2 spawnAreaMin = new Vector2(
        -35, -35); // Defina valores padr�o se n�o for configurar via Inspector
    public Vector2 spawnAreaMax = new Vector2(
        35, 35); // Defina valores padr�o se n�o for configurar via Inspector
    private BoardManager boardManager;
    private float spawnCooldown = 1.0f;
    private bool spawned = false;

    void Awake()
    {
        audioSource = GameObject.FindGameObjectWithTag("VictorySound").GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        boardManager =
            GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        getRandomPosition();
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

    private void getRandomPosition()
    {
        placedMatrix =
             new byte[boardManager.boardRows, boardManager.boardColumns];

        for (int i = 0; i < boardManager.boardRows; i++)
        {
            for (int j = 0; j < boardManager.boardColumns; j++)
            {
                placedMatrix[i, j] = boardManager.map[i, j];
            }
        }
    }
    void SpawnPedestalAndRuby(Vector3 position)
    {
        // Define a posi��o de spawn do pedestal dentro da �rea de spawn
        // Vector2 spawnPosition = new Vector2(
        //     Random.Range(spawnAreaMin.x, spawnAreaMax.x),
        //     Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        // );

        // Instancia o pedestal na posi��o de spawn
        pedestal =
            Instantiate(pedestalPrefab, position, Quaternion.identity);

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

        int px = (int)player.transform.position.x;
        int py = (int)player.transform.position.y;
        if (podeSair && px == randomPosition.x && py == randomPosition.y)
        {
            playerScript.end = true;
        }

        spawnCooldown -= Time.deltaTime;
        if (!spawned)
        {
            int indexX = Random.Range(0, boardManager.boardRows - 1);
            int indexY = Random.Range(0, boardManager.boardColumns - 1);
            bool valid = System.Math.Abs(px - indexX) >= 40 || System.Math.Abs(py - indexY) >= 40;
            if (valid && placedMatrix[indexX, indexY] == 0)
            {
                // Debug.Log(valid);
                // Debug.Log("calc:  " + (px - indexX) + "  " + System.Math.Abs(px - indexX) + " y" + (py - indexY) + "  " + System.Math.Abs(py - indexY));
                Vector3 position = new Vector3(indexX, indexY, 0);
                // Debug.Log("RANDOM RUBY POS:  " + position);

                placedMatrix[indexX, indexY] = 1;
                if (spawnCooldown <= 0.0f)
                {
                    SpawnPedestalAndRuby(position);
                    spawned = true;
                    spawnCooldown = 2000.0f;
                }
            }
        }
        if (spawned == true && Vector3.Distance(player.transform.position, ruby.transform.position) <
                2 &&
            ruby.activeSelf)
        {
            Debug.Log("Pressione E para pegar o rubi");
            if (Input.GetKeyDown(KeyCode.E))
            {
                gc.score += 5000;
                audioSource.clip = victory;
                audioSource.Play();
                StartCoroutine(DeactivateAfterAudio());
                // Adicione aqui o c�digo para tocar um som, se necess�rio
                GerarSaida();
                podeSair = true;
            }
        }
    }
    private IEnumerator DeactivateAfterAudio()
    {
        // Wait for the audio to finish playing
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Deactivate the GameObject
        ruby.SetActive(false);

    }

    private void GerarSaida()
    {
        HashSet<Vector2Int> positions = boardManager.walls;
        int randomIndex = Random.Range(0, positions.Count);

        // Convertendo o HashSet para uma lista para poder acessar o elemento pelo índice
        List<Vector2Int> positionsList = new List<Vector2Int>(positions);
        randomPosition = positionsList[randomIndex];
        boardManager.tileMapVisualizer.wallTileMap.SetTile(new Vector3Int(randomPosition.x, randomPosition.y, (int)player.transform.position.z), null);

        // Chamada para pintar a posição aleatória no floorTileMap com tamanho 7
        boardManager.tileMapVisualizer.PaintFloorTiles(new List<Vector2Int> { randomPosition }, 7);

        boardManager.map[randomPosition.x, randomPosition.y] = 0;

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
