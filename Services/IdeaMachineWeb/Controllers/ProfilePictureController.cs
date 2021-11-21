using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdeaMachine.Common.IPC;
using IdeaMachine.Common.IPC.Service.Interface;
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
	    private readonly IEndpointService _endpointService;

	    private readonly HttpClient _httpClient;

	    private readonly IPublishEndpoint _publishEndpoint;

	    public ProfilePictureController(
		    ISessionService sessionService,
			IEndpointService endpointService,
		    HttpClient httpClient, 
		    IPublishEndpoint publishEndpoint)
			: base(sessionService)
	    {
		    _endpointService = endpointService;
		    _httpClient = httpClient;
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

		    var endpoint = _endpointService.GetStatelessEndpointDomainName(ServiceType.ProfilePictureService);

		    var uploadContent = new MultipartFormDataContent()
		    {
				new StreamContent(imageStream),
		    };

		    var response = await _httpClient.PostAsync($"http://{endpoint}:30555/api/UploadPicture/{Session.User.UserId}", uploadContent);

		    if (!response.IsSuccessStatusCode)
		    {
			    return JsonResponse.Error();
		    }

		    var responseStream = await response.Content.ReadAsStreamAsync();
		    var responseJson = await System.Text.Json.JsonSerializer.DeserializeAsync<Dictionary<string, string>>(responseStream);

		    string? profilePicturePath = null;
		    if (!(responseJson?.TryGetValue("ProfilePicturePath", out profilePicturePath) ?? false))
		    {
			    return JsonResponse.Error();
			}

		    await _publishEndpoint.Publish(new AccountProfilePictureUpdated(Session.User.UserId, profilePicturePath!));

		    return JsonResponse.Success();

	    }
	}
}
