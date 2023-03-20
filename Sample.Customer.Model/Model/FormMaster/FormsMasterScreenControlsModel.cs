using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sample.Customer.Model
{

    public class ScreenControlsVM
    {
        public int DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string Description { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanSearch { get; set; }
        public string ListScreenTitle { get; set; }
        public string AddScreenTitle { get; set; }
        public string EditScreenTitle { get; set; }
        public int AccountId { get; set; }
        public string ScreenType { get; set; }
        public List<Fields> Fields { get; set; }
        public List<SearchFields> SearchFields { get; set; }
    }
    public class FormsMasterScreenControlsModel
    {
        public int DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string Description { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanSearch { get; set; }
        public string ListScreenTitle { get; set; }
        public string AddScreenTitle { get; set; }
        public string EditScreenTitle { get; set; }
        public int AccountId { get; set; }
        public string ScreenType { get; set; }

        public short FieldId { get; set; }
        public string FieldName { get; set; }
        public string ValidationType { get; set; }
        public string ControlType { get; set; }
        public string ControlTypeSearch { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public string HelpText { get; set; }
        public int? MaxLength { get; set; }
        public bool? Multiline { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string ListDataSourceName { get; set; }
        public string ListValueField { get; set; }
        public string ListTextField { get; set; }
        public bool UseShortLabel { get; set; }
        public short DisplayOrder { get; set; }
        public short? DatasourceDisplayType { get; set; }
        public string DisplayMaxLength { get; set; }
        public double? ColumnWidthPer { get; set; }
        public bool? Hidden { get; set; }
        public bool? ReadOnly { get; set; }
        public List<Fields> Fields { get; set; }

    }

    public class Fields
    {
        public short FieldId { get; set; }
        public string FieldName { get; set; }
        public string ValidationType { get; set; }
        public string ControlType { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public string HelpText { get; set; }
        public int? MaxLength { get; set; }
        public bool? Multiline { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string ListDataSourceName { get; set; }
        public string ListValueField { get; set; }
        public string ListTextField { get; set; }
        public bool UseShortLabel { get; set; }
        public short DisplayOrder { get; set; }
        public short? DatasourceDisplayType { get; set; }
        public string DisplayMaxLength { get; set; }
        public double? ColumnWidthPer { get; set; }
        public bool? Hidden { get; set; }
        public bool? ReadOnly { get; set; }
        [NotMapped]
        public dynamic DataSource { get; set; }
        [NotMapped]
        public List<ScreenControlValidationVM> Validations { get; set; }
    }

    public class SearchFields
    {
        public short FieldId { get; set; }
        public string FieldName { get; set; }
        public string ValidationType { get; set; }
        public string ControlType { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public string DefaultValue { get; set; }
        public string HelpText { get; set; }
        public int? MaxLength { get; set; }
        public bool? Multiline { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string ListDataSourceName { get; set; }
        public string ListValueField { get; set; }
        public string ListTextField { get; set; }
        public bool UseShortLabel { get; set; }
        public short DisplayOrder { get; set; }
        public short? DatasourceDisplayType { get; set; }
        public string DisplayMaxLength { get; set; }
        public double? ColumnWidthPer { get; set; }
        public bool Required { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool? Hidden { get; set; }
        public bool? ReadOnly { get; set; }
        [NotMapped]
        public dynamic DataSource { get; set; }
        [NotMapped]
        public List<ScreenControlValidationVM> Validations { get; set; }
    }

    public class ScreenControlValidationVM
    {
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string validator { get; set; }
        public string message { get; set; }
    }

    public class ControlsData
    {
        public int DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public int DataSourcePrimaryKey { get; set; }
        public string Description { get; set; }
        public string Account_Id { get; set; }
        public string ScreenType { get; set; }
        public string FieldId { get; set; }
        public string FieldName { get; set; }
        public string IsPrimaryKey { get; set; }
        public string ControlType { get; set; }
        public string FieldValue { get; set; }
        public string FieldText { get; set; }
        public List<RowSet> Rows { get; set; }
        public List<FieldProperties> Row { get; set; }
        public List<DropDownDataSource> Dropdowndatasource { get; set; }
        public List<DropDownPickList> Dropdownpicklist { get; set; }
        public List<RadioDataSource> Radiodatasource { get; set; }
        public List<RadioPickList> Radiopicklist { get; set; }
    }
    public class ControlsDataVM
    {
        public ControlsDataVM()
        {
            Rows1 = new List<Dictionary<string, object>>();
        }
        public int DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string Description { get; set; }
        public string Account_Id { get; set; }
        public string ScreenType { get; set; }
        public List<Dictionary<string, object>> Rows1 { get; set; }
    }

    public class ControlsDataResponseVM
    {
        public ControlsDataResponseVM()
        {
            Data = new List<Dictionary<string, object>>();
        }
        public int DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string Description { get; set; }
        public long AccountId { get; set; }
        public string ScreenType { get; set; }
        public string SourceSchema { get; set; }
        public string DataSourcePrimaryKey { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
    }

    public class RowItemCollection {
        public RowItemCollection()
        {
            Row = new List<List<RowItem>>();
        }
        public List<List<RowItem>> Row { get; set; }
    }

    public class RowItem
    {
        public string FieldName { get; set; }
        public dynamic FieldValue { get; set; }
        public string ControlType { get; set; }
        public bool IsPrimaryKeyld { get; set; }

    }
    public class ColumnResponse
    {
        public Dictionary<string, dynamic> Col { get; set; }
    }
    public class ColumnResponseCollection
    {
        public List<ColumnResponse> colList { get; set; }
    }
    public class RowResponse
    {
        public List<ColumnResponseCollection> row { get; set; }
    }

    public class FieldCollection
    {
        public string FieldId { get; set; }
        public string FieldName { get; set; }
        public dynamic FieldValue { get; set; }
        public string ControlType { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
    public class RowSet {

        public RowSet()
        {
            Row = new List<FieldProperties>();
        }
        public List<FieldProperties> Row { get; set; }
    }
    public class FieldProperties
    {
        public string FieldId { get; set; }
        public string FieldName { get; set; }
        public dynamic FieldValue { get; set; }
        public string ControlType { get; set; }
        public string IsPrimaryKey { get; set; }

    }


    public class DropDownPickList
    {
        [JsonPropertyName("ListValueField")]
        public string FieldValue { get; set; }

        [JsonPropertyName("ListTextField")]
        public string FieldText { get; set; }
        public bool IsSelected { get; set; }
    }
    public class DropDownDataSource
    {
        [Column("FieldValue")]
        public string ListValueField { get; set; }
        [Column("FieldText")]
        public string ListTextField { get; set; }
        public bool IsSelected { get; set; }
    }
    public class RadioPickList
    {
        [Column("FieldValue")]
        public string ListValueField { get; set; }
        [Column("FieldText")]
        public string ListTextField { get; set; }
        public bool IsSelected { get; set; }
    }
    public class RadioDataSource
    {
        [Column("FieldValue")]
        public string ListValueField { get; set; }
        [Column("FieldText")]
        public string ListTextField { get; set; }
        public bool IsSelected { get; set; }
    }

}
