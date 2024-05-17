using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrap : MonoBehaviour {
  public GameObject objectToPlace;  // Drag the prefab of the object to place
                                    // into this field in the Inspector
  public Transform playerTransform; // Drag the player transform into this field
                                    // in the Inspector
  public LayerMask
      placementMask; // Set up a LayerMask in the Inspector to check only the
                     // layers where objects can be placed
  private PlayerScript playerScript;

  void Start() {
    playerScript =
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.G)) {
      PlaceObject();
    }
  }

  void PlaceObject() {

    Vector3 positionToPlace = playerTransform.position;
    positionToPlace.z =
        positionToPlace.z + (float)0.01; // deixa a trap atraz do player

    // Define the size of the check area (this should match or be slightly
    // larger than the object's size)
    Vector3 checkSize = new Vector3(
        (float)1, (float)1,
        (float)1); // Adjust this size according to your sphere's size

    // Check if the area at the player's feet is clear
    Collider[] hits = Physics.OverlapBox(positionToPlace, checkSize / 2,
                                         Quaternion.identity, placementMask);
    // OverlapBox center is given by the first argument, half-extents by the
    // second, rotation by the third, and layerMask by the fourth.

    // If there is an object, destroy the first one found; otherwise, place a
    // new object
    if (hits.Length > 0) {
      if (hits[0].CompareTag("Trap")) {
        // Log and remove the first interactable object encountered
        playerScript.availableTraps++;
        Debug.Log("Removing object: " + hits[0].gameObject.name);
        Destroy(hits[0].gameObject);
      }
    } else {
      // No object found, place a new one

      if (playerScript.availableTraps > 0) {
        playerScript.availableTraps--;
        Debug.Log("Placing new object at: " + positionToPlace);
        Instantiate(objectToPlace, positionToPlace, Quaternion.identity);
      }
    }
  }

  // Optionally, draw the check area in the editor
  private void OnDrawGizmosSelected() {
    if (playerTransform != null) {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(playerTransform.position,
                          new Vector3((float)0.7, (float)0.7, (float)0));
    }
  }
}
