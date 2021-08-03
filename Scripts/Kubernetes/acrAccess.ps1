param (
	[string] $password
)

kubectl create secret docker-registry acrimgpullsecret --docker-server=https://ideamachine.azurecr.io --docker-username=ideamachine --docker-password=$password