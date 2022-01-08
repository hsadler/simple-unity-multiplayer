using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class MultiplayerSync
{


    private WebSocket ws;

    public delegate void GameStateHandlerDelegate(string gameStateJson);
    private GameStateHandlerDelegate gameStateHandlerDelegate;


    public MultiplayerSync(string gameServerUrl) {
        this.InitWebSocketClient(gameServerUrl);
    }

    // INTERFACE METHODS

    public void SynchToServer(string gameStateJson) {
        this.SendWebsocketClientMessage(gameStateJson);
    }

    public void RegisterSyncFromServerHandler(GameStateHandlerDelegate d) {
        this.gameStateHandlerDelegate = d;
    }

    // WEBSOCKET HELPERS

    private void InitWebSocketClient(string gameServerUrl)
    {
        // create websocket connection
        this.ws = new WebSocket(gameServerUrl);
        this.ws.Connect();
        // add message handler callback
        this.ws.OnMessage += this.ProcessServerMessage;
    }

    private void ProcessServerMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Server message received: " + e.Data);
        this.gameStateHandlerDelegate(e.Data);
    }

    private void SendWebsocketClientMessage(string messageJson)
    {
        Debug.Log("Client message sent: " + messageJson);
        this.ws.Send(messageJson);
    }

    
}
