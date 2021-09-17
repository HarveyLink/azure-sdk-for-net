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
    public class BatchDeploymentTrackedResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-BatchDeploymentTrackedResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string BatchEndpointPrefix = "test-endpoint";
        private const string ResourceNamePrefix = "test-depolyment";
        private const string ComputeNamePrefix = "test-compute";
        private const string ModelContainerNamePrefix = "test-model";
        private const string CodeContainerNamePrefix = "test-code";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _batchEndpointName = BatchEndpointPrefix;
        private string _computeName = ComputeNamePrefix;
        private string _modelContainerName = ModelContainerNamePrefix;
        private string _codeContainerName = CodeContainerNamePrefix;
        private string _environmentContainerName = "AzureML-sklearn-0.24-ubuntu18.04-py37-cpu";
        private string _environmentVerion = "7";

        public BatchDeploymentTrackedResourceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _batchEndpointName = SessionRecording.GenerateAssetName(BatchEndpointPrefix);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _computeName = SessionRecording.GenerateAssetName(ComputeNamePrefix);
            _modelContainerName = SessionRecording.GenerateAssetName(ModelContainerNamePrefix);
            _codeContainerName = SessionRecording.GenerateAssetName(CodeContainerNamePrefix);

            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();
            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            _ = await ws.GetBatchEndpointTrackedResources().CreateOrUpdateAsync(
                _batchEndpointName,
                DataHelper.GenerateBatchEndpointTrackedResourceData());
            //Code
            CodeContainerResource ccr = await (await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _codeContainerName,
                DataHelper.GenerateCodeContainerResourceData())).WaitForCompletionAsync();
            //TODO: Upload code to datacontainer()
            _ = ccr.GetCodeVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateCodeVersion());
            //Model
            DatastorePropertiesResource datastore = await ws.GetDatastorePropertiesResources().GetAsync("azureml");
            ModelContainerResource mcr = await (await ws.GetModelContainerResources().CreateOrUpdateAsync(
                _modelContainerName,
                DataHelper.GenerateModelContainerResourceData())).WaitForCompletionAsync();
            _ = await (await mcr.GetModelVersionResources().CreateOrUpdateAsync(
                "1",
                DataHelper.GenerateModelVersionResourceData(datastore))).WaitForCompletionAsync();
            //Compute
            _ = await (await ws.GetComputeResources().CreateOrUpdateAsync(
                _computeName,
                DataHelper.GenerateComputeResourceData())).WaitForCompletionAsync();

            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            BatchEndpointTrackedResource endpoint = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);
            //Code
            CodeContainerResource ccr = await ws.GetCodeContainerResources().GetAsync(_codeContainerName);
            CodeVersionResource code = await ccr.GetCodeVersionResources().GetAsync("1");
            //Model
            ModelContainerResource mcr = await ws.GetModelContainerResources().GetAsync(_modelContainerName);
            ModelVersionResource model = await mcr.GetModelVersionResources().GetAsync("1");
            //Environment
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            //Compute
            ComputeResource compute = await ws.GetComputeResources().GetAsync(_computeName);
            Assert.DoesNotThrowAsync(async () => _ = await endpoint.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateBatchDeploymentTrackedResourceData("BatchEndpoint/score.py", code, model, environment, compute)));
            var count = (await endpoint.GetBatchDeploymentTrackedResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            BatchEndpointTrackedResource parent = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);
            //Code
            CodeContainerResource ccr = await ws.GetCodeContainerResources().GetAsync(_codeContainerName);
            CodeVersionResource code = await ccr.GetCodeVersionResources().GetAsync("1");
            //Model
            ModelContainerResource mcr = await ws.GetModelContainerResources().GetAsync(_modelContainerName);
            ModelVersionResource model = await mcr.GetModelVersionResources().GetAsync("1");
            //Environment
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            //Compute
            ComputeResource compute = await ws.GetComputeResources().GetAsync(_computeName);
            Assert.DoesNotThrowAsync(async () => _ = await parent.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateBatchDeploymentTrackedResourceData("BatchEndpoint/score.py", code, model, environment, compute)));

            Assert.DoesNotThrowAsync(async () => await parent.GetBatchDeploymentTrackedResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await parent.GetBatchDeploymentTrackedResources().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            BatchEndpointTrackedResource parent = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);
            //Code
            CodeContainerResource ccr = await ws.GetCodeContainerResources().GetAsync(_codeContainerName);
            CodeVersionResource code = await ccr.GetCodeVersionResources().GetAsync("1");
            //Model
            ModelContainerResource mcr = await ws.GetModelContainerResources().GetAsync(_modelContainerName);
            ModelVersionResource model = await mcr.GetModelVersionResources().GetAsync("1");
            //Environment
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            //Compute
            ComputeResource compute = await ws.GetComputeResources().GetAsync(_computeName);
            BatchDeploymentCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateBatchDeploymentTrackedResourceData("BatchEndpoint/score.py", code, model, environment, compute)));

            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
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
            BatchEndpointTrackedResource endpoint = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);
            //Code
            CodeContainerResource ccr = await ws.GetCodeContainerResources().GetAsync(_codeContainerName);
            CodeVersionResource code = await ccr.GetCodeVersionResources().GetAsync("1");
            //Model
            ModelContainerResource mcr = await ws.GetModelContainerResources().GetAsync(_modelContainerName);
            ModelVersionResource model = await mcr.GetModelVersionResources().GetAsync("1");
            //Environment
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            //Compute
            ComputeResource compute = await ws.GetComputeResources().GetAsync(_computeName);
            Assert.DoesNotThrowAsync(async () => _ = await (await endpoint.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateBatchDeploymentTrackedResourceData("BatchEndpoint/score.py", code, model, environment, compute))).WaitForCompletionAsync());

            Assert.IsTrue(await endpoint.GetBatchDeploymentTrackedResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await endpoint.GetBatchDeploymentTrackedResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
