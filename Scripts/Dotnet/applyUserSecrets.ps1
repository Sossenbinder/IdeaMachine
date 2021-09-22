param (
	[string] $secretKey,
	[string] $secretValue
)

$serviceFolders = @("../../Services/IdeaMachine.AccountService");
$serviceFolders += "../../IdeaMachine";

foreach ($folder in $serviceFolders)
{
	Set-Location $folder.FullName;
	
	dotnet user-secrets set $secretKey $secretValue
}
