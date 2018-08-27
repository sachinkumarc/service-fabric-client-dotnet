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
    /// Gets the manifest describing a service type.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "SFServiceManifest", DefaultParameterSetName = "GetServiceManifest")]
    public partial class GetServiceManifestCmdlet : CommonCmdletBase
    {
        /// <summary>
        /// Gets or sets ApplicationTypeName. The name of the application type.
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "GetServiceManifest")]
        public string ApplicationTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ApplicationTypeVersion. The version of the application type.
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1, ParameterSetName = "GetServiceManifest")]
        public string ApplicationTypeVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ServiceManifestName. The name of a service manifest registered as part of an application type in a
        /// Service Fabric cluster.
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 2, ParameterSetName = "GetServiceManifest")]
        public string ServiceManifestName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ServerTimeout. The server timeout for performing the operation in seconds. This timeout specifies the
        /// time duration that the client is willing to wait for the requested operation to complete. The default value for
        /// this parameter is 60 seconds.
        /// </summary>
        [Parameter(Mandatory = false, Position = 3, ParameterSetName = "GetServiceManifest")]
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
                var result = this.ServiceFabricClient.ServiceTypes.GetServiceManifestAsync(
                    applicationTypeName: this.ApplicationTypeName,
                    applicationTypeVersion: this.ApplicationTypeVersion,
                    serviceManifestName: this.ServiceManifestName,
                    serverTimeout: this.ServerTimeout,
                    cancellationToken: this.CancellationToken).GetAwaiter().GetResult();

                this.WriteObject(this.FormatOutput(result));
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