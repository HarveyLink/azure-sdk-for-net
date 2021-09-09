﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Azure.Core.TestFramework;
using Azure.ResourceManager.Core;
using Azure.ResourceManager.MachineLearningServices.Tests.Extensions;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using Azure.ResourceManager.TestFramework;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests
{
    public class MachineLearningServicesManagerTestBase : ManagementRecordedTestBase<MachineLearningServicesTestEnvironment>
    {
        private const string CommonResourceResourceGroup = "test-ml-common";

        public ResourceIdentifier CommonResourceGroupId { get; private set; }

        public string CommonStorageId { get; private set; }

        public string CommonAppInsightId { get; private set; }

        public string CommonKeyVaultId { get; private set; }

        public string CommonAcrId { get; private set; }

        protected ArmClient Client { get; private set; }

        protected ResourceDataCreationHelper DataHelper => new ResourceDataCreationHelper(this);

        protected Location DefaultLocation => Location.WestUS2;

        protected MachineLearningServicesManagerTestBase(bool isAsync, RecordedTestMode mode)
        : base(isAsync, mode)
        {
        }

        protected MachineLearningServicesManagerTestBase(bool isAsync)
            : base(isAsync)
        {
        }

        public void CreateDependency()
        {
            // NOTE: For initial setup, add [Test] for this method and run it once.
            CommonResourceGroupId = GlobalClient.DefaultSubscription
                .GetResourceGroups()
                .CreateOrUpdate(CommonResourceResourceGroup, new ResourceGroupData(Location.WestUS2))
                .Value.Id;

            CreateAppInsight();
            CreateAcr();
            CreateKeyVault();
            CreateStorage();
            StopSessionRecording();
        }

        [OneTimeSetUp]
        public void SetupDependencyIds()
        {
            CommonResourceGroupId = GlobalClient.DefaultSubscription.Id + $"/resourceGroups/{CommonResourceResourceGroup}";
            CommonStorageId = CommonResourceGroupId.AppendProviderResource(
                "Microsoft.Storage",
                "storageAccounts",
                "track2mlstorage");
            CommonKeyVaultId = CommonResourceGroupId.AppendProviderResource(
                "Microsoft.KeyVault",
                "vaults",
                "track2mltestkeyvault");
            CommonAcrId = CommonResourceGroupId.AppendProviderResource(
                "Microsoft.ContainerRegistry",
                "registries",
                "track2mlacr");
            CommonAppInsightId = CommonResourceGroupId.AppendProviderResource(
                "microsoft.insights",
                "components",
                "track2mlappinsight");
        }

        [SetUp]
        public void CreateCommonClient()
        {
            Client = GetArmClient();
        }

        #region Dependency Resource Creation with GlobalClient
        protected void CreateStorage()
        {
            var id = CommonResourceGroupId.AppendProviderResource(
                "Microsoft.Storage",
                "storageAccounts",
                "track2mlstorage");
            var res = new GenericResourceData(Location.WestUS2)
            {
                Kind = "StorageV2",
                Properties = new Dictionary<string, object>
                {
                    { "minimumTlsVersion", "TLS1_2" },
                    { "allowBlobPublicAccess", true },
                    { "allowSharedKeyAccess", true }
                },
                Sku = new Sku("Standard_LRS") { Tier = SkuTier.Standard }
            };

            _ = GlobalClient.DefaultSubscription.GetGenericResources().CreateOrUpdateAsync(id, res)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            CommonStorageId = id;
        }

        protected void CreateAppInsight()
        {
            var id = CommonResourceGroupId.AppendProviderResource(
                "microsoft.insights",
                "components",
                "track2mlappinsight");
            var res = new GenericResourceData(Location.WestUS2)
            {
                Kind = "web",
                Properties = new Dictionary<string, object>
                {
                    { "Ver", "V2" },
                }
            };

            _ = GlobalClient.DefaultSubscription.GetGenericResources().CreateOrUpdateAsync(id, res)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            CommonAppInsightId = id;
        }

        protected void CreateKeyVault()
        {
            var id = CommonResourceGroupId.AppendProviderResource(
                "Microsoft.KeyVault",
                "vaults",
                "track2mltestkeyvault");
            var res = new GenericResourceData(Location.WestUS2)
            {
                Properties = new Dictionary<string, object>
                {
                    { "sku", new Dictionary<string, object> { { "Name", "Standard" }, { "Family", "A" } } },
                    { "tenantId", SessionEnvironment.TenantId },
                    { "enableSoftDelete", false},
                    { "accessPolicies", new[]
                        {
                            new Dictionary<string, object>
                            {
                                { "tenantId", SessionEnvironment.TenantId },
                                { "objectId", SessionEnvironment.ClientId },
                                {
                                    "permissions", new Dictionary<string, object>
                                    {
                                        { "keys", new[] { "all" }},
                                        { "secrets", new[] { "all" }},
                                        { "certificates", new[] { "all" }},
                                        { "storage", new[] { "all" }},
                                    }
                                }
                            }
                        }
                    }
                }};

            _ = GlobalClient.DefaultSubscription.GetGenericResources().CreateOrUpdateAsync(id, res)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            CommonKeyVaultId = id;
        }

        protected void CreateAcr()
        {
            var id = CommonResourceGroupId.AppendProviderResource(
                "Microsoft.ContainerRegistry",
                "registries",
                "track2mlacr");
            var res = new GenericResourceData(Location.WestUS2)
            {
                Properties = new Dictionary<string, object>(),
                Sku = new Sku("basic") { Tier = SkuTier.Basic }
            };

            _ = GlobalClient.DefaultSubscription.GetGenericResources().CreateOrUpdateAsync(id, res)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            CommonAcrId = id;
        }
        #endregion
    }
}
