using Microsoft.Extensions.Logging;

namespace IdeaMachine.ModulesServiceBase
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