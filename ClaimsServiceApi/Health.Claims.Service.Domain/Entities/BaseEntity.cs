using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Entities
{
    public class BaseEntity
    {
        public Guid EntityId { get; set; }
        public DateTime CreatedDt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

    }
}
