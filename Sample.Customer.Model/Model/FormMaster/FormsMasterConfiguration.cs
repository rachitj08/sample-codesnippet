using System;
using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class FormsMasterConfigurationRoot
    {
        public FormsMasterConfiguration data { get; set; }
    }

    public class FormsMasterConfiguration
    {
        public int DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string Description { get; set; }
        public string ScreenType { get; set; }
        public long AccountId { get; set; }
        public string Title { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanSearch { get; set; }
        public string ListScreenTitle { get; set; }
        public string AddScreenTitle { get; set; }
        public string EditScreenTitle { get; set; }
        public string Screen { get; set; }
        public DataSourceConcreteFields DataSourceConcreteFields { get; set; }
        public DataSourceFieldsConf DataSourceFields { get; set; }
        public ListScreens listScreens { get; set; }
        public ViewScreen viewScreen { get; set; }
        public AddScreens addScreens { get; set; }
        public EditScreens editScreens { get; set; }
        public SearchScreens SearchScreens { get; set; }
    }

    public class Validation
    {
        public int FieldId { get; set; }
        public string ValidationTypeId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Field
    {
        public int FieldID { get; set; }
        public string FieldName { get; set; }
        public string ControlType { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public string HelpText { get; set; }
        public int MaxLength { get; set; }
        public bool Multiline { get; set; }
        public string ListDataSourceName { get; set; }
        public string ListValueField { get; set; }
        public string ListTextField { get; set; }
        public string ControlTypeForAdd { get; set; }
        public string ControlTypeForView { get; set; }
        public string ControlTypeForList { get; set; }
        public string ControlTypeForSearch { get; set; }
        public string ControlTypeForEdit { get; set; }
        public List<Validation> Validations { get; set; }

    }
    public class SchemaTableFields
    {
        public string ColumnName { get; set; }
        public string DataTypeName { get; set; }
        public int ColumnMinSize { get; set; }
        public int ColumnMaxSize { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsForeginKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsNull { get; set; }
        public string ConstraintType { get; set; }
        public string ForeignTableName { get; set; }
        public string ForeignColumnName { get; set; }
    }
    public class ScreensField
    {
        public string FieldName { get; set; }
        public bool UseShortLabel { get; set; }
        public int DisplayOrder { get; set; }
        public int DatasourceDisplayType { get; set; }
        public string DisplayMaxLength { get; set; }
        public int ColumnWidthPer { get; set; }
        public bool Hidden { get; set; }
        public bool ReadOnly { get; set; }
    }
    public class DataSourceConcreteFields
    {
        public List<SchemaTableFields> lstSchemaTableFields { get; set; }
    }
    public class DataSourceFieldsConf
    {
        public List<Field> Fields { get; set; }
    }

    public class ListScreens
    {
        public List<ScreensField> Fields { get; set; }
    }

    public class ViewScreen
    {
        public List<ScreensField> Fields { get; set; }
    }

    public class AddScreens
    {
        public List<ScreensField> Fields { get; set; }
    }

    public class EditScreens
    {
        public List<ScreensField> Fields { get; set; }
    }

    public class SearchScreens
    {
        public List<ScreensField> Fields { get; set; }
    }
}
