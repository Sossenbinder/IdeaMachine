using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.ModulesServiceBase
{
	public class ServiceBase : ServiceBaseWithoutLogger
	{
		[NotNull]
		protected readonly ILogger Logger;

		protected ServiceBase([NotNull] ILogger logger)
		{
			Logger = logger;
		}
	}
}