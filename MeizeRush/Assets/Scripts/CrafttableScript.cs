using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrafttableScript : MonoBehaviour {
  public GameObject player;
  public GameObject menu;

  private bool isOnRange;

  // Start is called before the first frame update
  private void OnTriggerEnter(Collider other) {
    if (other.gameObject == player) {
      // Player has entered the interaction zone
      isOnRange = true;
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.gameObject == player) {
      // Player has left the interaction zone
      isOnRange = false;
    }
  }

  private void Update()
{
    if (isOnRange)
    {
      if(Input.GetKeyDown(KeyCode.E)){
        OpenMenu();
      }
    }else{
      CloseMenu();
    }
}
void OpenMenu()
  {
    menu.SetActive(true);
  }
  void CloseMenu() {
    menu.SetActive(false);
  }
}
