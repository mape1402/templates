namespace DTemplate.Business.Core.Services
{
    using DTemplate.Domain.Contracts;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents criteria for retrieving a single entity, supporting both expression-based and string-based filters, and tracking options.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class GetOneCriteria<TEntity>
    {
        /// <summary>
        /// Gets or sets the filter expression to select the entity.
        /// </summary>
        public Expression<Func<TEntity, bool>> FiltersExpression { get; set; }

        /// <summary>
        /// Gets or sets the string-based filter to select the entity.
        /// </summary>
        public string Filters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use tracking when retrieving the entity.
        /// </summary>
        public bool UseTracking { get; set; } = true;
    }

    /// <summary>
    /// Represents criteria for retrieving multiple entities, including filtering, ordering, pagination, and tracking options.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class GetManyCriteria<TEntity>
    {
        /// <summary>
        /// Gets or sets the filter expression to select entities.
        /// </summary>
        public Expression<Func<TEntity, bool>> FiltersExpression { get; set; }

        /// <summary>
        /// Gets or sets the expression to order the entities.
        /// </summary>
        public Expression<Func<TEntity, object>> SortingExpression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sorting is ascending.
        /// </summary>
        public bool AscendentSort { get; set; }

        /// <summary>
        /// Gets or sets the string-based filter to select entities.
        /// </summary>
        public string Filters { get; set; }

        /// <summary>
        /// Gets or sets the string-based sort to order entities.
        /// </summary>
        public string Sorts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use tracking when retrieving the entities.
        /// </summary>
        public bool UseTracking { get; set; } = true;

        /// <summary>
        /// Gets or sets the number of items per page. Null means no paging.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number to retrieve. Null means no paging.
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Determines if the filter expression should be used.
        /// </summary>
        /// <returns>True if a filter expression is set; otherwise, false.</returns>
        public bool UseFiltersExpression() => FiltersExpression != null;

        /// <summary>
        /// Determines if the string-based filter should be used.
        /// </summary>
        /// <returns>True if a string-based filter is set; otherwise, false.</returns>
        public bool UseFilters() => !string.IsNullOrWhiteSpace(Filters);

        /// <summary>
        /// Determines if the sorting expression should be used.
        /// </summary>
        /// <returns>True if a sorting expression is set; otherwise, false.</returns>
        public bool UseSortingExpression() => SortingExpression != null;

        /// <summary>
        /// Determines if the string-based sort should be used.
        /// </summary>
        /// <returns>True if a string-based sort is set; otherwise, false.</returns>
        public bool UseSorts() => !string.IsNullOrWhiteSpace(Sorts);

        /// <summary>
        /// Determines if paging should be used based on the presence and values of PageSize and PageNumber.
        /// </summary>
        /// <returns>True if both PageSize and PageNumber are set and greater than zero; otherwise, false.</returns>
        public bool UsePaging() => PageSize.HasValue && PageNumber.HasValue && PageSize.Value > 0 && PageNumber.Value > 0;
    }

    /// <summary>
    /// Represents a paged result set for batch queries.
    /// </summary>
    /// <typeparam name="T">The type of the result items.</typeparam>
    public class BatchResult<T>
    {
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the total number of rows available.
        /// </summary>
        public long RowCount { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the document.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the results for the current page.
        /// </summary>
        public IEnumerable<T> Results { get; set; }

        /// <summary>
        /// Returns the results as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>If the results are null, an empty enumerable is returned.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the results, or an empty enumerable if no results are available.</returns>
        public IEnumerable<T> AsEnumerable()
            => Results ?? Enumerable.Empty<T>();
    }

    /// <summary>
    /// Defines an abstraction for reading entities from storage with support for filtering, ordering, and pagination.
    /// </summary>
    public interface IStorageReaderAdapter
    {
        /// <summary>
        /// Asynchronously retrieves a single entity matching the specified criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TExpected">The type of the expected result.</typeparam>
        /// <param name="criteria">The criteria for selecting the entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with the expected result as the result.</returns>
        Task<TExpected> GetOneAsync<TEntity, TExpected>(GetOneCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
            where TExpected : class;

        /// <summary>
        /// Asynchronously retrieves multiple entities matching the specified criteria, with support for paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TExpected">The type of the expected result.</typeparam>
        /// <param name="criteria">The criteria for selecting the entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with a batch result containing the expected results as the result.</returns>
        Task<BatchResult<TExpected>> GetManyAsync<TEntity, TExpected>(GetManyCriteria<TEntity> criteria, CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
            where TExpected : class;
    }
}
