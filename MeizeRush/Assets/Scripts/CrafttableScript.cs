using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrafttableScript : MonoBehaviour {
  public GameObject menuPrefab; // Assign this in the Unity Inspector
  private GameObject menuInstance;
  private GameObject player;
  private bool isOnRange;

  void Start() { player = GameObject.FindGameObjectWithTag("Player"); }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject == player) {
      isOnRange = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject == player) {
      isOnRange = false;
      CloseMenu();
    }
  }

  private void Update() {
    if (isOnRange && Input.GetKeyDown(KeyCode.E)) {
      OpenMenu();
    }
  }

  void OpenMenu() {
    if (menuInstance == null) { // Ensure you don't create multiple menus
      menuInstance = Instantiate(menuPrefab);
    } else {
      menuInstance.SetActive(true);
    }
  }

  void CloseMenu() {
    if (menuInstance != null) {
      Destroy(menuInstance);
      menuInstance = null; // Ensure the reference is cleared
    }
  }
}
