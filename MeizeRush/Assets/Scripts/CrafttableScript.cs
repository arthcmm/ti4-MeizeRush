using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrafttableScript : MonoBehaviour
{
    public GameObject menuPrefab; // Assign this in the Unity Inspector
    private GameObject menuInstance;
    private GameObject player;
    public GameObject mesa;
    private bool isOnRange;
    public GameControllerScript gameControllerScript;
    public PlayerScript playerScript;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        gameControllerScript = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControllerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isOnRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isOnRange = false;
            CloseMenu();
        }
    }

    private void Update()
    {
        if (isOnRange && Input.GetKeyDown(KeyCode.E))
        {
            //OpenMenu();
            giveItem("Trap");
            mesa.SetActive(false);
        }
    }

    void OpenMenu()
    {
        if (menuInstance == null)
        { // Ensure you don't create multiple menus
            menuInstance = Instantiate(menuPrefab);
        }
        else
        {
            menuInstance.SetActive(true);
        }
    }

    void CloseMenu()
    {
        if (menuInstance != null)
        {
            Destroy(menuInstance);
            menuInstance = null; // Ensure the reference is cleared
        }
    }

    private int canCraft(string itemName)
    {
        Dictionary<string, int> resources = new Dictionary<string, int> {
      { "Trap", 15 }, { "Item 2", 3 }, { "Item 3", 4 }
    };
        int cost = resources[itemName];
        if (gameControllerScript.scrap >= cost)
        {
            gameControllerScript.scrap -= cost;
            return gameControllerScript.scrap;
        }
        else
        {
            return -1;
        }
    }

    public void giveItem(string itemName)
    {
        if (canCraft(itemName) >= 0)
        {
            if (itemName == "Trap")
            {
                playerScript.availableTraps++;
            }
            Debug.Log("Crafted: " + itemName);
        }
        else
        {
            Debug.Log("Not enough resources to craft " + itemName);
        }
    }
}
