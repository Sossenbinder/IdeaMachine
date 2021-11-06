using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using ISession = IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface.ISession;

namespace IdeaMachine.Modules.Idea.Service
{
    public class IdeaAttachmentService : IIdeaAttachmentService
    {
        private readonly AsyncLazy<BlobContainerClient> _containerClient;

        private readonly IIdeaRepository _ideaRepository;

        private readonly string _environmentName;

        public IdeaAttachmentService(
            BlobServiceClient blobServiceClient,
            IIdeaRepository ideaRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _ideaRepository = ideaRepository;
            _environmentName = hostingEnvironment.EnvironmentName;
            _containerClient = new AsyncLazy<BlobContainerClient>(async () =>
            {
                var client = blobServiceClient.GetBlobContainerClient("ideaattachments");

                var test = await client.ExistsAsync();

                await client.CreateIfNotExistsAsync(PublicAccessType.Blob);

                return client;
            });
        }

        public async Task<List<AttachmentUrlModel>> UploadAttachments(ISession session, IFormFileCollection files, int ideaId)
        {
            var client = await _containerClient;

            var uris = await Task.WhenAll(files.Select(async x =>
            {
                var blobClient = client.GetBlobClient($"{session.User.UserId}/{ideaId}/{x.FileName}");

                await using var readStream = x.OpenReadStream();
                await blobClient.UploadAsync(readStream, true);

                if (_environmentName == Environments.Development && blobClient.Uri.ToString().Contains("ideamachine.azurite"))
                {
	                return new Uri(blobClient.Uri.ToString().Replace("ideamachine.azurite", "127.0.0.1"));
                }

                return blobClient.Uri;
            }));

            var attachments = await _ideaRepository.AddAttachmentUrls(ideaId, uris.Select(x => x.ToString()));

            return attachments.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<string>> GetAttachments(ISession session, int ideaId)
        {
            var client = await _containerClient;

            var attachments = await client.GetBlobsAsync(prefix: $"{session.User.UserId}/{ideaId}").ConsumeAsEnumerable();

            return attachments.Select(x => $"{client.Uri.AbsoluteUri}/{x.Name}").ToList();
        }

        public async Task<ServiceResponse> RemoveAttachment(ISession session, int ideaId, int attachmentId)
        {
            var client = await _containerClient;

            var attachmentUrl = await _ideaRepository.GetAttachmentUrl(ideaId, attachmentId);

            if (attachmentUrl?.Idea.Creator != session.User.UserId)
            {
                return ServiceResponse.Failure("This is not your message");
            }

            var fileNameStartIndex = attachmentUrl.AttachmentUrl.LastIndexOf("ideaattachments/", StringComparison.Ordinal) + "ideaattachments/".Length;
            var fileName = attachmentUrl!.AttachmentUrl[fileNameStartIndex..];

            var deletionSuccess = await client.DeleteBlobIfExistsAsync(fileName);

            if (!deletionSuccess)
            {
                return ServiceResponse.Failure("This is not your message");
            }

            await _ideaRepository.DeleteAttachmentUrl(attachmentUrl);
            return ServiceResponse.Success();
        }
    }
}