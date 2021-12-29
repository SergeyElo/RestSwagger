using Domain.Core.Base;
using System;

namespace Domain.Core.Models.News
{
    public class NewsEntity : BaseEntity<Guid>
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
