// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.MachineLearningServices.Tests.Extensions;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests.ScenarioTests
{
    public class JobBaseResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-JobBaseResourceOperations";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceName = ResourceNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceGroupName = ResourceGroupNamePrefix;

        public JobBaseResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);

            // Create RG and Res with GlobalClient
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(_workspaceName, DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();

            _ = await ws.GetJobBaseResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateJobBaseResourceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            var deleteResourceName = Recording.GenerateAssetName(ResourceNamePrefix) + "_delete";
            JobCreateOrUpdateOperation res = null;
            Assert.DoesNotThrowAsync(async () => res = await ws.GetJobBaseResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateJobBaseResourceData()));
            Assert.DoesNotThrowAsync(async () => _ = await res.Value.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            JobBaseResource resource = await ws.GetJobBaseResources().GetAsync(_resourceName);
            JobBaseResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }
    }
}
