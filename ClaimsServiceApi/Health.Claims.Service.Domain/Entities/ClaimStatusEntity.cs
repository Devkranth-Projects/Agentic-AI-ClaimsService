using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Entities
{
    public class ClaimStatusEntity : BaseEntity
    {        
        public string StatusName { get; set; } = string.Empty;

        public virtual ICollection<ClaimRecordEntity> Claims { get; set; } = new List<ClaimRecordEntity>();

    }
}
