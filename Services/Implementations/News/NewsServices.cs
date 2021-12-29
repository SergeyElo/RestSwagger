using Domain.Core.Filters.Base;
using Domain.Core.Filters.News;
using Domain.Core.Models.News;
using Domain.Interfaces;
using Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Implementations.News
{
    public class NewsServices : INewsServices
    {
        public readonly INewsRepository _newsRepository;
        public NewsServices(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Guid> Create(NewsEntity newsEntity, CancellationToken cancellationToken = default)
        {
            var result = await _newsRepository.Create(newsEntity, cancellationToken: cancellationToken);

            return result.Id;
        }

        public async Task<NewsEntity> Delete(NewsEntity newsEntity, CancellationToken cancellationToken = default)
            => await _newsRepository.Delete(newsEntity, cancellationToken);

        public async Task<IEnumerable<NewsEntity>> GetByFilter(NewsEntityFilterDto filter = null, FilterPagingDto paging = null, CancellationToken cancellationToken = default)
            => await _newsRepository.GetFiltered(filter: filter, paging: paging, cancellationToken: cancellationToken);

        public async Task<NewsEntity> GetById(Guid id, CancellationToken cancellationToken = default)
        => await _newsRepository.GetById(id, cancellationToken);

        public async Task<NewsEntity> Update(NewsEntity newsEntity, CancellationToken cancellationToken = default)
        => await _newsRepository.Edit(newsEntity, cancellationToken);
    }
}
