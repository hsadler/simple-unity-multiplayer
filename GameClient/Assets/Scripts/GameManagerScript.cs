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


    // game object refs
    public GameObject mainCamera;

    // prefab refs
    public GameObject boardSquarePrefab;

    private GameState gameState;
    private MultiplayerSync mpSync;

    // localhost game server url
    private const string GAME_SERVER_URL = "ws://localhost:5000"; 
    private const int BOARD_SIZE = 12;


    // UNITY HOOKS

    void Awake()
    {
        this.mpSync = new MultiplayerSync(GAME_SERVER_URL);
        this.mpSync.RegisterSyncFromServerHandler(this.ProcessServerGameState);
        this.gameState = new GameState();
    }

    void Start()
    {
        if(!this.gameState.boardInitialized)
        {
            this.GenerateGameBoard();
        }
    }

    void Update()
    {
        
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void GenerateGameBoard()
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
                string boardSquareId = System.Guid.NewGuid().ToString();
                var boardSquare = new BoardSquare(boardSquareId, i, j);
                bsGO.GetComponent<BoardSquareScript>().bsModel = boardSquare;
                this.gameState.board.boardSquares.Add(boardSquare);
            }
        }
        this.mpSync.SynchToServer(JsonUtility.ToJson(this.gameState));
    }

    private void ProcessServerGameState(string gameStateJson)
    {
        Debug.Log("processing game state json from game manager: " + gameStateJson);
    }


}
