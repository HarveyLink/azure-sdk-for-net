// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.AppPlatform.Models
{
    [PersistableModelProxy(typeof(UnknownCertificateProperties))]
    public partial class AppPlatformCertificateProperties : IUtf8JsonSerializable, IJsonModel<AppPlatformCertificateProperties>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<AppPlatformCertificateProperties>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<AppPlatformCertificateProperties>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected virtual void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformCertificateProperties>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(AppPlatformCertificateProperties)} does not support writing '{format}' format.");
            }

            writer.WritePropertyName("type"u8);
            writer.WriteStringValue(CertificatePropertiesType);
            if (options.Format != "W" && Optional.IsDefined(Thumbprint))
            {
                writer.WritePropertyName("thumbprint"u8);
                writer.WriteStringValue(Thumbprint);
            }
            if (options.Format != "W" && Optional.IsDefined(Issuer))
            {
                writer.WritePropertyName("issuer"u8);
                writer.WriteStringValue(Issuer);
            }
            if (options.Format != "W" && Optional.IsDefined(IssuedOn))
            {
                writer.WritePropertyName("issuedDate"u8);
                writer.WriteStringValue(IssuedOn.Value, "O");
            }
            if (options.Format != "W" && Optional.IsDefined(ExpireOn))
            {
                writer.WritePropertyName("expirationDate"u8);
                writer.WriteStringValue(ExpireOn.Value, "O");
            }
            if (options.Format != "W" && Optional.IsDefined(ActivateOn))
            {
                writer.WritePropertyName("activateDate"u8);
                writer.WriteStringValue(ActivateOn.Value, "O");
            }
            if (options.Format != "W" && Optional.IsDefined(SubjectName))
            {
                writer.WritePropertyName("subjectName"u8);
                writer.WriteStringValue(SubjectName);
            }
            if (options.Format != "W" && Optional.IsCollectionDefined(DnsNames))
            {
                writer.WritePropertyName("dnsNames"u8);
                writer.WriteStartArray();
                foreach (var item in DnsNames)
                {
                    writer.WriteStringValue(item);
                }
                writer.WriteEndArray();
            }
            if (options.Format != "W" && Optional.IsDefined(ProvisioningState))
            {
                writer.WritePropertyName("provisioningState"u8);
                writer.WriteStringValue(ProvisioningState.Value.ToString());
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

        AppPlatformCertificateProperties IJsonModel<AppPlatformCertificateProperties>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformCertificateProperties>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(AppPlatformCertificateProperties)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeAppPlatformCertificateProperties(document.RootElement, options);
        }

        internal static AppPlatformCertificateProperties DeserializeAppPlatformCertificateProperties(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            if (element.TryGetProperty("type", out JsonElement discriminator))
            {
                switch (discriminator.GetString())
                {
                    case "ContentCertificate": return AppPlatformContentCertificateProperties.DeserializeAppPlatformContentCertificateProperties(element, options);
                    case "KeyVaultCertificate": return AppPlatformKeyVaultCertificateProperties.DeserializeAppPlatformKeyVaultCertificateProperties(element, options);
                }
            }
            return UnknownCertificateProperties.DeserializeUnknownCertificateProperties(element, options);
        }

        BinaryData IPersistableModel<AppPlatformCertificateProperties>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformCertificateProperties>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options);
                default:
                    throw new FormatException($"The model {nameof(AppPlatformCertificateProperties)} does not support writing '{options.Format}' format.");
            }
        }

        AppPlatformCertificateProperties IPersistableModel<AppPlatformCertificateProperties>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformCertificateProperties>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeAppPlatformCertificateProperties(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(AppPlatformCertificateProperties)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<AppPlatformCertificateProperties>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
