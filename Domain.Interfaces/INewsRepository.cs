using Domain.Core.Filters.News;
using Domain.Core.Models.News;
using System;

namespace Domain.Interfaces
{
    public interface INewsRepository : IBaseRepository<NewsEntity, NewsEntityFilterDto, Guid>
    {

    }
}
