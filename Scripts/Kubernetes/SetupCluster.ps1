param (
	[string] $clientId,
	[string] $clientPassword,
	[string] $dockerPassword,
	[string] $configuration = "dev"
)


kind create cluster --config ./kind/kindConfig.yaml

Start-Sleep 30

if ($configuration -ne "dev") {
	Invoke-Expression "helm install ingress-nginx ingress-nginx/ingress-nginx --namespace=ingress-nginx --create-namespace"
}
else {
	Invoke-Expression "kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/kind/deploy.yaml"
}

helm install ideamachinekeyvaultsecrets ./helm --set clientId=$clientId --set clientPassword=$clientPassword
kubectl create secret docker-registry acrimgpullsecret --docker-server=https://ideamachine.azurecr.io --docker-username=$clientId --docker-password=$clientPassword
& "./acrAccess.ps1" 

# Required dependencies
kubectl apply -f ./rabbitmq.yaml
kubectl apply -f ./genericConfig.yaml
kubectl apply -f ./seq.yaml

# Services
& "./services/DeployServices.ps1" $configuration 