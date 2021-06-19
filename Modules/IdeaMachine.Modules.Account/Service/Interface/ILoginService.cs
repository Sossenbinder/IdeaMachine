﻿using System;
using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
	public interface ILoginService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse<LoginResult>> Login(LoginModel loginModel);

		[OperationContract]
		Task Logout(IUserSession session);

		[OperationContract]
		Task RefreshLogin(RefreshLoginModel refreshLoginModel);
	}
}