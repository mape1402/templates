namespace DTemplate.Domain.Identifier
{
    using System.Linq.Expressions;

    /// <summary>
    /// Provides configuration options for <see cref="CId"/> identifier mapping and conversion to and from a database type.
    /// </summary>
    /// <typeparam name="TTargetType">The type used for the identifier in the domain model.</typeparam>
    /// <typeparam name="TDbType">The type used for the identifier in the database.</typeparam>
    public class CIdConfiguration<TTargetType, TDbType> 
    {
        /// <summary>
        /// Gets or sets the default factory function for creating new <see cref="CId"/> instances.
        /// </summary>
        public Func<CId> DefaultFactory { get; set; }

        /// <summary>
        /// Gets or sets the expression to convert a <see cref="CId"/> to the database type <typeparamref name="TDbType"/>.
        /// </summary>
        public Expression<Func<CId, TDbType>> ConvertToDb { get; set; }

        /// <summary>
        /// Gets or sets the expression to convert the database type <typeparamref name="TDbType"/> to a <see cref="CId"/>.
        /// </summary>
        public Expression<Func<TDbType, CId>> ConvertFromDb { get; set; }

#pragma warning disable CS8632

        /// <summary>
        /// Gets or sets the expression to convert a nullable <see cref="CId"/> to a nullable database type <typeparamref name="TDbType"/>.
        /// </summary>
        public Expression<Func<CId?, TDbType?>> ConvertToDbNullable { get; set; }

        /// <summary>
        /// Gets or sets the expression to convert a nullable database type <typeparamref name="TDbType"/> to a nullable <see cref="CId"/>.
        /// </summary>
        public Expression<Func<TDbType?, CId?>> ConvertFromDbNullable { get; set; }

#pragma warning restore CS8632

        /// <summary>
        /// Gets or sets the function used to convert a JSON string to a <see cref="CId"/> object.
        /// </summary>
        /// <remarks>This property is intended for internal use to facilitate JSON deserialization of <see
        /// cref="CId"/> objects.</remarks>
        public Func<string, CId> JsonConverter { get; set; }

        /// <summary>
        /// Gets or sets the function used to convert a JSON string to a nullable <see cref="CId"/> object.
        /// </summary>
        public Func<string, CId?> NulleableJsonConverter { get; set; }

        /// <summary>
        /// Gets or sets the function used to parse a string into a <see cref="CId"/> object.
        /// </summary>
        /// <remarks>The function is used to convert a string representation into a <see cref="CId"/>
        /// instance. This property is intended for internal configuration and should be set with caution.</remarks>
        public Func<string, CId> ParseFunction { get; set; }

        internal void ValidateAndThrow()
        {
            if(ConvertToDb == null)
                throw new InvalidOperationException("ConvertToDb must be set.");

            if(ConvertFromDb == null)   
                throw new InvalidOperationException("ConvertFromDb must be set.");

            if(ConvertToDbNullable == null)
                throw new InvalidOperationException("ConvertToDbNullable must be set.");

            if(ConvertFromDbNullable == null)
                throw new InvalidOperationException("ConvertFromDbNullable must be set.");

            if(JsonConverter == null)
                throw new InvalidOperationException("JsonConverter must be set.");

            if(NulleableJsonConverter == null)
                throw new InvalidOperationException("NulleableJsonConverter must be set.");

            if(DefaultFactory == null)
                throw new InvalidOperationException("DefaultFactory must be set.");

            if(ParseFunction == null)
                throw new InvalidOperationException("ParseFunction must be set.");
        }
    }
}
