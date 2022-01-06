using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{


    // TODO:
    //- procedurally build environment
    //- create serializable game-state object
    //- create multiplayer-sync class:
    //      - syncs game state object to server
    //      - syncs game state object from server
    //- receive player inputs:
    //      - cause scene changes
    //      - mutate game-state
    //- event hooks:
    //      - received update of game-state from server must update client
    //          game-state and scene
    //      - mutations to game-state from scene must automatically call for a
    //          sync to server

    public GameObject boardSquarePrefab;

    private GameState gameState;
    private MultiplayerSync mpSync;

    private const string GAME_SERVER_URL = "mock-url";
    private const int BOARD_SIZE = 12;


    // UNITY HOOKS

    void Awake()
    {
        this.mpSync = new MultiplayerSync(GAME_SERVER_URL);
        this.gameState = new GameState();
    }

    void Start()
    {
        this.BuildGameBoard();
    }

    void Update()
    {
        
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void BuildGameBoard()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                GameObject bsGO = Instantiate(
                    this.boardSquarePrefab,
                    new Vector3(i, j, 0),
                    Quaternion.identity
                );
                var boardSquare = new BoardSquare(i, j);
                bsGO.GetComponent<BoardSquareScript>().bsModel = boardSquare;
                this.gameState.board.boardSquares.Add(boardSquare);
            }
        }
    }


}
