## About

DailyTodo is a lightweight, containerized Blazor WebAssembly application designed for tracking daily routines and tasks.

**Key Features:**
*   **Blazor WebAssembly:** Built with .NET 10 for a modern, client-side web experience.
*   **Containerized:** Runs on a minimal Nginx Alpine image.
*   **Dynamic Configuration:** Tasks are loaded from environment variables (mapped from Kubernetes ConfigMaps) at runtime, injecting them into `appsettings.json` via a custom entrypoint script. This allows for updating tasks without rebuilding the image.
*   **Kubernetes Ready:** Includes manifests for Deployment, Service, Ingress, and ConfigMap.

## Build and Deploy

### 1. Publish Application
Publish the Blazor WASM app to the `publish/` directory:
```bash
dotnet publish DailyTodo.csproj -c Release -o publish
```

### 2. Build Docker Image
Build the image using the `Dockerfile` (which copies the published assets and the entrypoint script):
```bash
docker build -t ghcr.io/mitchfen/dailytodo:latest .
```

### 3. Run Locally (Docker)
Run the container to test locally. Access at `http://localhost:5000`:
```bash
docker run -p 5000:80 --rm -it ghcr.io/mitchfen/dailytodo:latest
```
*Note: You can pass environment variables to test the config injection:*
```bash
docker run -p 5000:80 -e "DAILY_TASKS=Test Task 1,Test Task 2" --rm -it ghcr.io/mitchfen/dailytodo:latest
```

### 4. Deploy to Kubernetes
Apply the manifests to your cluster
```bash
kubectl apply -f kubernetes-manifests
```
