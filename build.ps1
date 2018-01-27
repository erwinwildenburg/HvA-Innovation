# Build the API
dotnet publish --configuration Release src/API/API.csproj -o obj/Docker/publish
Remove-Item src/API/obj/Docker/publish/appsettings.json -ErrorAction SilentlyContinue

# Build the dashboard
Set-Location src/Dashboard
npm install
ng build --prod
Set-Location ../../

# Build the IdentityServer
dotnet publish --configuration Release src/IdentityServer/IdentityServer.csproj -o obj/Docker/publish
Remove-Item src/IdentityServer/obj/Docker/publish/appsettings.json -ErrorAction SilentlyContinue

# Create docker images
docker-compose build
