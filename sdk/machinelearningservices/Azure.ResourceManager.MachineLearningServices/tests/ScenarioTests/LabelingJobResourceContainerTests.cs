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
    public class LabelingJobResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-LabelingJobResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private const string DataContainerNamePrefix = "test-datacontainer";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _dataContainerName = DataContainerNamePrefix;
        private int _resourceCreated = 0;
        public LabelingJobResourceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _dataContainerName = SessionRecording.GenerateAssetName(DataContainerNamePrefix);
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();
            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            DataContainerResource dcr = await (await ws.GetDataContainerResources().CreateOrUpdateAsync(
                _dataContainerName,
                DataHelper.GenerateDataContainerResourceData())).WaitForCompletionAsync();
            _ = await (await dcr.GetDataVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateDataVersionResourceData())).WaitForCompletionAsync();
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            DataContainerResource dataContainer = await ws.GetDataContainerResources().GetAsync(_dataContainerName);
            DataVersionResource data = await dataContainer.GetDataVersionResources().GetAsync("1");
            Assert.DoesNotThrowAsync(async () => _ = await ws.GetLabelingJobResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateLabelingJobResourceData(dataContainer,data)));
            _resourceCreated++;
            var count = (await ws.GetLabelingJobResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(1, _resourceCreated);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            DataContainerResource dataContainer = await ws.GetDataContainerResources().GetAsync(_dataContainerName);
            DataVersionResource data = await dataContainer.GetDataVersionResources().GetAsync("1");
            Assert.DoesNotThrowAsync(async () => _ = await ws.GetLabelingJobResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateLabelingJobResourceData(dataContainer, data)));

            Assert.DoesNotThrowAsync(async () => await ws.GetLabelingJobResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await ws.GetLabelingJobResources().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            DataContainerResource dataContainer = await ws.GetDataContainerResources().GetAsync(_dataContainerName);
            DataVersionResource data = await dataContainer.GetDataVersionResources().GetAsync("1");
            LabelingJobCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetLabelingJobResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateLabelingJobResourceData(dataContainer, data)));

            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetLabelingJobResources().CreateOrUpdateAsync(
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
            DataContainerResource dataContainer = await ws.GetDataContainerResources().GetAsync(_dataContainerName);
            DataVersionResource data = await dataContainer.GetDataVersionResources().GetAsync("1");
            Assert.DoesNotThrowAsync(async () => _ = await (await ws.GetLabelingJobResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateLabelingJobResourceData(dataContainer, data))).WaitForCompletionAsync());

            Assert.IsTrue(await ws.GetLabelingJobResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await ws.GetLabelingJobResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
