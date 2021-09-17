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
    public class OnlineDeploymentTrackedResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-OnlineDeploymentTrackedResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string EndpointNamePrefix = "test-parent";
        private const string ResourceNamePrefix = "test-resource";
        private const string ComputeNamePrefix = "test-compute";
        private const string ModelContainerNamePrefix = "test-model";
        private const string CodeContainerNamePrefix = "test-code";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _endpointName = EndpointNamePrefix;
        private string _computeName = ComputeNamePrefix;
        private string _modelContainerName = ModelContainerNamePrefix;
        private string _codeContainerName = CodeContainerNamePrefix;
        private string _environmentContainerName = "AzureML-sklearn-0.24.1-ubuntu18.04-py37-cpu-inference";
        private string _environmentVersion = "12";

        public OnlineDeploymentTrackedResourceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _endpointName = SessionRecording.GenerateAssetName(EndpointNamePrefix);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _computeName = SessionRecording.GenerateAssetName(ComputeNamePrefix);
            _modelContainerName = SessionRecording.GenerateAssetName(ModelContainerNamePrefix);
            _codeContainerName = SessionRecording.GenerateAssetName(CodeContainerNamePrefix);
            ResourceGroup rg = (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).Value;
            Workspace ws = (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).Value;
            //compute
            ComputeResource compute = (await ws.GetComputeResources().CreateOrUpdateAsync(
                _computeName,
                DataHelper.GenerateComputeResourceData())).Value;
            //model
            DatastorePropertiesResource datastore = await ws.GetDatastorePropertiesResources().GetAsync("azureml");
            ModelContainerResource mcr = await (await ws.GetModelContainerResources().CreateOrUpdateAsync(
                _modelContainerName,
                DataHelper.GenerateModelContainerResourceData())).WaitForCompletionAsync();
            _ = mcr.GetModelVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateModelVersionResourceData(datastore));
            //endpoint
            _ = await ws.GetOnlineEndpointTrackedResources().CreateOrUpdateAsync(
                _endpointName,
                DataHelper.GenerateOnlineEndpointTrackedResourceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
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
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            Assert.DoesNotThrowAsync(async () => _ = await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateOnlineDeploymentTrackedResourceData(_endpointName,code,model,environment)));

            var count = (await parent.GetOnlineDeploymentTrackedResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
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
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            Assert.DoesNotThrowAsync(async () => _ = await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateOnlineDeploymentTrackedResourceData(_endpointName, code, model, environment)));

            Assert.DoesNotThrowAsync(async () => await parent.GetOnlineDeploymentTrackedResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await parent.GetOnlineDeploymentTrackedResources().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
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
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            OnlineDeploymentCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateOnlineDeploymentTrackedResourceData(_endpointName, code, model, environment)));

            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                resource.Value.Data));
            Assert.AreEqual("Updated", resource.Value.Data.Properties.Description);
        }

        [TestCase]
        [RecordedTest]
        public async Task CheckIfExists()
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
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            Assert.DoesNotThrowAsync(async () => _ = await (await parent.GetOnlineDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
               DataHelper.GenerateOnlineDeploymentTrackedResourceData(_endpointName, code, model, environment))).WaitForCompletionAsync());

            Assert.IsTrue(await parent.GetOnlineDeploymentTrackedResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await parent.GetOnlineDeploymentTrackedResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
