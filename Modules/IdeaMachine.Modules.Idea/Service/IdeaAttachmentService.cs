﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using Microsoft.AspNetCore.Http;
using ISession = IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface.ISession;

namespace IdeaMachine.Modules.Idea.Service
{
    public class IdeaAttachmentService : IIdeaAttachmentService
    {
        private readonly AsyncLazy<BlobContainerClient> _containerClient;

        private readonly IIdeaRepository _ideaRepository;

        public IdeaAttachmentService(
            BlobServiceClient blobServiceClient,
            IIdeaRepository ideaRepository)
        {
            _ideaRepository = ideaRepository;
            _containerClient = new AsyncLazy<BlobContainerClient>(async () =>
            {
                var client = blobServiceClient.GetBlobContainerClient("ideaattachments");

                await client.CreateIfNotExistsAsync(PublicAccessType.Blob);

                return client;
            });
        }

        public async Task UploadAttachments(ISession session, IFormFileCollection files, int ideaId)
        {
            var client = await _containerClient;

            var uris = await Task.WhenAll(files.Select(async x =>
            {
                var blobClient = client.GetBlobClient($"{session.User.UserId}/{ideaId}/{x.FileName}");

                await using var readStream = x.OpenReadStream();
                await blobClient.UploadAsync(readStream, true);

                return blobClient.Uri;
            }));

            await _ideaRepository.AddAttachmentUrls(ideaId, uris.Select(x => x.ToString()));
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