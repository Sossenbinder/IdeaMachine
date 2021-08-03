param (
	[string] $clientId,
	[string] $clientPassword,
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

#docker network connect "kind" "kindRegistry"

helm install ideamachinekeyvaultsecrets ./helm --set clientId=$clientId, clientPassword=$clientPassword

kubectl create secret docker-registry acrimgpullsecret --docker-server=https://ideamachine.azurecr.io --docker-username=$clientId --docker-password=$clientPassword

kubectl apply -f ./rabbitmq.yaml
kubectl apply -f ../../IdeaMachine/Kubernetes/IdeaMachineWeb.yaml