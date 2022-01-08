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

    private bool gameObjectsUpdateRequired;

    // localhost game server url
    private const string GAME_SERVER_URL = "ws://localhost:5000"; 

    // game settings
    private const int BOARD_SIZE = 12;

    private IDictionary<string, GameObject> idToBoardSquareGO;


    // UNITY HOOKS

    void Awake()
    {
        this.mpSync = new MultiplayerSync(GAME_SERVER_URL);
        this.mpSync.RegisterSyncFromServerHandler(this.SyncGameStateFromServer);
        this.gameObjectsUpdateRequired = false;
        this.idToBoardSquareGO = new Dictionary<string, GameObject>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (this.gameObjectsUpdateRequired)
        {
            this.SyncGameObjectsFromGameState();
        }
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void GenerateNewGameState()
    {
        this.gameState = new GameState();
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                string boardSquareId = System.Guid.NewGuid().ToString();
                var boardSquare = new BoardSquare(boardSquareId, i, j);
                this.gameState.board.boardSquares.Add(boardSquare);
            }
        }
        this.mpSync.SynchToServer(JsonUtility.ToJson(this.gameState));
    }

    private void SyncGameObjectsFromGameState()
    {
        // make Unity scene objects reflect the game-state
        foreach (BoardSquare bs in this.gameState.board.boardSquares)
        {
            if (this.idToBoardSquareGO.ContainsKey(bs.id))
            {
                GameObject bsGO = this.idToBoardSquareGO[bs.id];
                var bsScript = bsGO.GetComponent<BoardSquareScript>();
                bsScript.bsModel = bs;
            } else
            {
                GameObject bsGO = Instantiate(
                    this.boardSquarePrefab,
                    new Vector3(bs.positionX, bs.positionY, 0),
                    Quaternion.identity
                );
                var bsScript = bsGO.GetComponent<BoardSquareScript>();
                bsScript.bsModel = bs;
                this.idToBoardSquareGO.Add(bs.id, bsGO);
            }
        }
        this.gameObjectsUpdateRequired = false;
    }

    private void SyncGameStateFromServer(string gameStateJson)
    {
        if(gameStateJson == "")
        {
            this.GenerateNewGameState();
        } else
        {
            this.gameState = JsonUtility.FromJson<GameState>(gameStateJson);
        }
        this.gameObjectsUpdateRequired = true;
    }   

    private void SyncGameStateToServer(string gameStateJson)
    {
        this.mpSync.SynchToServer(gameStateJson);
    }


}
