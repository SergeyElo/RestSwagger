using Domain.Core.Filters.News;
using Domain.Core.Models.News;
using Domain.Interfaces;
using Persistence.Contexts;
using System;

namespace Persistence.Repositories
{
    public class NewsRepository : BaseRepository<NewsEntity, NewsEntityFilterDto, Guid>, INewsRepository
    {
        public NewsRepository(Context context) : base(context)
        {

        }
    }
}
