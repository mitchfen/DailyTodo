1. Publish: 
`dotnet publish DailyTodo.csproj -c Release -o publish`
2. Build image `docker build -t ghcr.io/mitchfen/dailytodo:latest .`
3. Run on Docker
`docker run -p 5000:80 --rm -it ghcr.io/mitchfen/dailytodo:latest`
