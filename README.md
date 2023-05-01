# Sample Micro-Services Application: TechiesTickets


dapr run --app-port 5001 --app-id event-management --app-protocol http --dapr-http-port 3500 --resources-path "C:\Users\jesus\.dapr\components" -- dotnet run

az containerapp env dapr-component set --name TechiesTicketsACAEnvironment --resource-group ACA_sample --dapr-component-name events --yaml "./events-store.yaml"

