using System;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Session.Service.Interface;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[Route("ProfilePicture")]
	public class ProfilePictureController : IdentityControllerBase
	{
		private readonly IPublishEndpoint _publishEndpoint;

		public ProfilePictureController(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;
		}

		[Route("UpdateProfilePicture")]
		[HttpPost]
		public async Task<JsonResponse> UpdateProfilePicture(IFormCollection form)
		{
			if (!form.Files.Any() || form.Files[0].Length == 0)
			{
				return JsonResponse.ClientError();
			}

			await using var imageStream = form.Files[0].OpenReadStream();
			var buffer = new Memory<byte>(new byte[imageStream.Length]);
			await imageStream.ReadAsync(buffer);
			var base64Image = Convert.ToBase64String(buffer.ToArray());

			await _publishEndpoint.Publish(new AccountUpdateProfilePicture(Session.User.UserId, base64Image));

			return JsonResponse.Success();
		}
	}
}