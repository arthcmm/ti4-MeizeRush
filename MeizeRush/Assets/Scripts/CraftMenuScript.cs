using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMenuScript : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign in inspector
    public RectTransform buttonsContainer; // Assign the Panel or a specific container within the Panel

    private List<string> itemList = new List<string>() { "Item 1", "Item 2", "Item 3" }; // Example list
    private Button selectedButton = null; // To keep track of the selected button
    private Color selectedColor = Color.green; // Color for selected button
    private Color normalColor = Color.white; // Normal color for buttons

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        foreach (string item in itemList)
        {
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

    void SelectItem(string itemName, Button button)
    {
        if (selectedButton != null)
        {
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

    public void LogSelectedItem()
    {
        if (selectedButton != null && selectedButton.colors.selectedColor == selectedColor)
            Debug.Log("Crafted: " + selectedButton.GetComponentInChildren<TMP_Text>().text);
        else
            Debug.Log("No item selected.");
    }
}
