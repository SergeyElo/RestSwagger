using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.News
{
    /// <summary>
    /// Расширенная модель новостей
    /// </summary>
    public class NewsFullDto : NewsDto
    {
        /// <summary>
        /// Содержимое новости
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Содержимое новости на английском
        /// </summary>
        public string DescriptionEng { get; set; }
    }
}
