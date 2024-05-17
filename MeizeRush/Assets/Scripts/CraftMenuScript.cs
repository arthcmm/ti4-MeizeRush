using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMenuScript : MonoBehaviour {
  public GameObject buttonPrefab;        // Assign in inspector
  public RectTransform buttonsContainer; // Assign the Panel or a specific
                                         // container within the Panel

  private List<string> itemList =
      new List<string>() { "Trap", "Item 2", "Item 3" }; // Example list
  private Button selectedButton = null; // To keep track of the selected button
  private Color selectedColor = Color.green; // Color for selected button
  private Color normalColor = Color.white;   // Normal color for buttons
  private GameControllerScript gameControllerScript;
  private PlayerScript playerScript;

  void Start() {
    GenerateButtons();
    gameControllerScript = GameObject.FindGameObjectWithTag("Controller")
                               .GetComponent<GameControllerScript>();
    playerScript =
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
  }

  void GenerateButtons() {
    foreach (string item in itemList) {
      GameObject buttonGO = Instantiate(buttonPrefab, buttonsContainer);
      buttonGO.GetComponentInChildren<TMP_Text>().text = item;
      Button button = buttonGO.GetComponent<Button>();
      button.onClick.AddListener(() => SelectItem(item, button));

      // Set the initial color of the button
      ColorBlock colors = button.colors;
      colors.normalColor = normalColor;
      button.colors = colors;
    }
  }

  void SelectItem(string itemName, Button button) {
    if (selectedButton != null) {
      // Reset the previous selected button color
      ColorBlock prevColors = selectedButton.colors;
      prevColors.normalColor = normalColor;
      prevColors.selectedColor = normalColor;
      selectedButton.colors = prevColors;
    }

    // Set the new selected button and change its color
    selectedButton = button;
    ColorBlock newColors = selectedButton.colors;
    newColors.normalColor = selectedColor;
    newColors.selectedColor = selectedColor;
    selectedButton.colors = newColors;
  }

  private int canCraft(string itemName) {
    Dictionary<string, int> resources = new Dictionary<string, int> {
      { "Trap", 15 }, { "Item 2", 3 }, { "Item 3", 4 }
    };
    int cost = resources[itemName];
    if (gameControllerScript.scrap >= cost) {
      gameControllerScript.scrap -= cost;
      return gameControllerScript.scrap;
    } else {
      return -1;
    }
  }

  private void giveItem(string itemName) {
    if (canCraft(itemName) >= 0) {
      if (itemName == "Trap") {
        playerScript.availableTraps++;
      }
      Debug.Log("Crafted: " + itemName);
    } else {
      Debug.Log("Not enough resources to craft " + itemName);
    }
  }

  public void LogSelectedItem() {
    if (selectedButton != null &&
        selectedButton.colors.selectedColor == selectedColor)
      giveItem(selectedButton.GetComponentInChildren<TMP_Text>().text);

    else
      Debug.Log("No item selected.");
  }
}
