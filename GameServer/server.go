package main

import (
	"bytes"
	"encoding/json"
	"flag"
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/gorilla/websocket"
)

///////////////// HUB /////////////////

type Hub struct {
	Clients   map[*Client]bool
	Add       chan *Client
	Remove    chan *Client
	Broadcast chan []byte
	GameState []byte
}

func NewHub() *Hub {
	return &Hub{
		Clients:   make(map[*Client]bool),
		Add:       make(chan *Client),
		Remove:    make(chan *Client),
		Broadcast: make(chan []byte),
		GameState: nil,
	}
}

func (h *Hub) Run() {
	for {
		select {
		case client := <-h.Add:
			fmt.Println("adding client from hub")
			h.Clients[client] = true
		case client := <-h.Remove:
			fmt.Println("removing client from hub")
			delete(h.Clients, client)
			client.Cleanup()
		case message := <-h.Broadcast:
			for c := range h.Clients {
				c.Send <- message
			}
		default:
			// fmt.Println("nothing for hub to process...")
		}
	}
}

///////////////// CLIENT /////////////////

type Client struct {
	Hub  *Hub
	Ws   *websocket.Conn
	Send chan []byte
}

func NewClient(h *Hub, ws *websocket.Conn) *Client {
	return &Client{
		Hub:  h,
		Ws:   ws,
		Send: make(chan []byte, 256),
	}
}

func (cl *Client) RecieveMessages() {
	// do player removal from game state and websocket close on disconnect
	defer func() {
		fmt.Println("Client.RecieveMessages() goroutine stopping")
		cl.Ws.Close()
	}()
	for {
		// read message
		_, message, err := cl.Ws.ReadMessage()
		if err != nil {
			log.Println("read:", err)
			break
		}
		// log message received
		fmt.Println("client message received:")
		// ConsoleLogJsonByteArray(message)
		cl.Hub.Broadcast <- message
	}
}

func (cl *Client) SendMessages() {
	defer func() {
		fmt.Println("Client.SendMessages() goroutine stopping")
	}()
	for {
		select {
		case message, ok := <-cl.Send:
			if !ok {
				return
			}
			SendJsonMessage(cl.Ws, message)
		default:
			// fmt.Println("no message to send...")
		}
	}
}

func (cl *Client) Cleanup() {
	close(cl.Send)
}

///////////////// SERVER MESSAGE SENDING /////////////////

func SendJsonMessage(ws *websocket.Conn, messageJson []byte) {
	ws.WriteMessage(1, messageJson)
	// log that message was sent
	fmt.Println("server message sent:")
	ConsoleLogJsonByteArray(messageJson)
}

///////////////// RUN SERVER /////////////////

func main() {
	flag.Parse()
	log.SetFlags(log.LstdFlags)
	// create and run hub singleton
	h := NewHub()
	go h.Run()
	http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
		// upgrade request to websocket and use default options
		upgrader := websocket.Upgrader{}
		ws, err := upgrader.Upgrade(w, r, nil)
		if err != nil {
			log.Print("Request upgrade error:", err)
			return
		}
		// create client, run processes, and add to hub
		cl := NewClient(h, ws)
		go cl.RecieveMessages()
		go cl.SendMessages()
		h.Add <- cl
		// TODO: initialize client's game state by sending latest game state
	})
	addr := flag.String("addr", "0.0.0.0:5000", "http service address")
	err := http.ListenAndServe(*addr, nil)
	if err != nil {
		log.Fatal("ListenAndServe: ", err)
	}
}

///////////////// HELPERS /////////////////

func ConsoleLogJsonByteArray(message []byte) {
	var out bytes.Buffer
	json.Indent(&out, message, "", "  ")
	out.WriteTo(os.Stdout)
}
