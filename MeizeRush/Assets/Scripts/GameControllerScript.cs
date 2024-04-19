using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {
  public int mapSize = 50;
  public GameObject chest;
  public Transform playerTransform; // Referência ao transform do jogador
  public float minDistanceToPlayer =
      10f; // Distância mínima entre baú e jogador
  public float minDistanceBetweenChests = 5f; // Distância mínima entre baús

  // Start is called before the first frame update
  void Start() {
    int chestNumber = Random.Range(2, 6); // valores aleatorios
    spawnChests(chestNumber);
  }

  // Update is called once per frame
  void Update() {}

  Vector3 getRandomPos() {
    float xSpawn = Random.Range(-mapSize, mapSize);
    float zSpawn = Random.Range(-mapSize, mapSize);
    return new Vector3(xSpawn, 0.49f, zSpawn);
  }

  void spawnChests(int chestNumber) {
    Vector3 spawnPos = Vector3.zero;
    List<Vector3> chestPositions = new List<Vector3>();

    for (int i = 0; i < chestNumber; i++) {
      bool validPosition = false;

      while (!validPosition) {
        spawnPos = getRandomPos();

        // Verifica se a nova posição está longe o suficiente do jogador
        if (Vector3.Distance(playerTransform.position, spawnPos) >=
            minDistanceToPlayer) {
          validPosition = true;

          // Verifica se a nova posição está longe o suficiente de outras
          // posições de baús
          foreach (Vector3 pos in chestPositions) {
            if (Vector3.Distance(pos, spawnPos) < minDistanceBetweenChests) {
              validPosition = false;
              break;
            }
          }
        }
      }

      chestPositions.Add(spawnPos);
      Instantiate(chest, spawnPos, Quaternion.Euler(90, 0, 0));
    }
  }
}
