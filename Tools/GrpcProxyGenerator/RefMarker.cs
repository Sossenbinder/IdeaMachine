using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IdeaMachine.Modules.Account.Service.Interface;

namespace GrpcProxyGenerator
{
	public class RefMarker
	{
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public RefMarker()
		{
			var typeList = new List<Type>()
			{
				typeof(IAccountService)
			};
		}
	}
}