using Sample.Customer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsMaster.DataSourceFields.Model
{
    public partial class DataSourceFields
    {
        public short FieldId { get; set; }
        public string FieldName { get; set; }
        public string DataSourceName { get; set; }
        public string ValidationType { get; set; }
        public string ControlTypeForAdd { get; set; }
        public string ControlTypeForView { get; set; }
        public string ControlTypeForList { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public bool Required { get; set; }
        public short? DefaultValue { get; set; }
        public string HelpText { get; set; }
        public short? MaxLength { get; set; }
        public bool? MultiLine { get; set; }
        public short? MinValue { get; set; }
        public short? MaxValue { get; set; }
        public string ListDataSourceName { get; set; }
        public string ListValueField { get; set; }
        public string ListTextField { get; set; }
        public bool IsPrimaryKey { get; set; }

        public virtual DataSources DataSourceNameNavigation { get; set; }
    }
}
