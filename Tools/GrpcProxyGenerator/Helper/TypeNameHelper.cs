namespace GrpcProxyGenerator.Helper
{
	public static class TypeNameHelper
	{
		public static string StripGenericArtifacts(string? name)
		{
			if (name == null)
			{
				return "";
			}

			var genericOpen = name.IndexOf('[');
			if (genericOpen != -1)
			{
				name = name.Replace('[', '<');
				name = name.Replace('[', '>');
			}

			var compilerGeneratedSuffix = name.IndexOf('`');
			if (compilerGeneratedSuffix != -1)
			{
				name = name[..compilerGeneratedSuffix];
			}

			return name;
		}
	}
}