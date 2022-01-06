using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{


    // TODO:
    //- procedurally build environment
    //- create serializable game-state object
    //- create game-state-synch service:
    //      - synchs game state object to server
    //      - synchs game state object from server
    //- receive player inputs:
    //      - cause scene changes
    //      - mutate game-state
    //- event hooks:
    //      - received update of game-state from server must update client
    //          game-state and scene
    //      - mutations to game-state from scene must automatically call for a
    //          synch to server

    void Start()
    {
        
    }

    void Update()
    {
        
    }


}
