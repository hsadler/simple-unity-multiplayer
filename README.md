# simple-unity-multiplayer
This project demonstrates a dead simple Unity multiplayer solution.

The game server's only responsibility is to synch the latest version of the game
state to all connected clients. All game clients have equal authority to push an 
updated version of the game state to the game server. 

Therefor, the game clients view and operate on a shared game state.

### Requirements
- Unity Editor
- Docker
- Docker Compose

### Running Locally

1. Build and spin-up the Golang game server.
```sh
cd GameServer/
docker build -t simple-unity-multiplayer:dev -f Dockerfile.dev .
docker-compose -f docker-compose.dev.yaml up
```

### Notes
This is a purely client authoritative solution, and therefor would only be 
appropriate for certain game types. 

The server does no verification that the version of the game state that was 
operated on and mutated by a client was the last version of the game state that 
the server had in memory. This poses a significant limitation and basically 
means that this is not a solution for real-time games, but could work for 
simple, turn-based games. Server verification of stepwise game state updates is 
a good candidate for a contribution to this repo. My initial thought is that 
this could be achieved via a game state checksum comparison server-side.

The server does no client disconnect management. Therefor, clients do not know 
if other clients disconnect. This needs to be managed within the game state by 
the clients. Server management of client disconnects is a good candidate for 
contribution to this repo. 
