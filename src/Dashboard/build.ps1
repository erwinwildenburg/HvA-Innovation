# Build the Dashboard
npm install
ng build --prod

# Build the docker container
docker build -t hva-innovation/dashboard .
