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
        
    }

    void Update()
    {
        
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void BuildGameBoard()
    {
        // STUB
    }


}
