using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Filters.Base
{
    /// <summary>
    /// Базовый контейнер фильтра для всех методов фильтрации
    /// </summary>
    public class BaseFilterDto
    {
        /// <summary>
        /// Статус: активен/заблокирован
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
