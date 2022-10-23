using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace IdeaMachineWeb.Utils
{
	public class ExactQueryParamAttribute : Attribute, IActionConstraint
	{
		private readonly string[] _queryParams;

		public ExactQueryParamAttribute(params string[] queryParams)
		{
			_queryParams = queryParams;
		}

		public int Order => 0;

		public bool Accept(ActionConstraintContext context)
		{
			var queryParams = context.RouteContext.HttpContext.Request.Query;
			return queryParams.Count == _queryParams.Length && _queryParams.All(param => queryParams.ContainsKey(param));
		}
	}
}
