using IdeaMachine.Common.IPC.Service.Interface;

namespace IdeaMachine.Common.IPC.Service
{
	public class KubernetesEndpointService : IEndpointService
	{
		public string GetStatelessEndpointDomainName(ServiceType serviceType)
		{
			return $"ideamachine-{serviceType.ToString().ToLower()}.default.svc.cluster.local";
		}
	}
}
