namespace DTemplate.Business.Core.Infrastructure
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using DTemplate.Business.Core.Services;
    using DTemplate.Domain.Contracts;
    using DTemplate.Persistence.Abstractions;
    using Sieve.Models;
    using Sieve.Services;

    /// <summary>
    /// Provides an implementation of <see cref="IStorageReaderAdapter"/> for reading entities from storage with support for filtering, sorting, and paging.
    /// </summary>
    internal class StorageReaderAdapter : IStorageReaderAdapter
    {
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageReaderAdapter"/> class.
        /// </summary>
        /// <param name="sieveProcessor">The Sieve processor for applying filters and sorts.</param>
        /// <param name="dbContext">The database context for entity access.</param>
        /// <param name="mapper">The AutoMapper instance for projection.</param>
        public StorageReaderAdapter(ISieveProcessor sieveProcessor, IDbContext dbContext, IMapper mapper)
        {
            _sieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Asynchronously retrieves a single entity matching the specified criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TExpected">The type of the expected result.</typeparam>
        /// <param name="criteria">The criteria for selecting the entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the expected result as the result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if criteria is null.</exception>
        /// <exception cref="ArgumentException">Thrown if FiltersExpression is null.</exception>
        public async Task<TExpected> GetOneAsync<TEntity, TExpected>(GetOneCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
            where TExpected : class
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            if (criteria.FiltersExpression == null)
                throw new ArgumentException("FiltersExpression cannot be null.", nameof(criteria));

            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (!criteria.UseTracking)
                query = query.AsNoTracking();

            query = query.Where(criteria.FiltersExpression);

            if(typeof(TEntity) == typeof(TExpected))
            {
                var entity = await query.FirstOrDefaultAsync(cancellationToken);
                return entity as TExpected;
            }

            return await query.ProjectTo<TExpected>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves multiple entities matching the specified criteria, with support for filtering, sorting, and paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TExpected">The type of the expected result.</typeparam>
        /// <param name="criteria">The criteria for selecting the entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with a batch result containing the expected results as the result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if criteria is null.</exception>
        public async Task<BatchResult<TExpected>> GetManyAsync<TEntity, TExpected>(GetManyCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
            where TExpected : class
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (!criteria.UseTracking)
                query = query.AsNoTracking();

            query = ApplyExpressions(query, criteria);
            query = ApplySieve(query, criteria);
            var pagedQuery = ApplyPaging(query, criteria);

            var results = Enumerable.Empty<TExpected>();

            if (typeof(TEntity) == typeof(TExpected))
            {
                var entities = await pagedQuery.query.ToListAsync(cancellationToken);
                results = entities.Cast<TExpected>();
            }
            else
                results = await pagedQuery.query.ProjectTo<TExpected>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new BatchResult<TExpected>
            {
                PageSize = pagedQuery.pageSize,
                PageNumber = pagedQuery.pageNumber,
                RowCount = pagedQuery.rowCount,
                PageCount = pagedQuery.pageCount,
                Results = results
            };
        }

        /// <summary>
        /// Applies filter and sorting expressions to the queryable source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The queryable source.</param>
        /// <param name="criteria">The criteria containing expressions.</param>
        /// <returns>The queryable source with expressions applied.</returns>
        private IQueryable<TEntity> ApplyExpressions<TEntity>(IQueryable<TEntity> source, GetManyCriteria<TEntity> criteria)
            where TEntity : BaseEntity
        {
            if (criteria.UseFiltersExpression())
                source = source.Where(criteria.FiltersExpression);

            if (criteria.UseSortingExpression())
            {
                source = criteria.AscendentSort
                    ? source.OrderBy(criteria.SortingExpression)
                    : source.OrderByDescending(criteria.SortingExpression);
            }

            return source;
        }

        /// <summary>
        /// Applies Sieve-based filtering and sorting to the queryable source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The queryable source.</param>
        /// <param name="criteria">The criteria containing Sieve filters and sorts.</param>
        /// <returns>The queryable source with Sieve filters and sorts applied.</returns>
        private IQueryable<TEntity> ApplySieve<TEntity>(IQueryable<TEntity> source, GetManyCriteria<TEntity> criteria)
            where TEntity : BaseEntity
        {
            if (!criteria.UseFilters() && !criteria.UseSorts())
                return source;

            var sieveModel = new SieveModel
            {
                Filters = criteria.Filters,
                Sorts = criteria.Sorts
            };

            return _sieveProcessor.Apply(sieveModel, source, applyPagination: false);
        }

        /// <summary>
        /// Applies paging to the queryable source based on the criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The queryable source.</param>
        /// <param name="criteria">The criteria containing paging information.</param>
        /// <returns>A tuple containing the paged query, row count, page count, page number, and page size.</returns>
        private (IQueryable<TEntity> query, int rowCount, int pageCount, int pageNumber, int pageSize) ApplyPaging<TEntity>(IQueryable<TEntity> source, GetManyCriteria<TEntity> criteria)
            where TEntity : BaseEntity
        {
            var rowCount = source.Count();

            if (!criteria.UsePaging())
                return (source, rowCount, 1, 1, rowCount);

            if(rowCount == default)
                return (source, rowCount, 0, 0, criteria.PageSize.Value);

            var pageCount = rowCount / criteria.PageSize.Value + (rowCount % criteria.PageSize.Value > 0 ? 1 : 0);

            var pageSize = criteria.PageSize.Value;
            var pageNumber = criteria.PageNumber.Value > pageCount ? pageCount : criteria.PageNumber.Value;

            return (source.Skip((pageNumber - 1) * pageSize).Take(pageSize), rowCount, pageCount, pageNumber, pageSize);
        }
    }
}
