// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabric.Client.Http
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Client.Http.Serialization;
    using Microsoft.ServiceFabric.Common;
    using Newtonsoft.Json;

    internal partial class VolumeResourceClient : IVolumeResourceClient
    {
        /// <inheritdoc />
        public Task<VolumeResourceDescription> CreateVolumeResourceAsync(
            string resourceFile,
            string volumeResourceName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            resourceFile.ThrowIfNull(nameof(resourceFile));
            volumeResourceName.ThrowIfNull(nameof(volumeResourceName));
            string content = File.ReadAllText(resourceFile);

            string requestId = Guid.NewGuid().ToString();
            var url = $"Resources/Volumes/{volumeResourceName}?api-version={Constants.DefaultApiVersionForResources}";

            HttpRequestMessage RequestFunc()
            {
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(content, Encoding.UTF8),
                };
                request.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                return request;
            }

            return this.httpClient.SendAsyncGetResponse(RequestFunc, url, VolumeResourceDescriptionConverter.Deserialize, requestId, cancellationToken);
        }
    }
}
