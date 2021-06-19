using IdeaMachine.Common.IPC.Service.Interface;

namespace IdeaMachine.Common.IPC.Service
{
	public class DockerComposeEndpointService : IEndpointService
	{
		public string GetDns(ServiceType serviceType)
		{
			return $"ideamachine.{serviceType}";
		}
	}
}