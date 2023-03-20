using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class State
    {
        public State()
        {
            City = new HashSet<City>();
        }

        public long StateId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
