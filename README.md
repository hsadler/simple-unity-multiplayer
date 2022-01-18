# simple-unity-multiplayer
This project demonstrates a dead simple Unity multiplayer solution.

The game server's only responsibility is to synch the latest version of the game
state to all connected clients. All game clients have equal authority to push an 
update of the game state to the game server. 

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