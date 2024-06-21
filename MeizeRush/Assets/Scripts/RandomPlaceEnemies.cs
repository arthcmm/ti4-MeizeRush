using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomPlaceEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public int maxEnemy = 30;
    public int minObjects;
    public int maxObjects;
    public float minDistance = 5.0f;
    public Vector3 areaSize = new Vector3(50, 50, 0);
    public List<GameObject> enemies = new List<GameObject>();
    private BoardManager boardManager;
    private float cooldown = 1.0f;

    private bool isOnRange;

    void Start()
    {
        boardManager =
            GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
        InstantiateObjectsRandomly(); // adiciona em uma lista
        enemyCount = 0;


    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0.0f)
        {
            foreach (GameObject obj in enemies)
            {
                if (Random.Range(0, 100) % 2 == 0 && enemyCount <= maxEnemy && !obj.activeSelf)
                {
                    obj.transform.position = GenerateRandomPos();
                    obj.SetActive(true);
                    enemyCount++;
                }

            }
            cooldown = 5.0f;
        }
    }
    Vector3 GenerateRandomPos()
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

        // // Select a random position from the potential positions
        int indexX = 0;
        int indexY = 0;
        do
        {
            indexX = Random.Range(0, boardManager.boardRows - 1);
            indexY = Random.Range(0, boardManager.boardColumns - 1);
        } while (placedMatrix[indexX, indexY] >= 1);
        // // Remove nearby positions to enforce minimum distance

        // o ideal seria dar um sort pra quantidade de inimigos e, aqui caso a
        // dist�ncia sera ruim, recalcular at� funcionar.

        Vector3 position = new Vector3(indexX, indexY, 0);
        placedMatrix[indexX, indexY] = 1;
        return position;
    }

    void InstantiateObjectsRandomly()
    {
        // int objectsCount = Random.Range(minObjects, maxObjects + 1);
        int objectsCount = maxEnemy;
        // List<Vector3> potentialPositions = GeneratePotentialPositions();
        // List<Vector3> chosenPositions = new List<Vector3>();
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
            // if (potentialPositions.Count == 0)
            //     break;
            //
            // // Select a random position from the potential positions
            int indexX = 0;
            int indexY = 0;
            do
            {
                indexX = Random.Range(0, boardManager.boardRows - 1);
                indexY = Random.Range(0, boardManager.boardColumns - 1);
            } while (placedMatrix[indexX, indexY] >= 1);
            // Vector3 position = potentialPositions[index];
            // chosenPositions.Add(position);
            // potentialPositions.RemoveAt(index);
            //
            // // Remove nearby positions to enforce minimum distance
            // potentialPositions.RemoveAll(p => Vector3.Distance(p, position) <
            //                                   minDistance);
            // o ideal seria dar um sort pra quantidade de inimigos e, aqui caso a
            // dist�ncia sera ruim, recalcular at� funcionar.

            Vector3 position = new Vector3(indexX, indexY, 0);
            placedMatrix[indexX, indexY] = 1;

            // enemyPrefab.SetActive(false);
            // enemies.Add(
            //     enemyPrefab); // adiciona tudo numa lista (confia que vai ser util)
            GameObject enemy =
                Instantiate(enemyPrefab, position, Quaternion.identity);
            enemy.SetActive(false);
            enemies.Add(enemy);
            // Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }

    List<Vector3> GeneratePotentialPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        int gridSize =
            Mathf.CeilToInt(Mathf.Max(areaSize.x, areaSize.y) / minDistance);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 pos = new Vector3(
                    -areaSize.x / 2 + (x * minDistance + minDistance / 2),
                    -areaSize.y / 2 + (y * minDistance + minDistance / 2), 0);

                if (pos.x <= areaSize.x / 2 && pos.z <= areaSize.y / 2)
                    positions.Add(pos);
            }
        }

        return positions;
    }
}
