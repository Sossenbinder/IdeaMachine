using System.Net;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace IdeaMachine.Common.Logging.Log
{
    public class ServiceNameEnricher : ILogEventEnricher
    {
	    private readonly string _serviceName;

	    public ServiceNameEnricher()
	    {
		    var entryAssembly = Assembly.GetEntryAssembly();
			
		    _serviceName = entryAssembly?.GetName().Name ?? Dns.GetHostName();
	    }

	    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	    {
		    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ServiceName", _serviceName));
	    }
    }
}
