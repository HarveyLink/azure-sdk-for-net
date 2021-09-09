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
    public class ModelVersionResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-ModelVersionResourceOperations";
        private const string WorkspacePrefix = "test-workspace";
        private const string ParentPrefix = "test-parent";
        private const string ResourceNamePrefix = "test-resource";
        private const string DataStoreNamePrefix = "test_dataStore";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceName = ResourceNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _parentPrefix = ParentPrefix;
        private string _dataStoreName = DataStoreNamePrefix;

        public ModelVersionResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _parentPrefix = SessionRecording.GenerateAssetName(ParentPrefix);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _dataStoreName = SessionRecording.GenerateAssetName(DataStoreNamePrefix);

            // Create RG and Res with GlobalClient
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups().CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(_workspaceName, DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();

            ModelContainerResource parent = await (await ws.GetModelContainerResources().CreateOrUpdateAsync(_parentPrefix, DataHelper.GenerateModelContainerResourceData())).WaitForCompletionAsync();

            DatastorePropertiesResource dateStore = await (await ws.GetDatastorePropertiesResources().CreateOrUpdateAsync(_dataStoreName, DataHelper.GenerateDatastorePropertiesResourceData())).WaitForCompletionAsync();

            _ = await parent.GetModelVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateModelVersionResourceData(dateStore));
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ModelContainerResource parent = await ws.GetModelContainerResources().GetAsync(_parentPrefix);
            DatastorePropertiesResource dateStore = await ws.GetDatastorePropertiesResources().GetAsync(_dataStoreName);

            var deleteResourceName = Recording.GenerateAssetName(ResourceNamePrefix) + "_delete";
            ModelVersionCreateOrUpdateOperation res = null;
            Assert.DoesNotThrowAsync(async () => res = await parent.GetModelVersionResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateModelVersionResourceData(dateStore)));
            Assert.DoesNotThrowAsync(async () => _ = await res.Value.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ModelContainerResource parent = await ws.GetModelContainerResources().GetAsync(_parentPrefix);

            ModelVersionResource resource = await parent.GetModelVersionResources().GetAsync(_resourceName);
            ModelVersionResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }
    }
}
