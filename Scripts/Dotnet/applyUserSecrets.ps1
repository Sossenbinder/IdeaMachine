param (
	[string] $secretKey,
	[string] $secretValue
)

$projects = @("../../Services/IdeaMachine.AccountService/IdeaMachine.AccountService.csproj");
$projects += "../../IdeaMachine/IdeaMachine.csproj";

foreach ($project in $projects)
{	
	dotnet user-secrets set $secretKey $secretValue --project $project
}
