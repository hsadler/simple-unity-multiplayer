
build:
	docker build -t simple-unity-multiplayer:dev -f Dockerfile.dev .

up:
	docker-compose -f docker-compose.dev.yaml up

prod-deploy: heroku-push heroku-release

prod-info:
	heroku apps:info simple-unity-multiplayer

prod-logs:
	heroku logs -a simple-unity-multiplayer --tail

heroku-push:
	heroku container:push web -a simple-unity-multiplayer

heroku-release:
	heroku container:release web -a simple-unity-multiplayer

