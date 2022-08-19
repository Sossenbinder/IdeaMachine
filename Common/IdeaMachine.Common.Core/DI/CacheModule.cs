using Autofac;
using IdeaMachine.Common.Core.Cache.Implementations;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;

namespace IdeaMachine.Common.Core.DI
{
    public class CacheModule : Module
    {
	    protected override void Load(ContainerBuilder builder)
	    {
		    builder.RegisterGeneric(typeof(IMemoryCache<,>))
			    .As(typeof(MemoryCache<,>));
	    }
    }
}
