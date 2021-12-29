using Domain.Core.Base;
using Domain.Core.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<TModel, TFilter, TKey>
        where TModel : BaseEntity<TKey>
        where TFilter : BaseFilterDto
        where TKey : struct, IEquatable<TKey>
    {
        Task<TModel> GetById(TKey guid, CancellationToken cancellationToken = default);

        Task<TModel> Create(TModel data, Guid? creatorId = null, CancellationToken cancellationToken = default);
        Task<TModel> Edit(TModel data, CancellationToken cancellationToken = default);
        Task<TModel> Delete(TModel data, CancellationToken cancellationToken = default);

        Task<IEnumerable<TModel>> GetFiltered(
            TFilter filter = null,
            FilterSortDto sort = null,
            FilterPagingDto paging = null,
            CancellationToken cancellationToken = default);


        Task<long> GetCount(TFilter filter = null, CancellationToken cancellationToken = default);
    }
}
