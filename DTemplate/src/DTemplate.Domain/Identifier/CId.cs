namespace DTemplate.Domain.Identifier
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a strongly-typed identifier with a value of a specific allowed type.
    /// </summary>
    [TypeConverter(typeof(CIdTypeConverter))]
    public struct CId : IEquatable<CId>
    {
        /// <summary>
        /// Indicates whether the identifier is empty.
        /// </summary>
        private readonly bool _isEmpty = true;

        /// <summary>
        /// Initializes a new empty <see cref="CId"/>.
        /// </summary>
        public CId()
        {
            _isEmpty = true;
        }

        /// <summary>
        /// Initializes a new <see cref="CId"/> with the specified value.
        /// </summary>
        /// <param name="value">The value of the identifier. Must be of the allowed type.</param>
        /// <exception cref="ArgumentException">Thrown if the value is not of the allowed type.</exception>
        public CId(object value)
        {
            if(value.GetType() != CIdMetadata.AllowedType)
                throw new ArgumentException($"Value must be of type {CIdMetadata.AllowedType}.");

            Value = value;
            _isEmpty = false;
        }

        /// <summary>
        /// Gets the value of the identifier.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets an instance of <see cref="CId"/> that represents an empty identifier.
        /// </summary>
        /// <remarks>This property returns a new instance of <see cref="CId"/> with default values,  which
        /// can be used to represent a non-initialized or empty state.</remarks>
        public static CId Empty => new CId();

        /// <summary>
        /// Creates a new <see cref="CId"/> using the default factory.
        /// </summary>
        /// <returns>A new <see cref="CId"/> instance.</returns>
        public static CId New()
            => CIdMetadata.DefaultFactory();

        /// <summary>
        /// Parses the specified string to create a new <see cref="CId"/> instance.
        /// </summary>
        /// <param name="value">The string representation of the CId to parse. Cannot be null or empty.</param>
        /// <returns>A <see cref="CId"/> instance that corresponds to the specified string.</returns>
        public static CId Parse(string value)
            => CIdMetadata.ParseFunction(value);

        /// <summary>
        /// Casts the value of the identifier to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <returns>The value cast to type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">Thrown if the value cannot be cast to the specified type or the identifier is empty.</exception>
        public T Cast<T>()
        {
            if (_isEmpty || Value is not T castValue)
                throw new InvalidCastException($"Value cannot be converted to {typeof(T)}.");

            return castValue;
        }

        /// <summary>
        /// Returns the string representation of the identifier value.
        /// </summary>
        /// <returns>The string representation of the value.</returns>
        public override string ToString()
            => Value?.ToString();

        /// <summary>
        /// Determines whether the specified object is equal to the current identifier.
        /// </summary>
        /// <param name="obj">The object to compare with the current identifier.</param>
        /// <returns><c>true</c> if the specified object is equal to the current identifier; otherwise, <c>false</c>.</returns>
        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is not CId other)
                return false;

            if (other._isEmpty && _isEmpty)
                return true;

            if (Value is null && other.Value is null)
                return true;

            if((Value is null && other.Value is not null) || (Value is not null && other.Value is null))
                return false;

            return other.Value.Equals(Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="CId"/> is equal to the current identifier.
        /// </summary>
        /// <param name="other">The identifier to compare with the current identifier.</param>
        /// <returns><c>true</c> if the specified identifier is equal to the current identifier; otherwise, <c>false</c>.</returns>
        public bool Equals(CId other)
            => Equals((object)other);

        /// <summary>
        /// Returns a hash code for the identifier.
        /// </summary>
        /// <returns>A hash code for the identifier.</returns>
        public override int GetHashCode()
            => _isEmpty ? default : Value.GetHashCode();

        /// <summary>
        /// Determines whether two <see cref="CId"/> instances are equal.
        /// </summary>
        /// <param name="left">The first identifier to compare.</param>
        /// <param name="right">The second identifier to compare.</param>
        /// <returns><c>true</c> if the identifiers are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(CId left, CId right)
            => left.Equals(right);

        /// <summary>
        /// Determines whether two <see cref="CId"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first identifier to compare.</param>
        /// <param name="right">The second identifier to compare.</param>
        /// <returns><c>true</c> if the identifiers are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(CId left, CId right)
            => !left.Equals(right);
    }
}
