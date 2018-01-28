# Build the API
dotnet publish --configuration Release -o obj/Docker/publish
Remove-Item obj/Docker/publish/appsettings.json

# Build the docker container
docker build -t hva-innovation/api .
