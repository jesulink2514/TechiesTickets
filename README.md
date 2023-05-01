# Sample Micro-Services Application: TechiesTickets


dapr run --app-port 5001 --app-id event-management --app-protocol http --dapr-http-port 3500 --resources-path "C:\Users\jesus\.dapr\components" -- dotnet run

az containerapp env dapr-component set --name TechiesTicketsACAEnvironment --resource-group ACA_sample --dapr-component-name events --yaml "./events-store.yaml"

az containerapp create \
  --name registration \
  --resource-group ACA_sample \
  --environment TechiesTicketsACAEnvironment \
  --image somostechies.azurecr.io/techiestickets-registration:1.0.0 \
  --target-port 80 \
  --ingress 'external' \
  --registry-server somostechies.azurecr.io \
  --enable-dapr \
  --dapr-app-id registration \
  --dapr-app-port 80 \
  --query properties.configuration.ingress.fqdn

  az containerapp create \
  --name notifications \
  --resource-group ACA_sample \
  --environment TechiesTicketsACAEnvironment \
  --image somostechies.azurecr.io/techiestickets-notifications:1.0.0 \
  --target-port 80 \
  --ingress 'internal' \
  --registry-server somostechies.azurecr.io \
  --enable-dapr \
  --dapr-app-id notifications \
  --dapr-app-port 80 \
  --query properties.configuration.ingress.fqdn