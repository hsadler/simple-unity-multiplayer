using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState
{

    public Board board;
    public List<Player> players;
        
    public GameState() {
        this.board = new Board();
        this.players = new List<Player>();
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

    public BoardSquare(int posX, int posY)
    {
        this.boardSquareId = System.Guid.NewGuid().ToString();
        this.positionX = posX;
        this.positionY = posY;
        this.ownerPlayerName = null;
    }
}


public class Player
{

    public string playerName;

    public Player() { }

}
