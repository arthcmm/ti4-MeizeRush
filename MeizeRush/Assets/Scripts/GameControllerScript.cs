using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public int mapSize = 50;
    public int scrap;
    public int score;
    public bool paused = false;
    public Canvas win_canvas;
    public Canvas death_canvas;
    public Canvas pause_canvas;
    public Text gameScore;
    public Text gameScrap;
    public Text gameScoreFinal;
    public Transform spawnPoint;
    public GameObject chest;
    public PlayerScript player;
    private BoardManager boardManager;
    public Transform playerTransform; // Refer�ncia ao transform do jogador
    public float minDistanceToPlayer =
        10f; // Dist�ncia m�nima entre ba� e jogador
    public float minDistanceBetweenChests = 5f; // Dist�ncia m�nima entre ba�s
    public int minChests;
    public int maxChests;

    public Slider sliderHealth;
    public Slider sliderStamina;

    public List<GameObject> chests = new List<GameObject>();
    private float cooldown = 1.0f;
    private bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        boardManager =
            GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
        // int chestNumber = Random.Range(2, 6); // valores aleatorios
        // spawnChests(chestNumber);
        // spawnPlayer();
        win_canvas.gameObject.SetActive(false);
        death_canvas.gameObject.SetActive(false);
        pause_canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Clamp(score, 0, 9999);
        scrap = Mathf.Clamp(scrap, 0, 999);
        gameScore.text = score.ToString("D4");
        gameScrap.text = scrap.ToString("D3");
        if (player.end)
        {
            Endgame();
        }

        cooldown -= Time.deltaTime;
        if (cooldown <= 0.0f && !spawned)
        {
            spawnPlayer();
            InstantiateObjectsRandomly(); // adiciona em uma lista
            foreach (GameObject obj in chests)
            {
                obj.SetActive(true);
                // Instantiate(obj);
            }
            spawned = true;
            cooldown = 2000.0f;
        }

        if (player.life <= 0) // para o jogo quando o jogador perde, passar essa
                              // verificação para o gamecontroller depois
        {
            paused=true;
            death_canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private Vector3 GetRandomPosition()
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

        int indexX;

        int indexY;
        do
        {
            indexX = Random.Range(1, boardManager.boardRows - 2);
            indexY = Random.Range(1, boardManager.boardColumns - 2);
        } while (IsCloseToWall(placedMatrix, indexX, indexY));

        Vector3 position = new Vector3(indexX, indexY, 0);
        placedMatrix[indexX, indexY] = 1;

        return position;
    }

    private bool IsCloseToWall(byte[,] placedMatrix, int x, int y)
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

    void spawnPlayer()
    {
        // bool isWall = true;
        // Vector2Int element = new Vector2Int(0, 0);
        // while (isWall) {
        //   element = boardManager.roomFloors.ElementAt(
        //       Random.Range(0, boardManager.roomFloors.Count));
        //   if (!boardManager.walls.Contains(element)) {
        //     isWall = false;
        //   }
        // }
        // // Debug.Log("RANDOM FLOOR POS:  " + element);
        // Vector3 newPos = new Vector3(element.x, element.y, 0);
        playerTransform.position = GetRandomPosition();
    }

    void InstantiateObjectsRandomly()
    {
        int objectsCount = Random.Range(minChests, maxChests + 1);
        byte[,] placedMatrix =
            new byte[boardManager.boardRows, boardManager.boardColumns];

        for (int i = 0; i < boardManager.boardRows; i++)
        {
            for (int j = 0; j < boardManager.boardColumns; j++)
            {
                placedMatrix[i, j] = boardManager.map[i, j];
            }
        }

        for (int i = 0; i < objectsCount; i++)
        {
            int indexX = 0;
            int indexY = 0;
            do
            {
                indexX = Random.Range(0, boardManager.boardRows - 1);
                indexY = Random.Range(0, boardManager.boardColumns - 1);
            } while (IsCloseToWall(placedMatrix, indexX, indexY));

            Vector3 position = new Vector3(indexX, indexY, 0);
            placedMatrix[indexX, indexY] = 1;
            GameObject chestI = Instantiate(chest, position, Quaternion.identity);
            chests.Add(chestI);
        }
    }

    void Endgame()
    {
        paused=true;
        gameScoreFinal.text = gameScore.text;
        win_canvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    void Pause()
    {
        Time.timeScale = 0;
        pause_canvas.gameObject.SetActive(true);
        paused = true;
    }
    public void Unpause()
    {
    
        pause_canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
}
