using Domain.Core.Base;
using Domain.Core.Filters.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public abstract class BaseRepository<TModel, TFilter, TKey> : IBaseRepository<TModel, TFilter, TKey>
        where TModel : BaseEntity<TKey>
        where TFilter : BaseFilterDto
        where TKey : struct, IEquatable<TKey>
    {

        private readonly Context Context;

        protected BaseRepository(Context context)
        {
            Context = context;
        }

        public async Task<TModel> GetById(TKey guid, CancellationToken cancellationToken = default)
        {
            var data = await GetDataSet().FirstOrDefaultAsync(u => u.Id.Equals(guid), cancellationToken);
            return data;
        }

        public async Task<TModel> Create(TModel data, Guid? creatorId = null, CancellationToken cancellationToken = default)
        {
            data.CreatorId = creatorId;
            data.DateCreated = DateTime.UtcNow;
            data.IsActive ??= true;
            data.DateUpdated = DateTime.UtcNow;
            await Context.Set<TModel>().AddAsync(data, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<TModel> Edit(TModel data, CancellationToken cancellationToken = default)
        {
            data.DateUpdated = DateTime.UtcNow;
            Context.Update(data);
            await Context.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<TModel> Delete(TModel data, CancellationToken cancellationToken = default)
        {
            data.IsDelete = true;
            data.DateDelete = DateTime.UtcNow;
            data.DateUpdated = DateTime.UtcNow;
            Context.Update(data);
            await Context.SaveChangesAsync(cancellationToken);
            return data;
        }

        public async Task<long> GetCount(TFilter filter = null, CancellationToken cancellationToken = default)
        {
            var result = GetDataSet();
            if (filter != null)
            {
                result = ApplyFilter(result, filter);
            }

            return await result.LongCountAsync(cancellationToken);
        }

        /// <summary>
        /// Get filtered entities
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TModel>> GetFiltered(
            TFilter filter = null,
            FilterSortDto sort = null,
            FilterPagingDto paging = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetDataSet();

            if (filter != null)
            {
                query = ApplyFilter(query, filter);
            }

            if (sort != null)
            {
                query = ApplySort(query, sort);
            }

            if (paging != null)
            {
                query = ApplyPaging(query, paging);
            }

            var result = await query.ToArrayAsync(cancellationToken);
            return result;
        }


        protected IQueryable<TModel> GetDataSet(bool withoutDeleted = true)
        {
            var query = Context.Set<TModel>()
                .AsNoTracking();

            if (withoutDeleted)
            {
                query = query.Where(x => x.IsDelete == false);
            }
            query = EntityContextFilter(query);

            return query;
        }

        protected virtual IQueryable<TModel> EntityContextFilter(IQueryable<TModel> source)
            => source;

        protected virtual IQueryable<TModel> ApplyFilter(IQueryable<TModel> result, TFilter filter)
        {
            if (filter.IsActive.HasValue)
                result = result.Where(p => p.IsActive == filter.IsActive);

            return result;
        }

        protected virtual IQueryable<TModel> ApplySort(IQueryable<TModel> source, FilterSortDto sort)
        {
            sort ??= new FilterSortDto
            {
                ColumnName = nameof(BaseEntity<TKey>.DateCreated),
                IsDescending = true
            };

            if (sort.ColumnName == nameof(BaseEntity<TKey>.DateCreated))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.DateCreated)
                    : source.OrderBy(p => p.DateCreated);

            if (sort.ColumnName == nameof(BaseEntity<TKey>.IsActive))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.IsActive)
                    : source.OrderBy(p => p.IsActive);

            if (sort.ColumnName == nameof(BaseEntity<TKey>.DateCreated))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.DateCreated)
                    : source.OrderBy(p => p.DateCreated);

            if (sort.ColumnName == nameof(BaseEntity<TKey>.IsActive))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.IsActive)
                    : source.OrderBy(p => p.IsActive);

            return source;
        }

        protected virtual IQueryable<TModel> ApplyPaging(IQueryable<TModel> source, FilterPagingDto paging)
        {
            paging ??= new FilterPagingDto { PageSize = 10 };
            return source
                .Skip(paging.PageNumber * paging.PageSize)
                .Take(paging.PageSize);
        }

    }
}
