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
    public class OnlineDeploymentTrackedResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-OnlineDeploymentTrackedResourceOperations";
        private const string WorkspacePrefix = "test-workspace";
        private const string EndpointNamePrefix = "test-endpoint";
        private const string ResourceNamePrefix = "test-resource";
        private const string ComputeNamePrefix = "test-compute";
        private const string CodeContainerNamePrefix = "test-code";
        private const string ModelContainerNamePrefix = "test-model";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceName = ResourceNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _endpointName = EndpointNamePrefix;
        private string _computeName = ComputeNamePrefix;
        private string _codeContainerName = CodeContainerNamePrefix;
        private string _modelContainerName = ModelContainerNamePrefix;
        private string _environmentContainerName = "AzureML-sklearn-0.24-ubuntu18.04-py37-cpu";
        private string _environmentVerion = "7";
        public OnlineDeploymentTrackedResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _endpointName = SessionRecording.GenerateAssetName(EndpointNamePrefix);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _computeName = SessionRecording.GenerateAssetName(ComputeNamePrefix);

            // Create RG and Res with GlobalClient
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            //Compute
            ComputeResource compute = await (await ws.GetComputeResources().CreateOrUpdateAsync(
                _computeName,
                DataHelper.GenerateComputeResourceData())).WaitForCompletionAsync();
            //Endpoint
            OnlineEndpointTrackedResource parent = await (await ws.GetOnlineEndpointTrackedResources().CreateOrUpdateAsync(
                _endpointName,
                DataHelper.GenerateOnlineEndpointTrackedResourceData())).WaitForCompletionAsync();
            //Code
            CodeContainerResource ccr = await (await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _codeContainerName,
                DataHelper.GenerateCodeContainerResourceData())).WaitForCompletionAsync();
            CodeVersionResource code = await (await ccr.GetCodeVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateCodeVersion())).WaitForCompletionAsync();
            //Model
            DatastorePropertiesResource datastore = await ws.GetDatastorePropertiesResources().GetAsync("azureml");
            ModelContainerResource mcr = await (await ws.GetModelContainerResources().CreateOrUpdateAsync(
                _modelContainerName,
                DataHelper.GenerateModelContainerResourceData())).WaitForCompletionAsync();
            ModelVersionResource model = await (await mcr.GetModelVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateModelVersionResourceData(datastore))).WaitForCompletionAsync();
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            _ = await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateOnlineDeploymentTrackedResourceData("",code,model,environment));
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            OnlineEndpointTrackedResource parent = await ws.GetOnlineEndpointTrackedResources().GetAsync(_endpointName);
            //Code
            CodeContainerResource ccr = await ws.GetCodeContainerResources().GetAsync(_codeContainerName);
            CodeVersionResource code = await ccr.GetCodeVersionResources().GetAsync("1");
            //Model
            ModelContainerResource mcr = await ws.GetModelContainerResources().GetAsync(_modelContainerName);
            ModelVersionResource model = await mcr.GetModelVersionResources().GetAsync("1");
            //Environment
            EnvironmentContainerResource ecr =
                await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment =
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync("1");
            string deleteResourceName = Recording.GenerateAssetName(ResourceNamePrefix) + "_delete";
            OnlineDeploymentCreateOrUpdateOperation res = null;
            Assert.DoesNotThrowAsync(async () => res = await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateOnlineDeploymentTrackedResourceData("OnlineEndpoint/score.py",code,model,environment)));
            Assert.DoesNotThrowAsync(async () => _ = await res.Value.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            OnlineEndpointTrackedResource parent = await ws.GetOnlineEndpointTrackedResources().GetAsync(_endpointName);

            OnlineDeploymentTrackedResource resource = await parent.GetOnlineDeploymentTrackedResources().GetAsync(_resourceName);
            OnlineDeploymentTrackedResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Update()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            OnlineEndpointTrackedResource parent = await ws.GetOnlineEndpointTrackedResources().GetAsync(_endpointName);
            OnlineDeploymentTrackedResource resource = await parent.GetOnlineDeploymentTrackedResources().GetAsync(_resourceName);
            var update = new PartialOnlineDeploymentPartialTrackedResource();
            update.Tags.Add("tag1", "value1");

            OnlineDeploymentTrackedResource updatedResource = await (await resource.UpdateAsync(update)).WaitForCompletionAsync();
            Assert.AreEqual("value1", updatedResource.Data.Tags["tag1"]);
        }
    }
}
