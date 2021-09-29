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
    public class LabelingJobResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-LabelingJobResourceOperations";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private const string DataContainerNamePrefix = "test-datacontainer";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceName = ResourceNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _dataContainerName = DataContainerNamePrefix;

        public LabelingJobResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _dataContainerName = SessionRecording.GenerateAssetName(DataContainerNamePrefix);
            // Create RG and Res with GlobalClient
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            DataContainerResource dataContainer = await (await ws.GetDataContainerResources().CreateOrUpdateAsync(
                _dataContainerName,
                DataHelper.GenerateDataContainerResourceData())).WaitForCompletionAsync();
            DataVersionResource data = await (await dataContainer.GetDataVersionResources().CreateOrUpdateAsync("1", DataHelper.GenerateDataVersionResourceData())).WaitForCompletionAsync();
            _ = await ws.GetLabelingJobResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateLabelingJobResourceData(dataContainer,data));
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            DataContainerResource dataContainer = await ws.GetDataContainerResources().GetAsync(_dataContainerName);
            DataVersionResource data = await dataContainer.GetDataVersionResources().GetAsync("1");
            var deleteResourceName = Recording.GenerateAssetName(ResourceNamePrefix) + "_delete";
            LabelingJobCreateOrUpdateOperation res = null;
            Assert.DoesNotThrowAsync(async () => res = await ws.GetLabelingJobResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateLabelingJobResourceData(dataContainer, data)));
            Assert.DoesNotThrowAsync(async () => _ = await res.Value.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            LabelingJobResource resource = await ws.GetLabelingJobResources().GetAsync(_resourceName);
            LabelingJobResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }
    }
}
