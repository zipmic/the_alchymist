using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public enum GameMode
{
    MainMenu,
    ActionSelection,
    GameRunning
}

public enum ActionType
{
    None,
    Forward,
    Backwards,
    RotateLeft45,
    RotateRight45,
    RotateLeft90,
    RotateRight90,
    Shoot,
    Stop
}

public class GameManager : MonoBehaviour
{
    private bool player1Done;
    private bool player2Done;
    private GameMode mode;
    private SerialPort stream;

    // Selection-mode
    private int currentPlayerNum;

    private int currentCard;
    private int maxCards;
    private ActionType[] player1Cards;
    private ActionType[] player2Cards;

    [SerializeField]
    private GameObject selectionScreen;
    [SerializeField]
    private PlayerController player1;
    [SerializeField]
    private PlayerController player2;
    [SerializeField]
    private PlayerCardSelection playerCardSelection1;
    [SerializeField]
    private PlayerCardSelection playerCardSelection2;

    private Dictionary<string, ActionType> actionMap = new Dictionary<string, ActionType>
    {
        { " 60 95 D3 A5", ActionType.Forward },
        { " B4 03 DA 1D", ActionType.Backwards },
        { " C6 85 92 10", ActionType.RotateLeft45 },
        { " 84 96 4B 1D", ActionType.RotateRight45 },
        { " A4 23 4C 1D", ActionType.Stop },
        { " A4 BB 0A 1D", ActionType.Shoot }
    };

    void Start ()
    {
        mode = GameMode.ActionSelection;
        currentCard = 0;
        maxCards = 5;
        player1Cards = new ActionType[maxCards];
        player2Cards = new ActionType[maxCards];

        stream = new SerialPort("COM3", 9600);
        stream.ReadTimeout = 100;
        stream.Open();

        StartCoroutine(CheckForCards());
    }

    void Update()
    {
        if (mode == GameMode.GameRunning && player1Done && player2Done)
        {
            mode = GameMode.ActionSelection;
            selectionScreen.SetActive(true);
            StartCoroutine(CheckForCards());
        }
    }

    public void PlayerDone(string tag)
    {
        if (tag.Equals("Player 1"))
        {
            player1Done = true;
        }
        else if (tag.Equals("Player 2"))
        {
            player2Done = true;
        }
    }

    IEnumerator CheckForCards()
    {
        yield return new WaitForSeconds(0.0f);

        if (mode == GameMode.ActionSelection)
        {
            if (stream.IsOpen)
            {
                try
                {
                    var value = stream.ReadLine();
                    Debug.Log(value);
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (actionMap.ContainsKey(value))
                        {
                            ActionType action = actionMap[value];
                            // A card has been registered!
                            if (currentPlayerNum == 0)
                            {
                                player1Cards[currentCard++] = action;
                                playerCardSelection1.LightUpNext();
                            }
                            else
                            {
                                player2Cards[currentCard++] = action;
                                playerCardSelection2.LightUpNext();
                            }

                            // Are we done with registering the player's cards?
                            if (currentCard == 5)
                            {
                                currentCard = 0;
                                if (currentPlayerNum == 1) // The second player is done with his cards
                                {
                                    currentPlayerNum = 0;
                                    mode = GameMode.GameRunning;
                                    selectionScreen.SetActive(false);
                                    player1.ExecuteActions(new Queue<ActionType>(player1Cards));
                                    player2.ExecuteActions(new Queue<ActionType>(player2Cards));
                                    player1Done = false;
                                    player2Done = false;
                                    playerCardSelection1.ClearAll();
                                    playerCardSelection2.ClearAll();
                                }
                                else
                                    currentPlayerNum++;
                            }
                        }
                    }
                }
                catch (TimeoutException e)
                {
                }
            }
            StartCoroutine(CheckForCards());
        }
    }
}
