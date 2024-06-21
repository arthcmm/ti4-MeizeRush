using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaceCraftTables : MonoBehaviour
{
    public GameObject craftTablePrefab;
    public int minObjects;
    public int maxObjects;
    public float minDistance = 5.0f;
    public Vector3 areaSize = new Vector3(50, 50, 0);

    public List<GameObject> craftingTables = new List<GameObject>();
    private BoardManager boardManager;
    private float cooldown = 1.0f;
    private bool spawned = false;

    private bool isOnRange;

    void Start()
    {
        boardManager =
            GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0.0f && !spawned)
        {
            InstantiateObjectsRandomly(); // adiciona em uma lista
            foreach (GameObject obj in craftingTables)
            {
                obj.SetActive(true);
                // Instantiate(obj);
            }
            spawned = true;
            cooldown = 2000.0f;
        }
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

    void InstantiateObjectsRandomly()
    {
        int objectsCount = Random.Range(minObjects, maxObjects + 1);
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
            } while (isCloseToWall(placedMatrix, indexX, indexY));

            Vector3 position = new Vector3(indexX, indexY, 0.1f);
            placedMatrix[indexX, indexY] = 1;
            GameObject craftingTable =
                Instantiate(craftTablePrefab, position, Quaternion.identity);
            craftingTables.Add(craftingTable);
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
