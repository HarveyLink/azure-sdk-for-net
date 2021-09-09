// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests.ScenarioTests
{
    public class ModelVersionResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-ModelVersionResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ParentPrefix = "test-parent";
        private const string ResourceNamePrefix = "test-resource";
        private const string DataStoreNamePrefix = "test_dataStore";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _parentPrefix = ParentPrefix;
        private string _dataStoreName = DataStoreNamePrefix;

        public ModelVersionResourceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _parentPrefix = SessionRecording.GenerateAssetName(ParentPrefix);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _dataStoreName = SessionRecording.GenerateAssetName(DataStoreNamePrefix);

            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();

            _ = await ws.GetModelContainerResources().CreateOrUpdateAsync(
                _parentPrefix,
                DataHelper.GenerateModelContainerResourceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ModelContainerResource parent = await ws.GetModelContainerResources().GetAsync(_parentPrefix);
            DatastoreCreateOrUpdateOperation dateStore = await ws.GetDatastorePropertiesResources().CreateOrUpdateAsync(
                _dataStoreName,
                DataHelper.GenerateDatastorePropertiesResourceData());

            Assert.DoesNotThrowAsync(async () => _ = await parent.GetModelVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateModelVersionResourceData(dateStore.Value)));

            var count = (await parent.GetModelVersionResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ModelContainerResource parent = await ws.GetModelContainerResources().GetAsync(_parentPrefix);
            DatastoreCreateOrUpdateOperation dateStore = await ws.GetDatastorePropertiesResources().CreateOrUpdateAsync(
                _dataStoreName,
                DataHelper.GenerateDatastorePropertiesResourceData());

            Assert.DoesNotThrowAsync(async () => _ = await parent.GetModelVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateModelVersionResourceData(dateStore.Value)));

            Assert.DoesNotThrowAsync(async () => await parent.GetModelVersionResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await parent.GetModelVersionResources().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ModelContainerResource parent = await ws.GetModelContainerResources().GetAsync(_parentPrefix);
            DatastoreCreateOrUpdateOperation dateStore = await ws.GetDatastorePropertiesResources().CreateOrUpdateAsync(
                _dataStoreName,
                DataHelper.GenerateDatastorePropertiesResourceData());

            ModelVersionCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetModelVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateModelVersionResourceData(dateStore.Value)));

            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetModelVersionResources().CreateOrUpdateAsync(
                _resourceName,
                resource.Value.Data.Properties));
            Assert.AreEqual("Updated", resource.Value.Data.Properties.Description);
        }

        [TestCase]
        [RecordedTest]
        public async Task CheckIfExists()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ModelContainerResource parent = await ws.GetModelContainerResources().GetAsync(_parentPrefix);
            DatastoreCreateOrUpdateOperation dateStore = await ws.GetDatastorePropertiesResources().CreateOrUpdateAsync(
                _dataStoreName,
                DataHelper.GenerateDatastorePropertiesResourceData());

            Assert.DoesNotThrowAsync(async () => _ = await (await parent.GetModelVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateModelVersionResourceData(dateStore.Value))).WaitForCompletionAsync());

            Assert.IsTrue(await parent.GetModelVersionResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await parent.GetModelVersionResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
