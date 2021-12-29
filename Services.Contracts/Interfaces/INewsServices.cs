using Domain.Core.Filters.Base;
using Domain.Core.Filters.News;
using Domain.Core.Models.News;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Contracts.Interfaces
{
    public interface INewsServices
    {
        Task<Guid> Create(NewsEntity newsEntity, CancellationToken cancellationToken = default);
        Task<NewsEntity> Update(NewsEntity newsEntity, CancellationToken cancellationToken = default);
        Task<NewsEntity> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsEntity>> GetByFilter(NewsEntityFilterDto filter = null, FilterPagingDto paging = null, CancellationToken cancellationToken = default);
        Task<NewsEntity> Delete(NewsEntity newsEntity, CancellationToken cancellationToken = default);
    }
}
