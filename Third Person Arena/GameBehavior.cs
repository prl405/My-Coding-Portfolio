using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomExtensions;
using UnityEngine.UI;

public class GameBehavior : MonoBehaviour, IManager
{
    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;

    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }



    public string labelText = "Collect all 4 items to win.";
    public int maxItems = 4;

    public bool showWinScreen = false;
    public bool showLossScreen = false;

    private int _itemsCollected = 0;

    private void Start()
    {
        Initialize();

        InventoryList<string> inventoryList = new InventoryList<string>();
        inventoryList.SetItem("Potion");
        Debug.Log(inventoryList.item);

    }

    public void Initialize()
    {
        _state = "Manager Initiated...";
        _state.SpecialDebug();
        
        debug(_state);
        LogWithDelegate(debug);

        GameObject player = GameObject.Find("Player");
        PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
        playerBehavior.playerJump += HandlePlayerJump;
    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public static void Print(string newText)
    {
        Debug.Log(newText);
    }

    public void LogWithDelegate(DebugDelegate @delegate)
    {
        @delegate("Delegating the debug task...");
    }


    public int Items
    {
        get { return _itemsCollected; }

        set
        {
            _itemsCollected = value;
            
            if (_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";

                showWinScreen = true;
                Time.timeScale = 0f;
            }
            else
            {
                labelText = "Item found, only " + (maxItems - _itemsCollected) 
                    + "more to go.";
            }
        }
    }

    private int _playerLives = 1;

    public int Lives
    {
        get { return _playerLives; }
        set
        {
            _playerLives = value;
            Debug.LogFormat("Lives: {0}", _playerLives);

            if(_playerLives <= 0)
            {
                labelText = "You Lossed";

                showLossScreen = true;
                Time.timeScale = 0;
            }
            else
            {
                labelText = "You've been hit!";
            }
        }
    }
    
    private void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Lives: " + _playerLives);

        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected " + _itemsCollected);

        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);

        if(showWinScreen)
        {
            WinLossButton("You Won!");

            // Utilities.RestartLevel(); //Line 157
        }

        if (showLossScreen)
        {
            WinLossButton("You Lossed...");


            // try
            // {
            //     Utilities.RestartLevel(-1);
            //     debug("Level restarted successfully...");
            // }
            // catch(System.ArgumentException e)
            // {
            //     Utilities.RestartLevel(0);
            //     debug("Reverting scene to 0: " + e.ToString());
            // }
            // finally
            // {
            //     debug("Restart handled...");
            // }
        }
    }


    private void WinLossButton(string winLossMessage)
    {
        if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), winLossMessage))
        {
            Utilities.RestartLevel();
        }
    }
}
