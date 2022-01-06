using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class MultiplayerSync
{

    private WebSocket ws;

    public MultiplayerSync(string gameServerUrl) {
        this.InitWebSocketClient(gameServerUrl);
    }

    // websocket helpers

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
        // STUB
    }

    private void SendWebsocketClientMessage(string messageJson)
    {
        Debug.Log("Client message sent: " + messageJson);
        this.ws.Send(messageJson);
    }

    
}
