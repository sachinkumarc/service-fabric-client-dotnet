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

    internal partial class MeshGatewaysClient : IMeshGatewaysClient
    {
        /// <inheritdoc />
        public Task<VolumeResourceDescription> CreateOrUpdateMeshGatewayAsync(
            string gatewayResourceName,
            string jsonDescription,
            string apiVersion = Constants.DefaultApiVersionForResources,
            long? serverTimeout = 60,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            gatewayResourceName.ThrowIfNull(nameof(gatewayResourceName));
            jsonDescription.ThrowIfNull(nameof(jsonDescription));

            string requestId = Guid.NewGuid().ToString();
            var url = $"Resources/Gateways/{gatewayResourceName}?api-version={Constants.DefaultApiVersionForResources}";

            HttpRequestMessage RequestFunc()
            {
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(jsonDescription, Encoding.UTF8),
                };
                request.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                return request;
            }

            return this.httpClient.SendAsyncGetResponse(RequestFunc, url, VolumeResourceDescriptionConverter.Deserialize, requestId, cancellationToken);
        }
    }
}
