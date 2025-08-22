namespace Microsoft.Extensions.DependencyInjection
{
    using FluentValidation;
    using DTemplate.Business;
    using DTemplate.Business.Core.Infrastructure;
    using DTemplate.Business.Core.Services;
    using DTemplate.Business.MappingProfiles;
    using Sieve.Services;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class ServicesExtensions
    {
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IStorageReaderAdapter, StorageReaderAdapter>();
            services.AddScoped<IStorageWriterAdapter, StorageWriterAdapter>();
            services.AddScoped<IMapperAdapter, MapperAdapter>();
            services.AddScoped<IValidatorAdapter, ValidatorAdapter>();

            services.AddValidatorsFromAssembly(typeof(Constants).Assembly);

            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

            services.AddSingleton<ISieveProcessor, SieveProcessor>();
        }
    }
}
