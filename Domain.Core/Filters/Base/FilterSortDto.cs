using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Filters.Base
{
    /// <summary>
    /// Параметры сортировки для фильтрации
    /// </summary>
    public sealed class FilterSortDto
    {
        /// <summary>
        /// Название поля для сортировки
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Сортировка в обратном направлении (true - в обратном, false - в прямом)
        /// </summary>
        public bool IsDescending { get; set; }
    }
}
