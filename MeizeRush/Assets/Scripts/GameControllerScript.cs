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
    public Text gameScore;
    public Text gameScrap;
    public Transform spawnPoint;
    public GameObject chest;
    public PlayerScript player;
    public BoardManager boardManager;
    public Transform playerTransform; // Refer�ncia ao transform do jogador
    public float minDistanceToPlayer =
        10f; // Dist�ncia m�nima entre ba� e jogador
    public float minDistanceBetweenChests = 5f; // Dist�ncia m�nima entre ba�s

    public Slider sliderHealth;
    public Slider sliderStamina;




    // Start is called before the first frame update
    void Start()
    {
        int chestNumber = Random.Range(2, 6); // valores aleatorios
        spawnChests(chestNumber);
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Clamp(score, 0, 9999);
        scrap = Mathf.Clamp(scrap, 0, 999);
        gameScore.text = score.ToString();
        gameScrap.text = scrap.ToString();
        if (player.hasRuby)
        {
            endgame();
        }

    }

    Vector3 getRandomPos()
    {
        float xSpawn = Random.Range(-mapSize, mapSize);
        float ySpawn = Random.Range(-mapSize, mapSize);
        return new Vector3(xSpawn, ySpawn, 0);
    }
    void spawnPlayer()
    {
        bool isWall = true;
        Vector2Int element = new Vector2Int(0, 0);
        while (isWall)
        {
            element = boardManager.roomFloors.ElementAt(Random.Range(0, boardManager.roomFloors.Count));
            if (!boardManager.walls.Contains(element))
            {
                isWall = false;
            }
        }
        //Debug.Log("RANDOM FLOOR POS:  " + element);
        Vector3 newPos = new Vector3(element.x, element.y, 0);
        playerTransform.position = newPos;

    }

    void spawnChests(int chestNumber)
    {
        Vector3 spawnPos = Vector3.zero;
        List<Vector3> chestPositions = new List<Vector3>();

        for (int i = 0; i < chestNumber; i++)
        {
            bool validPosition = false; //se a validposition for false, manda um getRandomPos pra esse bau dnv

            while (!validPosition)
            {
                spawnPos = getRandomPos();

                // Verifica se a nova posi��o est� longe o suficiente do jogador
                if (Vector3.Distance(playerTransform.position, spawnPos) >=
                    minDistanceToPlayer)
                {
                    validPosition = true;

                    // Verifica se a nova posi��o est� longe o suficiente de outras
                    // posi��es de ba�s
                    foreach (Vector3 pos in chestPositions)
                    {
                        if (Vector3.Distance(pos, spawnPos) < minDistanceBetweenChests)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                }
            }

            chestPositions.Add(spawnPos);
            Instantiate(chest, spawnPos, Quaternion.identity);
        }
    }

    void endgame()
    {

    }
}
