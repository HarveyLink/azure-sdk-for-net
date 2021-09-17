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
    public class DataVersionResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-DataVersionResourceOperations";
        private const string WorkspacePrefix = "test-workspace";
        private const string ParentPrefix = "test-parent";
        private const string ResourceNamePrefix = "1";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceName = ResourceNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _parentPrefix = ParentPrefix;

        public DataVersionResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _parentPrefix = SessionRecording.GenerateAssetName(ParentPrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);

            // Create RG and Res with GlobalClient
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(_workspaceName, DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();

            DataContainerResource parent = await (await ws.GetDataContainerResources().CreateOrUpdateAsync(_parentPrefix, DataHelper.GenerateDataContainerResourceData())).WaitForCompletionAsync();

            _ = await parent.GetDataVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateDataVersionResourceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            DataContainerResource parent = await ws.GetDataContainerResources().GetAsync(_parentPrefix);

            var deleteResourceName = "2";
            DataVersionCreateOrUpdateOperation res = null;
            Assert.DoesNotThrowAsync(async () => res = await parent.GetDataVersionResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateDataVersionResourceData()));
            Assert.DoesNotThrowAsync(async () => _ = await res.Value.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            DataContainerResource parent = await ws.GetDataContainerResources().GetAsync(_parentPrefix);

            DataVersionResource resource = await parent.GetDataVersionResources().GetAsync(_resourceName);
            DataVersionResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }
    }
}
