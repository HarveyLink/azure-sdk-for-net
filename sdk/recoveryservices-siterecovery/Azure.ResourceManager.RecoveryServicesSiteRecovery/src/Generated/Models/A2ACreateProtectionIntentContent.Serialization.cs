// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.RecoveryServicesSiteRecovery.Models
{
    public partial class A2ACreateProtectionIntentContent : IUtf8JsonSerializable, IJsonModel<A2ACreateProtectionIntentContent>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<A2ACreateProtectionIntentContent>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<A2ACreateProtectionIntentContent>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected override void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<A2ACreateProtectionIntentContent>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(A2ACreateProtectionIntentContent)} does not support writing '{format}' format.");
            }

            base.JsonModelWriteCore(writer, options);
            writer.WritePropertyName("fabricObjectId"u8);
            writer.WriteStringValue(FabricObjectId);
            writer.WritePropertyName("primaryLocation"u8);
            writer.WriteStringValue(PrimaryLocation);
            writer.WritePropertyName("recoveryLocation"u8);
            writer.WriteStringValue(RecoveryLocation);
            writer.WritePropertyName("recoverySubscriptionId"u8);
            writer.WriteStringValue(RecoverySubscriptionId);
            writer.WritePropertyName("recoveryAvailabilityType"u8);
            writer.WriteStringValue(RecoveryAvailabilityType.ToString());
            if (Optional.IsDefined(ProtectionProfileCustomContent))
            {
                writer.WritePropertyName("protectionProfileCustomInput"u8);
                writer.WriteObjectValue(ProtectionProfileCustomContent, options);
            }
            writer.WritePropertyName("recoveryResourceGroupId"u8);
            writer.WriteStringValue(RecoveryResourceGroupId);
            if (Optional.IsDefined(PrimaryStagingStorageAccountCustomContent))
            {
                writer.WritePropertyName("primaryStagingStorageAccountCustomInput"u8);
                writer.WriteObjectValue(PrimaryStagingStorageAccountCustomContent, options);
            }
            if (Optional.IsDefined(RecoveryAvailabilitySetCustomContent))
            {
                writer.WritePropertyName("recoveryAvailabilitySetCustomInput"u8);
                writer.WriteObjectValue(RecoveryAvailabilitySetCustomContent, options);
            }
            if (Optional.IsDefined(RecoveryVirtualNetworkCustomContent))
            {
                writer.WritePropertyName("recoveryVirtualNetworkCustomInput"u8);
                writer.WriteObjectValue(RecoveryVirtualNetworkCustomContent, options);
            }
            if (Optional.IsDefined(RecoveryProximityPlacementGroupCustomContent))
            {
                writer.WritePropertyName("recoveryProximityPlacementGroupCustomInput"u8);
                writer.WriteObjectValue(RecoveryProximityPlacementGroupCustomContent, options);
            }
            if (Optional.IsDefined(AutoProtectionOfDataDisk))
            {
                writer.WritePropertyName("autoProtectionOfDataDisk"u8);
                writer.WriteStringValue(AutoProtectionOfDataDisk.Value.ToString());
            }
            if (Optional.IsCollectionDefined(VmDisks))
            {
                writer.WritePropertyName("vmDisks"u8);
                writer.WriteStartArray();
                foreach (var item in VmDisks)
                {
                    writer.WriteObjectValue(item, options);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsCollectionDefined(VmManagedDisks))
            {
                writer.WritePropertyName("vmManagedDisks"u8);
                writer.WriteStartArray();
                foreach (var item in VmManagedDisks)
                {
                    writer.WriteObjectValue(item, options);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(MultiVmGroupName))
            {
                writer.WritePropertyName("multiVmGroupName"u8);
                writer.WriteStringValue(MultiVmGroupName);
            }
            if (Optional.IsDefined(MultiVmGroupId))
            {
                writer.WritePropertyName("multiVmGroupId"u8);
                writer.WriteStringValue(MultiVmGroupId);
            }
            if (Optional.IsDefined(RecoveryBootDiagStorageAccount))
            {
                writer.WritePropertyName("recoveryBootDiagStorageAccount"u8);
                writer.WriteObjectValue(RecoveryBootDiagStorageAccount, options);
            }
            if (Optional.IsDefined(DiskEncryptionInfo))
            {
                writer.WritePropertyName("diskEncryptionInfo"u8);
                writer.WriteObjectValue(DiskEncryptionInfo, options);
            }
            if (Optional.IsDefined(RecoveryAvailabilityZone))
            {
                writer.WritePropertyName("recoveryAvailabilityZone"u8);
                writer.WriteStringValue(RecoveryAvailabilityZone);
            }
            if (Optional.IsDefined(AgentAutoUpdateStatus))
            {
                writer.WritePropertyName("agentAutoUpdateStatus"u8);
                writer.WriteStringValue(AgentAutoUpdateStatus.Value.ToString());
            }
            if (Optional.IsDefined(AutomationAccountAuthenticationType))
            {
                writer.WritePropertyName("automationAccountAuthenticationType"u8);
                writer.WriteStringValue(AutomationAccountAuthenticationType.Value.ToString());
            }
            if (Optional.IsDefined(AutomationAccountArmId))
            {
                writer.WritePropertyName("automationAccountArmId"u8);
                writer.WriteStringValue(AutomationAccountArmId);
            }
        }

        A2ACreateProtectionIntentContent IJsonModel<A2ACreateProtectionIntentContent>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<A2ACreateProtectionIntentContent>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(A2ACreateProtectionIntentContent)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeA2ACreateProtectionIntentContent(document.RootElement, options);
        }

        internal static A2ACreateProtectionIntentContent DeserializeA2ACreateProtectionIntentContent(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            ResourceIdentifier fabricObjectId = default;
            AzureLocation primaryLocation = default;
            AzureLocation recoveryLocation = default;
            string recoverySubscriptionId = default;
            A2ARecoveryAvailabilityType recoveryAvailabilityType = default;
            ProtectionProfileCustomDetails protectionProfileCustomContent = default;
            ResourceIdentifier recoveryResourceGroupId = default;
            StorageAccountCustomDetails primaryStagingStorageAccountCustomContent = default;
            RecoveryAvailabilitySetCustomDetails recoveryAvailabilitySetCustomContent = default;
            RecoveryVirtualNetworkCustomDetails recoveryVirtualNetworkCustomContent = default;
            RecoveryProximityPlacementGroupCustomDetails recoveryProximityPlacementGroupCustomContent = default;
            AutoProtectionOfDataDisk? autoProtectionOfDataDisk = default;
            IList<A2AProtectionIntentDiskDetails> vmDisks = default;
            IList<A2AProtectionIntentManagedDiskDetails> vmManagedDisks = default;
            string multiVmGroupName = default;
            string multiVmGroupId = default;
            StorageAccountCustomDetails recoveryBootDiagStorageAccount = default;
            SiteRecoveryDiskEncryptionInfo diskEncryptionInfo = default;
            string recoveryAvailabilityZone = default;
            SiteRecoveryAgentAutoUpdateStatus? agentAutoUpdateStatus = default;
            AutomationAccountAuthenticationType? automationAccountAuthenticationType = default;
            ResourceIdentifier automationAccountArmId = default;
            string instanceType = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> rawDataDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("fabricObjectId"u8))
                {
                    fabricObjectId = new ResourceIdentifier(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("primaryLocation"u8))
                {
                    primaryLocation = new AzureLocation(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("recoveryLocation"u8))
                {
                    recoveryLocation = new AzureLocation(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("recoverySubscriptionId"u8))
                {
                    recoverySubscriptionId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("recoveryAvailabilityType"u8))
                {
                    recoveryAvailabilityType = new A2ARecoveryAvailabilityType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("protectionProfileCustomInput"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    protectionProfileCustomContent = ProtectionProfileCustomDetails.DeserializeProtectionProfileCustomDetails(property.Value, options);
                    continue;
                }
                if (property.NameEquals("recoveryResourceGroupId"u8))
                {
                    recoveryResourceGroupId = new ResourceIdentifier(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("primaryStagingStorageAccountCustomInput"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    primaryStagingStorageAccountCustomContent = StorageAccountCustomDetails.DeserializeStorageAccountCustomDetails(property.Value, options);
                    continue;
                }
                if (property.NameEquals("recoveryAvailabilitySetCustomInput"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    recoveryAvailabilitySetCustomContent = RecoveryAvailabilitySetCustomDetails.DeserializeRecoveryAvailabilitySetCustomDetails(property.Value, options);
                    continue;
                }
                if (property.NameEquals("recoveryVirtualNetworkCustomInput"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    recoveryVirtualNetworkCustomContent = RecoveryVirtualNetworkCustomDetails.DeserializeRecoveryVirtualNetworkCustomDetails(property.Value, options);
                    continue;
                }
                if (property.NameEquals("recoveryProximityPlacementGroupCustomInput"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    recoveryProximityPlacementGroupCustomContent = RecoveryProximityPlacementGroupCustomDetails.DeserializeRecoveryProximityPlacementGroupCustomDetails(property.Value, options);
                    continue;
                }
                if (property.NameEquals("autoProtectionOfDataDisk"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    autoProtectionOfDataDisk = new AutoProtectionOfDataDisk(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("vmDisks"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<A2AProtectionIntentDiskDetails> array = new List<A2AProtectionIntentDiskDetails>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(A2AProtectionIntentDiskDetails.DeserializeA2AProtectionIntentDiskDetails(item, options));
                    }
                    vmDisks = array;
                    continue;
                }
                if (property.NameEquals("vmManagedDisks"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<A2AProtectionIntentManagedDiskDetails> array = new List<A2AProtectionIntentManagedDiskDetails>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(A2AProtectionIntentManagedDiskDetails.DeserializeA2AProtectionIntentManagedDiskDetails(item, options));
                    }
                    vmManagedDisks = array;
                    continue;
                }
                if (property.NameEquals("multiVmGroupName"u8))
                {
                    multiVmGroupName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("multiVmGroupId"u8))
                {
                    multiVmGroupId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("recoveryBootDiagStorageAccount"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    recoveryBootDiagStorageAccount = StorageAccountCustomDetails.DeserializeStorageAccountCustomDetails(property.Value, options);
                    continue;
                }
                if (property.NameEquals("diskEncryptionInfo"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    diskEncryptionInfo = SiteRecoveryDiskEncryptionInfo.DeserializeSiteRecoveryDiskEncryptionInfo(property.Value, options);
                    continue;
                }
                if (property.NameEquals("recoveryAvailabilityZone"u8))
                {
                    recoveryAvailabilityZone = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("agentAutoUpdateStatus"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    agentAutoUpdateStatus = new SiteRecoveryAgentAutoUpdateStatus(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("automationAccountAuthenticationType"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    automationAccountAuthenticationType = new AutomationAccountAuthenticationType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("automationAccountArmId"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    automationAccountArmId = new ResourceIdentifier(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("instanceType"u8))
                {
                    instanceType = property.Value.GetString();
                    continue;
                }
                if (options.Format != "W")
                {
                    rawDataDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = rawDataDictionary;
            return new A2ACreateProtectionIntentContent(
                instanceType,
                serializedAdditionalRawData,
                fabricObjectId,
                primaryLocation,
                recoveryLocation,
                recoverySubscriptionId,
                recoveryAvailabilityType,
                protectionProfileCustomContent,
                recoveryResourceGroupId,
                primaryStagingStorageAccountCustomContent,
                recoveryAvailabilitySetCustomContent,
                recoveryVirtualNetworkCustomContent,
                recoveryProximityPlacementGroupCustomContent,
                autoProtectionOfDataDisk,
                vmDisks ?? new ChangeTrackingList<A2AProtectionIntentDiskDetails>(),
                vmManagedDisks ?? new ChangeTrackingList<A2AProtectionIntentManagedDiskDetails>(),
                multiVmGroupName,
                multiVmGroupId,
                recoveryBootDiagStorageAccount,
                diskEncryptionInfo,
                recoveryAvailabilityZone,
                agentAutoUpdateStatus,
                automationAccountAuthenticationType,
                automationAccountArmId);
        }

        BinaryData IPersistableModel<A2ACreateProtectionIntentContent>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<A2ACreateProtectionIntentContent>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options);
                default:
                    throw new FormatException($"The model {nameof(A2ACreateProtectionIntentContent)} does not support writing '{options.Format}' format.");
            }
        }

        A2ACreateProtectionIntentContent IPersistableModel<A2ACreateProtectionIntentContent>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<A2ACreateProtectionIntentContent>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeA2ACreateProtectionIntentContent(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(A2ACreateProtectionIntentContent)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<A2ACreateProtectionIntentContent>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
