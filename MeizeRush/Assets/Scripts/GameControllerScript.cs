using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {
  public int mapSize = 50;
  public int scrap;
  public int score;
  public Text gameScore;
  public Text gameScrap;
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
  void Start() {
    boardManager =
        GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
    // int chestNumber = Random.Range(2, 6); // valores aleatorios
    // spawnChests(chestNumber);
    spawnPlayer();
  }

  // Update is called once per frame
  void Update() {
    score = Mathf.Clamp(score, 0, 9999);
    scrap = Mathf.Clamp(scrap, 0, 999);
    gameScore.text = score.ToString();
    gameScrap.text = scrap.ToString();
    if (player.hasRuby) {
      endgame();
    }

    cooldown -= Time.deltaTime;
    if (cooldown <= 0.0f && !spawned) {
      InstantiateObjectsRandomly(); // adiciona em uma lista
      foreach (GameObject obj in chests) {
        obj.SetActive(true);
        // Instantiate(obj);
      }
      spawned = true;
      cooldown = 2000.0f;
    }
  }

  void spawnPlayer() {
    bool isWall = true;
    Vector2Int element = new Vector2Int(0, 0);
    while (isWall) {
      element = boardManager.roomFloors.ElementAt(
          Random.Range(0, boardManager.roomFloors.Count));
      if (!boardManager.walls.Contains(element)) {
        isWall = false;
      }
    }
    // Debug.Log("RANDOM FLOOR POS:  " + element);
    Vector3 newPos = new Vector3(element.x, element.y, 0);
    playerTransform.position = newPos;
  }

  void InstantiateObjectsRandomly() {
    int objectsCount = Random.Range(minChests, maxChests + 1);
    byte[,] placedMatrix =
        new byte[boardManager.boardRows, boardManager.boardColumns];

    for (int i = 0; i < boardManager.boardRows; i++) {
      for (int j = 0; j < boardManager.boardColumns; j++) {
        placedMatrix[i, j] = boardManager.map[i, j];
      }
    }

    for (int i = 0; i < objectsCount; i++) {
      int indexX = 0;
      int indexY = 0;
      do {
        indexX = Random.Range(0, boardManager.boardRows - 1);
        indexY = Random.Range(0, boardManager.boardColumns - 1);
      } while (placedMatrix[indexX, indexY] >= 1);

      Vector3 position = new Vector3(indexX, indexY, 0);
      placedMatrix[indexX, indexY] = 1;
      GameObject chestI = Instantiate(chest, position, Quaternion.identity);
      chests.Add(chestI);
    }
  }

  void endgame() {}
}
