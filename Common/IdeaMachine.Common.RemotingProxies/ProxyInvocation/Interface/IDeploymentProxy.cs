using System;
using System.Threading.Tasks;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Common.RemotingProxies.ProxyInvocation.Interface
{
	public interface IDeploymentProxy<out TServiceType>
		where TServiceType : IGrpcService
	{
		/// <summary>
		/// Invoke a function on a random replica
		/// </summary>
		/// <param name="invocationFunc">Function to invoke on the endpoint</param>
		/// <returns>Task representing the invocation</returns>
		Task Invoke(Func<TServiceType, Task> invocationFunc);

		/// <summary>
		/// Invoke a function on a random replica and return a result
		/// </summary>
		/// <param name="invocationFunc">Function to invoke on the endpoint</param>
		/// <returns>Task representing the invocation, containing the result</returns>
		Task<TResult> InvokeWithResult<TResult>(Func<TServiceType, Task<TResult>> invocationFunc);
	}
}