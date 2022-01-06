﻿using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.ModulesServiceBase.Attributes;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
	[GrpcServiceIdentifier(3)]
	public interface IRegistrationService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse<IdentityErrorCode>> RegisterAccount(RegisterModel registerModel);
	}
}