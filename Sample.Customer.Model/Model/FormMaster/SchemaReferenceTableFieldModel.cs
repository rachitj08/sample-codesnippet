using Sample.Customer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Customer.Model
{
    public class SchemaReferenceTableFieldModel
    {
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ReferenceTable { get; set; }
        public string ReferenceCol { get; set; }
        public List<SchemaTableFields> lstSchemaTableFieldsData { get; set; }
    }
}
