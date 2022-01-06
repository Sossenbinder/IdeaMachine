using System.Linq;
using System.Reflection;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.RuntimeSerialization.Serialize.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using ProtoBuf.Meta;

namespace IdeaMachine.Common.RuntimeSerialization.Serialize
{
	public class UserSessionContainerSerializer : ISectionSerializer
	{
		public void SerializeSection()
		{
			var containerType = typeof(UserSessionContainer);
			var index = StaticSerializationInfo.UserSessionContainerStartIndex;

			var containerImplementers = Assembly
				.GetEntryAssembly()!
				.GetReferencedAssemblies()
				.Select(Assembly.Load)
				.SelectMany(x => x.GetTypes())
				.Where(x => x.BaseType == containerType)
				.OrderBy(type => type.Name)
				.ToList();

			if (containerImplementers.IsNullOrEmpty())
			{
				return;
			}

			var baseMetaData = RuntimeTypeModel.Default.Add(containerType);

			foreach (var implementer in containerImplementers)
			{
				baseMetaData.AddSubType(index, implementer);
				index++;
			}
		}
	}
}