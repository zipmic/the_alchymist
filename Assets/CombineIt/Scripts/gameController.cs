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

public class GameController : MonoBehaviour
{
    private GameMode mode;
    private SerialPort stream;

    // Selection-mode
    private int currentPlayerNum;

    private int currentCard;
    private int maxCards;
    private string[] player1Cards;
    private string[] player2Cards;

    void Start ()
    {
        mode = GameMode.ActionSelection;
        currentCard = 0;
        maxCards = 5;
        player1Cards = new string[maxCards];
        player2Cards = new string[maxCards];

        stream = new SerialPort("COM3", 9600);
        stream.Open();
    }
	
	void Update ()
    {
        if (stream.IsOpen)
        {
            var value = stream.ReadLine();

            if(!string.IsNullOrEmpty(value))
            {
                // A card has been registered!
                if (currentPlayerNum == 0)
                {
                    player1Cards[currentCard++] = value;
                }
                else
                {
                    player2Cards[currentCard++] = value;
                }

                // Are we done with registering the player's cards?
                if (currentCard == 5)
                {
                    currentCard = 0;
                    if (currentPlayerNum == 1) // The second player is done with his cards
                    {
                        mode = GameMode.GameRunning;
                    }
                }
            }
        }
	}
}
