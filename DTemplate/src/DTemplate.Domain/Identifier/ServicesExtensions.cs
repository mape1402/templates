namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using DTemplate.Domain.Identifier;

    /// <summary>
    /// Provides extension methods for configuring CId identifier services in the dependency injection container.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configures the allowed type, default factory, and database value converter for <see cref="CId"/> identifiers in the service collection.
        /// </summary>
        /// <typeparam name="TTargetType">The type to be used for database storage of the identifier value.</typeparam>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="defaultFactory">The default factory function for creating new <see cref="CId"/> instances.</param>
        /// <param name="convertToDb">A function to convert a <see cref="CId"/> to the database type <typeparamref name="TTargetType"/>.</param>
        /// <param name="convertFromDb">A function to convert the database type <typeparamref name="TTargetType"/> to a <see cref="CId"/>.</param>
        public static void UseCId<TTargetType, TDbType>(this IServiceCollection services, Action<CIdConfiguration<TTargetType, TDbType>> setup)
        {
            var config = new CIdConfiguration<TTargetType, TDbType>();
            setup(config);

            config.ValidateAndThrow();

            CIdMetadata.AllowedType = typeof(TTargetType);
            CIdMetadata.DefaultFactory = config.DefaultFactory;
            CIdMetadata.JsonConverter = config.JsonConverter;
            CIdMetadata.NulleableJsonConverter = config.NulleableJsonConverter;
            CIdMetadata.ParseFunction = config.ParseFunction;
            CIdMetadata.DbConverter = new ValueConverter<CId, TDbType>(config.ConvertToDb, config.ConvertFromDb);

#pragma warning disable CS8632
            CIdMetadata.DbNulleableConverter = new ValueConverter<CId?, TDbType?>(config.ConvertToDbNullable, config.ConvertFromDbNullable);
#pragma warning restore CS8632
        }
    }
}
