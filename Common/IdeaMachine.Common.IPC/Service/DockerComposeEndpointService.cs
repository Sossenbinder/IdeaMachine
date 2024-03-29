﻿using IdeaMachine.Common.IPC.Service.Interface;

namespace IdeaMachine.Common.IPC.Service
{
	public class DockerComposeEndpointService : IEndpointService
	{
		public string GetStatelessEndpointDomainName(ServiceType serviceType)
		{
			return $"ideamachine.{serviceType.ToString().ToLower()}";
		}
	}
}