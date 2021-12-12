using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using MassTransit;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace IdeaMachine.ProfilePictureService
{
	public class Upload
	{
		private readonly IHostEnvironment _hostEnv;

		private readonly AccountContext _accountContext;

		private readonly IPublishEndpoint _publishEndpoint;

		private readonly ILogger _logger;

		private readonly AsyncLazy<BlobContainerClient> _containerClient;

		public Upload(
			IHostEnvironment hostEnv,
			ILoggerFactory loggerFactory,
			BlobServiceClient blobServiceClient,
			AccountContext accountContext,
			IPublishEndpoint publishEndpoint)
		{
			_hostEnv = hostEnv;
			_accountContext = accountContext;
			_publishEndpoint = publishEndpoint;
			_logger = loggerFactory.CreateLogger<Upload>();

			_containerClient = new AsyncLazy<BlobContainerClient>(async () =>
			{
				var client = blobServiceClient.GetBlobContainerClient("profilepictures");
				
				await client.CreateIfNotExistsAsync(PublicAccessType.Blob);

				return client;
			});
		}

		public record MassTransitMessage(AccountUpdateProfilePicture Message);

		[Function(nameof(UploadPicture))]
		public async Task UploadPicture([RabbitMQTrigger(nameof(AccountUpdateProfilePicture), ConnectionStringSetting = "RabbitMqConnectionString")] MassTransitMessage massTransitMessage)
		{
			var (userId, base64Message) = massTransitMessage.Message;

			_logger.LogInformation("Received profile picture for user {UserId}", userId);
			var containerClient = await _containerClient;

			var rawData = BinaryData.FromBytes(Convert.FromBase64String(base64Message));

			await using var resizedImageStream = await ConvertToSmallImage(rawData);
			resizedImageStream.Position = 0;

			var resizedImageName = $"{userId}/64.png";
			
			await Task.WhenAll(
				OverwriteBlob($"{userId}/raw.png", resizedImageStream),
				OverwriteBlob(resizedImageName, rawData)
			);

			_logger.LogInformation("Uploaded profile pictures for user {UserId}", userId);

			var resizedImageUrl = $"{containerClient.Uri}/{resizedImageName}";

			if (_hostEnv.IsEnvironment(Environments.Development))
			{
				resizedImageUrl = resizedImageUrl.Replace("ideamachine.azurite", "localhost");
			}

			await UpdateUser(userId, resizedImageUrl);

			await _publishEndpoint.Publish(new AccountProfilePictureUpdated(userId, resizedImageUrl));
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