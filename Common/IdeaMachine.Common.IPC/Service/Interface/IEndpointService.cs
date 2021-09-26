namespace IdeaMachine.Common.IPC.Service.Interface
{
	public interface IEndpointService
	{
		string GetStatelessEndpointDomainName(ServiceType serviceType);
	}
}