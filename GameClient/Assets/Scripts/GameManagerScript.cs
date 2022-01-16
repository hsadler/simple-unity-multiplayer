using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{


    // game object refs
    public GameObject mainCamera;

    // prefab refs
    public GameObject boardSquarePrefab;

    public GameState gameState;
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
        this.mainCamera.transform.position = new Vector3(
            (BOARD_SIZE / 2) - 0.5f,
            (BOARD_SIZE / 2) - 0.5f,
            this.mainCamera.transform.position.z
        );
    }

    void Update()
    {
        if (this.gameObjectsUpdateRequired)
        {
            this.SyncGameObjectsFromGameState();
        }
    }

    // INTERFACE METHODS

    public void SyncGameStateToServer()
    {
        string gameStateJson = JsonUtility.ToJson(this.gameState);
        this.mpSync.SynchToServer(gameStateJson);
    }

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
    }

    private void SyncGameObjectsFromGameState()
    {
        // make Unity scene objects reflect the game-state
        foreach (BoardSquare bs in this.gameState.board.boardSquares)
        {
            if (this.idToBoardSquareGO.ContainsKey(bs.id))
            {
                GameObject boardSquareGO = this.idToBoardSquareGO[bs.id];
                boardSquareGO.GetComponent<BoardSquareScript>().bsModel = bs;
            } else
            {
                GameObject boardSquareGO = Instantiate(
                    this.boardSquarePrefab,
                    new Vector3(bs.positionX, bs.positionY, 0),
                    Quaternion.identity
                );
                var bsScript = boardSquareGO.GetComponent<BoardSquareScript>();
                bsScript.bsModel = bs;
                bsScript.gms = this;
                this.idToBoardSquareGO.Add(bs.id, bsGO);
            }
        }
        this.gameObjectsUpdateRequired = false;
    }

    private void SyncGameStateFromServer(string gameStateJson)
    {
        // create a new game state if there's not currently one being
        // distributed by the server
        if(gameStateJson == "")
        {
            this.GenerateNewGameState();
            this.SyncGameStateToServer();
        } else
        {
            this.gameState = JsonUtility.FromJson<GameState>(gameStateJson);
        }
        this.gameObjectsUpdateRequired = true;
    }


}
