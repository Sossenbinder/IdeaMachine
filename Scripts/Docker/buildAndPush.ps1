# Build and push all docker images within the repo
param (
	[string] $configuration = "dev"
)

# TODO: Make this work
$exclusionList = @("rabbitmq")

$dockerFiles = Get-ChildItem -recurse ../../ |
	Where-Object {$_.Name -eq "Dockerfile"}

foreach ($dockerfile in $dockerFiles) 
{
	$filePath = $dockerfile.Directory
	Write-Host "Building $filePath"

	$csprojPath = Join-Path -Path $filePath -ChildPath (Get-ChildItem $filePath | Where-Object {$_.Extension -like ".csproj"})
	if ($csprojPath.EndsWith(".csproj"))
	{
		[xml] $csproj = Get-Content $csprojPath
		$dockerContext = $csproj.Project.PropertyGroup.DockerfileContext[0].ToString() + "\";
	
		if ($null -eq $dockerContext)
		{
			$dockerContext = "."
		}
	
		$dockerImageName = $csproj.GetElementsByTagName("DockerImageName").InnerText
	
		$imageName = $dockerImageName
	}
	else 
	{
		$imageName = $filePath.Name;
		$dockerContext = $filePath;
	}

	$imageName = "$(If ($configuration -eq "dev") { "localhost:5000" } Else { "ideamachine.azurecr.io"})/${imageName}:${configuration}"

	Write-Host $imageName

	Write-Host "docker build -t $imageName -f $($dockerfile.FullName) $dockerContext"

	docker build -t $imageName -f $dockerfile.FullName $dockerContext/.
	docker push $imageName
}