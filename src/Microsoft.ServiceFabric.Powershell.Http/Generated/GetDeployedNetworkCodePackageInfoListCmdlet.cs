// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabric.Powershell.Http
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using Microsoft.ServiceFabric.Common;

    /// <summary>
    /// Gets the list of Service Fabric code packages deployed to a Service Fabric node associated with a Service Fabric
    /// container network.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "SFDeployedNetworkCodePackageInfoList", DefaultParameterSetName = "GetDeployedNetworkCodePackageInfoList")]
    public partial class GetDeployedNetworkCodePackageInfoListCmdlet : CommonCmdletBase
    {
        /// <summary>
        /// Gets or sets NodeName. The name of the node.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "GetDeployedNetworkCodePackageInfoList")]
        public NodeName NodeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets NetworkName. The name of a Service Fabric container network. A network name serves as the identity of
        /// a container network and is case-sensitive.
        /// </summary>
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "GetDeployedNetworkCodePackageInfoList")]
        public string NetworkName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets MaxResults. The maximum number of results to be returned as part of the paged queries. This parameter
        /// defines the upper bound on the number of results returned. The results returned can be less than the specified
        /// maximum results if they do not fit in the message as per the max message size restrictions defined in the
        /// configuration. If this parameter is zero or not specified, the paged query includes as many results as possible
        /// that fit in the return message.
        /// </summary>
        [Parameter(Mandatory = false, Position = 2, ParameterSetName = "GetDeployedNetworkCodePackageInfoList")]
        public long? MaxResults
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ServerTimeout. The server timeout for performing the operation in seconds. This timeout specifies the
        /// time duration that the client is willing to wait for the requested operation to complete. The default value for
        /// this parameter is 60 seconds.
        /// </summary>
        [Parameter(Mandatory = false, Position = 3, ParameterSetName = "GetDeployedNetworkCodePackageInfoList")]
        public long? ServerTimeout
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void ProcessRecordInternal()
        {
            try
            {
                var continuationToken = ContinuationToken.Empty;
                do
                {
                    var result = this.ServiceFabricClient.MeshNetworks.GetDeployedNetworkCodePackageInfoListAsync(
                        nodeName: this.NodeName,
                        networkName: this.NetworkName,
                        continuationToken: continuationToken,
                        maxResults: this.MaxResults,
                        serverTimeout: this.ServerTimeout,
                        cancellationToken: this.CancellationToken).GetAwaiter().GetResult();

                    var count = 0;
                    foreach (var item in result.Data)
                    {
                        count++;
                        this.WriteObject(this.FormatOutput(item));
                    }

                    continuationToken = result.ContinuationToken;
                    this.WriteDebug(string.Format(Resource.MsgCountAndContinuationToken, count, continuationToken));
                }
                while (continuationToken.Next);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <inheritdoc/>
        protected override object FormatOutput(object output)
        {
            return output;
        }
    }
}