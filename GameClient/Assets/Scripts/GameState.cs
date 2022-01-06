using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState
{

    public Board board;
    public List<Player> players;
        
    public GameState() { }

}


public class Board
{

    public List<BoardSquare> boardSquares;

    public Board() { }

}


public class BoardSquare
{

    public int positionX;
    public int positionY;
    public string ownerPlayerName;

    public BoardSquare(int posX, int posY)
    {
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
