// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.BotService.Models
{
    public partial class BotServiceHostSettingsResult : IUtf8JsonSerializable, IJsonModel<BotServiceHostSettingsResult>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<BotServiceHostSettingsResult>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<BotServiceHostSettingsResult>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected virtual void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<BotServiceHostSettingsResult>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(BotServiceHostSettingsResult)} does not support writing '{format}' format.");
            }

            if (Optional.IsDefined(OAuthUri))
            {
                writer.WritePropertyName("OAuthUrl"u8);
                writer.WriteStringValue(OAuthUri.AbsoluteUri);
            }
            if (Optional.IsDefined(ToBotFromChannelOpenIdMetadataUri))
            {
                writer.WritePropertyName("ToBotFromChannelOpenIdMetadataUrl"u8);
                writer.WriteStringValue(ToBotFromChannelOpenIdMetadataUri.AbsoluteUri);
            }
            if (Optional.IsDefined(ToBotFromChannelTokenIssuer))
            {
                writer.WritePropertyName("ToBotFromChannelTokenIssuer"u8);
                writer.WriteStringValue(ToBotFromChannelTokenIssuer);
            }
            if (Optional.IsDefined(ToBotFromEmulatorOpenIdMetadataUri))
            {
                writer.WritePropertyName("ToBotFromEmulatorOpenIdMetadataUrl"u8);
                writer.WriteStringValue(ToBotFromEmulatorOpenIdMetadataUri.AbsoluteUri);
            }
            if (Optional.IsDefined(ToChannelFromBotLoginUri))
            {
                writer.WritePropertyName("ToChannelFromBotLoginUrl"u8);
                writer.WriteStringValue(ToChannelFromBotLoginUri.AbsoluteUri);
            }
            if (Optional.IsDefined(ToChannelFromBotOAuthScope))
            {
                writer.WritePropertyName("ToChannelFromBotOAuthScope"u8);
                writer.WriteStringValue(ToChannelFromBotOAuthScope);
            }
            if (Optional.IsDefined(ValidateAuthority))
            {
                writer.WritePropertyName("ValidateAuthority"u8);
                writer.WriteBooleanValue(ValidateAuthority.Value);
            }
            if (Optional.IsDefined(BotOpenIdMetadata))
            {
                writer.WritePropertyName("BotOpenIdMetadata"u8);
                writer.WriteStringValue(BotOpenIdMetadata);
            }
            if (options.Format != "W" && _serializedAdditionalRawData != null)
            {
                foreach (var item in _serializedAdditionalRawData)
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
        }

        BotServiceHostSettingsResult IJsonModel<BotServiceHostSettingsResult>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<BotServiceHostSettingsResult>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(BotServiceHostSettingsResult)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeBotServiceHostSettingsResult(document.RootElement, options);
        }

        internal static BotServiceHostSettingsResult DeserializeBotServiceHostSettingsResult(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Uri oAuthUrl = default;
            Uri toBotFromChannelOpenIdMetadataUrl = default;
            string toBotFromChannelTokenIssuer = default;
            Uri toBotFromEmulatorOpenIdMetadataUrl = default;
            Uri toChannelFromBotLoginUrl = default;
            string toChannelFromBotOAuthScope = default;
            bool? validateAuthority = default;
            string botOpenIdMetadata = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> rawDataDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("OAuthUrl"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    oAuthUrl = new Uri(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("ToBotFromChannelOpenIdMetadataUrl"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    toBotFromChannelOpenIdMetadataUrl = new Uri(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("ToBotFromChannelTokenIssuer"u8))
                {
                    toBotFromChannelTokenIssuer = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("ToBotFromEmulatorOpenIdMetadataUrl"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    toBotFromEmulatorOpenIdMetadataUrl = new Uri(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("ToChannelFromBotLoginUrl"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    toChannelFromBotLoginUrl = new Uri(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("ToChannelFromBotOAuthScope"u8))
                {
                    toChannelFromBotOAuthScope = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("ValidateAuthority"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    validateAuthority = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("BotOpenIdMetadata"u8))
                {
                    botOpenIdMetadata = property.Value.GetString();
                    continue;
                }
                if (options.Format != "W")
                {
                    rawDataDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = rawDataDictionary;
            return new BotServiceHostSettingsResult(
                oAuthUrl,
                toBotFromChannelOpenIdMetadataUrl,
                toBotFromChannelTokenIssuer,
                toBotFromEmulatorOpenIdMetadataUrl,
                toChannelFromBotLoginUrl,
                toChannelFromBotOAuthScope,
                validateAuthority,
                botOpenIdMetadata,
                serializedAdditionalRawData);
        }

        BinaryData IPersistableModel<BotServiceHostSettingsResult>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<BotServiceHostSettingsResult>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options);
                default:
                    throw new FormatException($"The model {nameof(BotServiceHostSettingsResult)} does not support writing '{options.Format}' format.");
            }
        }

        BotServiceHostSettingsResult IPersistableModel<BotServiceHostSettingsResult>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<BotServiceHostSettingsResult>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeBotServiceHostSettingsResult(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(BotServiceHostSettingsResult)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<BotServiceHostSettingsResult>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
