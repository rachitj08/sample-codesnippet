using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
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
        public string DefaultValue { get; set; }
        public string HelpText { get; set; }
        public short? MaxLength { get; set; }
        public bool? MultiLine { get; set; }
        public short? MinValue { get; set; }
        public short? MaxValue { get; set; }
        public string ListDataSourceName { get; set; }
        public string ListValueField { get; set; }
        public string ListTextField { get; set; }
        public bool IsPrimaryKey { get; set; }
        public int? AccountId { get; set; }
        public int? ServiceId { get; set; }
        public string ListPrimaryKey { get; set; }
        public string ControlTypeForSearch { get; set; }
        public string ControlTypeForEdit { get; set; }
        public bool? IsIdentity { get; set; }

        public virtual DataSources DataSourceNameNavigation { get; set; }
    }
}
