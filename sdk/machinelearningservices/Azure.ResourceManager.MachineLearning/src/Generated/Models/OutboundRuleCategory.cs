// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.MachineLearning.Models
{
    /// <summary> Category of a managed network Outbound Rule of a machine learning workspace. </summary>
    public readonly partial struct OutboundRuleCategory : IEquatable<OutboundRuleCategory>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="OutboundRuleCategory"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public OutboundRuleCategory(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string RequiredValue = "Required";
        private const string RecommendedValue = "Recommended";
        private const string UserDefinedValue = "UserDefined";
        private const string DependencyValue = "Dependency";

        /// <summary> Required. </summary>
        public static OutboundRuleCategory Required { get; } = new OutboundRuleCategory(RequiredValue);
        /// <summary> Recommended. </summary>
        public static OutboundRuleCategory Recommended { get; } = new OutboundRuleCategory(RecommendedValue);
        /// <summary> UserDefined. </summary>
        public static OutboundRuleCategory UserDefined { get; } = new OutboundRuleCategory(UserDefinedValue);
        /// <summary> Dependency. </summary>
        public static OutboundRuleCategory Dependency { get; } = new OutboundRuleCategory(DependencyValue);
        /// <summary> Determines if two <see cref="OutboundRuleCategory"/> values are the same. </summary>
        public static bool operator ==(OutboundRuleCategory left, OutboundRuleCategory right) => left.Equals(right);
        /// <summary> Determines if two <see cref="OutboundRuleCategory"/> values are not the same. </summary>
        public static bool operator !=(OutboundRuleCategory left, OutboundRuleCategory right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="OutboundRuleCategory"/>. </summary>
        public static implicit operator OutboundRuleCategory(string value) => new OutboundRuleCategory(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is OutboundRuleCategory other && Equals(other);
        /// <inheritdoc />
        public bool Equals(OutboundRuleCategory other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
