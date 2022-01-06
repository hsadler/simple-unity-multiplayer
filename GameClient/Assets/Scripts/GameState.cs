using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState
{

    public Board board;
    public bool boardInitialized;
    public List<Player> players;
        
    public GameState() {
        this.board = new Board();
        this.boardInitialized = false;
        this.players = new List<Player>();
    }

    public GameState CreateFromJson(string gameStateJson)
    {
        // TODO: maybe use this
        return new GameState();
    }

}


public class Board
{

    public List<BoardSquare> boardSquares;

    public Board() {
        this.boardSquares = new List<BoardSquare>();
    }

}


public class BoardSquare
{

    public string boardSquareId;
    public int positionX;
    public int positionY;
    public string ownerPlayerName;

    public BoardSquare(string boardSquareId, int positionX, int positionY)
    {
        this.boardSquareId = boardSquareId;
        this.positionX = positionX;
        this.positionY = positionY;
        this.ownerPlayerName = null;
    }
}


public class Player
{

    public string playerName;

    public Player() { }

}
