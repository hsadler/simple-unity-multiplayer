using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class GameState
{

    public Board board;
        
    public GameState() {
        this.board = new Board();
    }

}


[Serializable]
public class Board
{

    public List<BoardSquare> boardSquares;

    public Board() {
        this.boardSquares = new List<BoardSquare>();
    }

}


[Serializable]
public class BoardSquare
{

    public string id;
    public int positionX;
    public int positionY;
    public bool active;

    public BoardSquare(string id, int positionX, int positionY)
    {
        this.id = id;
        this.positionX = positionX;
        this.positionY = positionY;
        this.active = false;
    }
}
