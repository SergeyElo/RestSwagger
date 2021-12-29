using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.News
{
    /// <summary>
    /// Модель для обновления новости
    /// </summary>
    public class UpdateNewsItemDto
    {
        /// <summary>
        /// Заголовок новости
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Заголовок новости на английском
        /// </summary>
        public string TitleEng { get; set; }

        /// <summary>
        /// Содержимое новости
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Содержимое новости на английском
        /// </summary>
        public string DescriptionEng { get; set; }

        /// <summary>
        /// Ссылка на картинку
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
