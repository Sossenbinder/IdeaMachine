param (
	[string] $configuration = "testing"
)

# Grab all kustomize folders matching the current configuration
$kustomizeFolders = Get-ChildItem -dir -recurse -Exclude node_modules ../../ | 
	Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "Kubernetes"}|
	ForEach-Object {
		Get-ChildItem -dir -recurse $_ |
		Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match $configuration } |
		ForEach-Object { $_.FullName }
	}

foreach ($folder in $kustomizeFolders)
{
	kubectl apply -k $folder
}

kubectl apply -f ./seq.yaml
kubectl apply -f ./rabbitmq.yaml