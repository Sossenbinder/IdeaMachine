using Microsoft.Extensions.Logging;

namespace IdeaMachine.Modules.ServiceBase
{
	public class ServiceBase : ServiceBaseWithoutLogger
	{
		protected readonly ILogger Logger;

		protected ServiceBase(ILogger logger)
		{
			Logger = logger;
		}
	}
}