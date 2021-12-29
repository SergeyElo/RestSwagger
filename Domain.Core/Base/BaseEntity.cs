using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.Core.Base
{
    public class BaseEntity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }

        public bool? IsActive { get; set; }

        [IndexColumn(IsClustered = false, IsUnique = false)]
        public DateTime DateCreated { get; set; }

        [IndexColumn(IsClustered = false, IsUnique = false)]
        public Guid? CreatorId { get; set; }

        public bool IsDelete { get; set; }

        [IndexColumn(IsClustered = false, IsUnique = false)]
        public DateTime? DateDelete { get; set; }

        [IndexColumn(IsClustered = false, IsUnique = false)]
        public DateTime DateUpdated { get; set; }
    }
}
