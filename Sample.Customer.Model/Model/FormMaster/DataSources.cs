using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public partial class DataSources
    {
        public DataSources()
        {
            DataSourceFields = new HashSet<DataSourceFields>();
        }

        public short DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string Description { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public string ListScreenTitle { get; set; }
        public string AddScreenTitle { get; set; }
        public string EditScreenTitle { get; set; }
        public int? AccountId { get; set; }
        public int? ServiceId { get; set; }

        public virtual ICollection<DataSourceFields> DataSourceFields { get; set; }


    }
    
    public class DataSourceFields
    {
        public List<Field> Fields { get; set; }
    }
}

