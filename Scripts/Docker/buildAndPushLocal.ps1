# Build and push all docker images within the repo

# TODO: Make this work
#$exclusionList = @("*node_modules*","*bin*","*obj*")

$dockerFiles = Get-ChildItem -recurse ../../ |
	Where-Object {$_.Name -eq "Dockerfile"}

foreach ($dockerfile in $dockerFiles) 
{
	$filePath = $dockerfile.Directory
	Write-Host "Building $filePath"

	$csprojPath = Join-Path -Path $filePath -ChildPath (Get-ChildItem $filePath | Where-Object {$_.Extension -like ".csproj"})

	[xml] $csproj = Get-Content $csprojPath
	$dockerContext = $csproj.Project.PropertyGroup.DockerfileContext[0].ToString() + "\";

	if ($null -eq $dockerContext)
	{
		$dockerContext = "."
	}

	$dockerImageName = $csproj.GetElementsByTagName("DockerImageName").InnerText

	$imageName = "localhost:5000/$dockerImageName"

	Write-Host "docker build -t $imageName -f $($dockerfile.FullName) $dockerContext"

	docker build -t $imageName -f $dockerfile.FullName $dockerContext/.
	docker push $imageName
}