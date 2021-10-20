# Grab all kustomize folders matching the current configuration
$dockerFileFolders = Get-ChildItem -dir -recurse -Exclude node_modules ../../ | 
	Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "Kubernetes"}|
	ForEach-Object {
		Get-ChildItem -dir -recurse $_ |
		Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match $configuration } |
		ForEach-Object { $_.FullName }
	}