using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Filters.Base
{
    /// <summary>
    /// Параметры пагинации для фильтрации
    /// </summary>
    public sealed class FilterPagingDto
    {
        /// <summary>
        /// Номер страницы, начиная с нуля
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Размер страницы, по умолчанию - 10
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
