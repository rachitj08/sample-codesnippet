using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ScreenFields
    {
        public short ScreenId { get; set; }
        public short FieldId { get; set; }
        public bool UseShortLabel { get; set; }
        public short DisplayOrder { get; set; }
        public short? DatasourceDisplayType { get; set; }
        public string DisplayMaxLength { get; set; }
        public double? ColumnWidthPer { get; set; }
        public bool? Hidden { get; set; }
        public bool? ReadOnly { get; set; }
        public int? AccountId { get; set; }
    }
}
