using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Tasks
{
	public struct RestartingBackgroundTaskOptions
	{
		public string Name { get; set; }

		public CancellationToken? CancellationToken { get; set; }

		public Func<Exception, Task> OnException { get; set; }
	}
}