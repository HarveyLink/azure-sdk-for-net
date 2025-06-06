// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.Network.Models
{
    public partial class LoadBalancingRuleProperties : IUtf8JsonSerializable, IJsonModel<LoadBalancingRuleProperties>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<LoadBalancingRuleProperties>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<LoadBalancingRuleProperties>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected virtual void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LoadBalancingRuleProperties>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(LoadBalancingRuleProperties)} does not support writing '{format}' format.");
            }

            if (Optional.IsDefined(FrontendIPConfiguration))
            {
                writer.WritePropertyName("frontendIPConfiguration"u8);
                JsonSerializer.Serialize(writer, FrontendIPConfiguration);
            }
            if (Optional.IsDefined(BackendAddressPool))
            {
                writer.WritePropertyName("backendAddressPool"u8);
                JsonSerializer.Serialize(writer, BackendAddressPool);
            }
            if (Optional.IsCollectionDefined(BackendAddressPools))
            {
                writer.WritePropertyName("backendAddressPools"u8);
                writer.WriteStartArray();
                foreach (var item in BackendAddressPools)
                {
                    JsonSerializer.Serialize(writer, item);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(Probe))
            {
                writer.WritePropertyName("probe"u8);
                JsonSerializer.Serialize(writer, Probe);
            }
            writer.WritePropertyName("protocol"u8);
            writer.WriteStringValue(Protocol.ToString());
            if (Optional.IsDefined(LoadDistribution))
            {
                writer.WritePropertyName("loadDistribution"u8);
                writer.WriteStringValue(LoadDistribution.Value.ToString());
            }
            writer.WritePropertyName("frontendPort"u8);
            writer.WriteNumberValue(FrontendPort);
            if (Optional.IsDefined(BackendPort))
            {
                writer.WritePropertyName("backendPort"u8);
                writer.WriteNumberValue(BackendPort.Value);
            }
            if (Optional.IsDefined(IdleTimeoutInMinutes))
            {
                writer.WritePropertyName("idleTimeoutInMinutes"u8);
                writer.WriteNumberValue(IdleTimeoutInMinutes.Value);
            }
            if (Optional.IsDefined(EnableFloatingIP))
            {
                writer.WritePropertyName("enableFloatingIP"u8);
                writer.WriteBooleanValue(EnableFloatingIP.Value);
            }
            if (Optional.IsDefined(EnableTcpReset))
            {
                writer.WritePropertyName("enableTcpReset"u8);
                writer.WriteBooleanValue(EnableTcpReset.Value);
            }
            if (Optional.IsDefined(DisableOutboundSnat))
            {
                writer.WritePropertyName("disableOutboundSnat"u8);
                writer.WriteBooleanValue(DisableOutboundSnat.Value);
            }
            if (options.Format != "W" && Optional.IsDefined(ProvisioningState))
            {
                writer.WritePropertyName("provisioningState"u8);
                writer.WriteStringValue(ProvisioningState.Value.ToString());
            }
            foreach (var item in AdditionalProperties)
            {
                writer.WritePropertyName(item.Key);
#if NET6_0_OR_GREATER
				writer.WriteRawValue(item.Value);
#else
                using (JsonDocument document = JsonDocument.Parse(item.Value, ModelSerializationExtensions.JsonDocumentOptions))
                {
                    JsonSerializer.Serialize(writer, document.RootElement);
                }
#endif
            }
        }

        LoadBalancingRuleProperties IJsonModel<LoadBalancingRuleProperties>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LoadBalancingRuleProperties>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(LoadBalancingRuleProperties)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeLoadBalancingRuleProperties(document.RootElement, options);
        }

        internal static LoadBalancingRuleProperties DeserializeLoadBalancingRuleProperties(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            WritableSubResource frontendIPConfiguration = default;
            WritableSubResource backendAddressPool = default;
            IList<WritableSubResource> backendAddressPools = default;
            WritableSubResource probe = default;
            LoadBalancingTransportProtocol protocol = default;
            LoadDistribution? loadDistribution = default;
            int frontendPort = default;
            int? backendPort = default;
            int? idleTimeoutInMinutes = default;
            bool? enableFloatingIP = default;
            bool? enableTcpReset = default;
            bool? disableOutboundSnat = default;
            NetworkProvisioningState? provisioningState = default;
            IDictionary<string, BinaryData> additionalProperties = default;
            Dictionary<string, BinaryData> additionalPropertiesDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("frontendIPConfiguration"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    frontendIPConfiguration = JsonSerializer.Deserialize<WritableSubResource>(property.Value.GetRawText());
                    continue;
                }
                if (property.NameEquals("backendAddressPool"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    backendAddressPool = JsonSerializer.Deserialize<WritableSubResource>(property.Value.GetRawText());
                    continue;
                }
                if (property.NameEquals("backendAddressPools"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<WritableSubResource> array = new List<WritableSubResource>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(JsonSerializer.Deserialize<WritableSubResource>(item.GetRawText()));
                    }
                    backendAddressPools = array;
                    continue;
                }
                if (property.NameEquals("probe"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    probe = JsonSerializer.Deserialize<WritableSubResource>(property.Value.GetRawText());
                    continue;
                }
                if (property.NameEquals("protocol"u8))
                {
                    protocol = new LoadBalancingTransportProtocol(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("loadDistribution"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    loadDistribution = new LoadDistribution(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("frontendPort"u8))
                {
                    frontendPort = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("backendPort"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    backendPort = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("idleTimeoutInMinutes"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    idleTimeoutInMinutes = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("enableFloatingIP"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    enableFloatingIP = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("enableTcpReset"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    enableTcpReset = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("disableOutboundSnat"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    disableOutboundSnat = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("provisioningState"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    provisioningState = new NetworkProvisioningState(property.Value.GetString());
                    continue;
                }
                additionalPropertiesDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
            }
            additionalProperties = additionalPropertiesDictionary;
            return new LoadBalancingRuleProperties(
                frontendIPConfiguration,
                backendAddressPool,
                backendAddressPools ?? new ChangeTrackingList<WritableSubResource>(),
                probe,
                protocol,
                loadDistribution,
                frontendPort,
                backendPort,
                idleTimeoutInMinutes,
                enableFloatingIP,
                enableTcpReset,
                disableOutboundSnat,
                provisioningState,
                additionalProperties);
        }

        BinaryData IPersistableModel<LoadBalancingRuleProperties>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LoadBalancingRuleProperties>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options);
                default:
                    throw new FormatException($"The model {nameof(LoadBalancingRuleProperties)} does not support writing '{options.Format}' format.");
            }
        }

        LoadBalancingRuleProperties IPersistableModel<LoadBalancingRuleProperties>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LoadBalancingRuleProperties>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeLoadBalancingRuleProperties(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(LoadBalancingRuleProperties)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<LoadBalancingRuleProperties>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
