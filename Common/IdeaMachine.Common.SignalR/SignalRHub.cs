using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace IdeaMachine.Common.SignalR
{
	public class SignalRHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			var userId = GetUserId();
			if (userId is not null)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, userId);
			}
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var userId = GetUserId();
			if (userId is not null)
			{
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
			}
		}

		private string? GetUserId()
		{
			var httpContext = Context.GetHttpContext();
			var userIdentity = httpContext.User.Identity;

			if (userIdentity?.IsAuthenticated ?? false)
			{
				return userIdentity.Name;
			}

			httpContext.Request.Cookies.TryGetValue("AnonId", out var anonCookieValue);
			return anonCookieValue;
		}
	}
}