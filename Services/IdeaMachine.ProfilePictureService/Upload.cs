using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HttpMultipartParser;
using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Modules.Account.Repository.Context;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace IdeaMachine.ProfilePictureService
{
	public class Upload
	{
		private readonly AccountContext _accountContext;

		private readonly ILogger _logger;

		private readonly AsyncLazy<BlobContainerClient> _containerClient;

		public Upload(
			ILoggerFactory loggerFactory,
			BlobServiceClient blobServiceClient,
			AccountContext accountContext)
		{
			_accountContext = accountContext;
			_logger = loggerFactory.CreateLogger<Upload>();

			_containerClient = new AsyncLazy<BlobContainerClient>(async () =>
			{
				var client = blobServiceClient.GetBlobContainerClient("profilepictures");
				
				await client.CreateIfNotExistsAsync(PublicAccessType.Blob);

				return client;
			});
		}

		[Function(nameof(UploadPicture))]
		public async Task<HttpResponseData> UploadPicture(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "UploadPicture/{userId:guid}")] HttpRequestData req,
			Guid userId)
		{
			var multiPartData = await MultipartFormDataParser.ParseAsync(req.Body);

			if (!multiPartData.Files.Any())
			{
				return req.CreateResponse(HttpStatusCode.BadRequest);
			}

			_logger.LogInformation("Received profile picture for user {UserId}", userId);

			var profilePicture = multiPartData.Files.Single();

			var containerClient = await _containerClient;

			var rawData = await BinaryData.FromStreamAsync(profilePicture.Data);

			await using var resizedImageStream = await ConvertToSmallImage(rawData);
			resizedImageStream.Position = 0;

			var resizedImageName = $"{userId}/64.png";
			
			await Task.WhenAll(
				OverwriteBlob($"{userId}/raw.png", resizedImageStream),
				OverwriteBlob(resizedImageName, rawData)
			);

			_logger.LogInformation("Uploaded profile pictures for user {UserId}", userId);

			var resizedImageUrl = $"{containerClient.Uri}/{resizedImageName}";

			await UpdateUser(userId, resizedImageUrl);

			var response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
			await response.WriteAsJsonAsync(new
			{
				ProfilePicturePath = resizedImageUrl,
			});

			return response;
		}

		private async Task OverwriteBlob(string blobName, Stream imgStream)
		{
			var containerClient = await _containerClient;

			var blobClient = containerClient.GetBlobClient(blobName);

			await blobClient.UploadAsync(imgStream, true);
		}

		private async Task OverwriteBlob(string blobName, BinaryData rawData)
		{
			var containerClient = await _containerClient;

			var blobClient = containerClient.GetBlobClient(blobName);

			await blobClient.UploadAsync(rawData, true);
		}

		private async Task UpdateUser(Guid userId, string profilePictureUrl)
		{
			var user = await _accountContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

			if (user is not null)
			{
				user.ProfilePictureUrl = profilePictureUrl;

				await _accountContext.SaveChangesAsync();
			}
		}

		private static async Task<Stream> ConvertToSmallImage(BinaryData rawData)
		{
			using var img = Image.Load(rawData);

			img.Mutate(x => x.Resize(64, 64, KnownResamplers.Lanczos3));

			var stream = new MemoryStream();
			await img.SaveAsync(stream, new PngEncoder());

			return stream;
		}
	}
}