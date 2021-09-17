// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests.ScenarioTests
{
    public class MLSubscriptionExtensionsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-MLSubscriptionExtensions";
        private const string WorkspacePrefix = "test-workspace";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;

        public MLSubscriptionExtensionsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);

            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            _ = await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData());
            StopSessionRecording();
        }
        [TestCase]
        [RecordedTest]
        public async Task GetQuotas()
        {
            var quotas = await Client.DefaultSubscription.GetQuotasAsync(DefaultLocation).ToEnumerableAsync();
            Assert.Greater(quotas.Count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task UpdateQuotas()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            string id = ws.Id + "/quotas/standardNCFamily";
            Assert.DoesNotThrowAsync(async () => await Client.DefaultSubscription.UpdateQuotasAsync(
                DefaultLocation,
                new List<QuotaBaseProperties>
                {
                    new QuotaBaseProperties
                    {
                        Id = id,
                        Limit = 67,
                        Type = "Microsoft.MachineLearningServices/workspaces/dedicatedCores/quotas",
                        Unit = new QuotaUnit("Count")
                    }
                }));
            List<ResourceQuota> quotaBaseProperties = null;
            Assert.DoesNotThrowAsync(async () => quotaBaseProperties = await Client.DefaultSubscription.GetQuotasAsync(DefaultLocation).ToEnumerableAsync());
            var query = from a in quotaBaseProperties
                        where a.Id == id
                        where a.Type == "Microsoft.MachineLearningServices/workspaces/dedicatedCores/quotas"
                        where a.Limit == 67
                        select a;
            var result = query.ToList();
            Assert.AreEqual(result.Count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task GetMachineLearningSkus()
        {
            var skus = await Client.DefaultSubscription.GetWorkspaceSkusAsync().ToEnumerableAsync();
            Assert.Greater(skus.Count, 1);
        }
    }
}
