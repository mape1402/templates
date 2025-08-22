namespace DTemplate.Domain.Identifier
{
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    /// <summary>
    /// Provides metadata and configuration for the <see cref="CId"/> identifier type.
    /// </summary>
    public static class CIdMetadata
    {
        /// <summary>
        /// Gets or sets the allowed type for the value of a <see cref="CId"/>.
        /// </summary>
        public static Type AllowedType { get; internal set; } 

        /// <summary>
        /// Gets or sets the default factory function for creating new <see cref="CId"/> instances.
        /// </summary>
        public static Func<CId> DefaultFactory { get; internal set; } = () => new CId();

        /// <summary>
        /// Gets or sets the value converter used for database persistence of <see cref="CId"/> values.
        /// </summary>
        public static ValueConverter DbConverter { get; internal set; }

        /// <summary>
        /// Gets or sets the value converter used for database persistence of nullable <see cref="CId"/> values.
        /// </summary>
        public static ValueConverter DbNulleableConverter { get; internal set; }

        /// <summary>
        /// Gets or sets the function used to convert a JSON string to a <see cref="CId"/> object.
        /// </summary>
        /// <remarks>This property is intended for internal use to facilitate JSON deserialization of <see
        /// cref="CId"/> objects.</remarks>
        public static Func<string, CId> JsonConverter { get; internal set; } = value => new CId(value);

        /// <summary>
        /// Gets or sets the function used to convert a JSON string to a nullable <see cref="CId"/> object.
        /// </summary>
        public static Func<string, CId?> NulleableJsonConverter { get; internal set; } = value => new CId(value);

        /// <summary>
        /// Gets or sets the function used to parse a string into a <see cref="CId"/> object.
        /// </summary>
        /// <remarks>The function is used to convert a string representation into a <see cref="CId"/>
        /// instance. This property is intended for internal configuration and should be set with caution.</remarks>
        public static Func<string, CId> ParseFunction { get; internal set; } = value => new CId(value);
    }
}
