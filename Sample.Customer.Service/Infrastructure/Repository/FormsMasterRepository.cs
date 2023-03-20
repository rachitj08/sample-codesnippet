using AutoMapper;
using Common.Model;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using DataSources = Sample.Customer.Service.Infrastructure.DataModels.DataSources;
using ValidationTypes = Sample.Customer.Service.Infrastructure.DataModels.ValidationTypes;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class FormsMasterRepository : RepositoryBase<FormsMaster.DataSourceFields.Model.DataSourceFields>, IFormsMasterRepository
    {
        private Dictionary<string, object> dataSourceResult = new Dictionary<string, object>();
        private readonly string accountSchema;
        public FormsMasterRepository(CloudAcceleratorContext context, IMapper mapper) : base(context)
        {
            accountSchema = base.context.schemaId;
        }
        public async Task<ScreenControlsVM> GetScreenControls(string screen, string datasource, long accountId)
        {

            Task<dynamic> searchTask = null;
            Task<IEnumerable<ScreenControlValidationVM>> validationTask = null;
            IEnumerable<ScreenControlValidationVM> listValidationData = null;

            //string dataSchemaName = string.Empty;
            //if (!string.IsNullOrWhiteSpace(datasource) && (datasource == "Screens" || datasource == "ValidationTypes" || datasource == "ControlType"))
            //    dataSchemaName = "formsmaster";
            //else
            //    dataSchemaName = "iam";

            //For Search
            if (!string.IsNullOrWhiteSpace(screen) && !string.IsNullOrWhiteSpace(datasource) && screen.ToLower().Trim() == "list")
            {
                var querySearch = @$"
                SELECT 
                    DatasourceId AS ""DatasourceId"", DataSourceName AS ""DataSourceName"", Description AS ""Description"", 
                    CanAdd AS ""CanAdd"", CanEdit AS ""CanEdit"", CanDelete AS ""CanDelete"", CanSearch AS ""CanSearch"", 
                    ListScreenTitle AS ""ListScreenTitle"",
                    AddScreenTitle AS ""AddScreenTitle"", EditScreenTitle AS ""EditScreenTitle"", AccountId AS ""AccountId"",
                    ScreenType AS ""ScreenType"", FieldId AS ""FieldId"", FieldName AS ""FieldName"", ValidationType AS ""ValidationType"",
                    ControlType AS ""ControlType"", ControlTypeSearch AS ""ControlTypeSearch"", Label AS ""Label"", 
                    ShortLabel AS ""ShortLabel"", Required AS ""Required"", DefaultValue AS ""DefaultValue"", IsPrimaryKey AS ""IsPrimaryKey"", IsIdentity AS ""IsIdentity"",
                    HelpText AS ""HelpText"", MaxLength AS ""MaxLength"", Multiline AS ""Multiline"", MinValue AS ""MinValue"", MaxValue AS ""MaxValue"",
                    ListDataSourceName AS ""ListDataSourceName"", ListValueField AS ""ListValueField"", ListTextField AS ""ListTextField"",
                    UseShortLabel AS ""UseShortLabel"", DisplayOrder AS ""DisplayOrder"", DataSourceDisplayType AS ""DataSourceDisplayType"",
                    DisplayMaxLength AS ""DisplayMaxLength"", ColumnWidthPer AS ""ColumnWidthPer"", Hidden AS ""Hidden"", ReadOnly AS ""ReadOnly""
                    FROM
                    {accountSchema}.fn_getscreencontrols_search(
	                'ListSearch', '{datasource.ToLower().Trim()}', {accountId})";
                searchTask = ExecuteQuery<SearchFields>(querySearch);
            }

            //For Validation
            if (!string.IsNullOrWhiteSpace(screen) && (screen.ToLower().Trim() == "add" || screen.ToLower().Trim() == "edit" || screen.ToLower().Trim() == "list"))
            {
                validationTask = GetDataSourceValidationData(datasource);
            }

            //For Creating Screen Controls
            string query = "select * from "+ accountSchema + ".fn_getscreencontrols('" + screen + "','" + datasource + "'," + accountId + ")";
            
            //var result = await base.context.ScreenControls.FromSqlRaw(query).ToListAsync();
            var result = await GetListDataFromQuery<FormsMasterScreenControlsModel>(query);

            List<Fields> listFields = new List<Fields>();
            List<SearchFields> listSearchFields = new List<SearchFields>();

            if (result != null && result.Count > 0)
            {
                if (validationTask != null)
                    listValidationData = await validationTask;

                foreach (FormsMasterScreenControlsModel item in result)
                {
                    #region For Fetching Master Data
                    if ((screen.ToLower().Trim() == "add" || screen.ToLower().Trim() == "edit") && !string.IsNullOrEmpty(item.ListDataSourceName))
                    {
                        StringBuilder sbDataSource = new StringBuilder();// For Add and Edit Screen, to get referncial table data
                        sbDataSource.AppendLine($@"Select");
                        sbDataSource.AppendLine($@" ""{item.ListDataSourceName}"".""{item.ListValueField}"" as Id,");
                        sbDataSource.AppendLine($@" ""{item.ListDataSourceName}"".""{item.ListTextField}"" as Name");
                        sbDataSource.AppendLine($@" from ""{accountSchema}"".""{item.ListDataSourceName}""");
                        GetDataSourceData(sbDataSource.ToString(), item.ListDataSourceName);
                    }
                    #endregion

                    #region Controls Building
                    if (screen.ToLower().Trim() == "view")
                    {
                        listFields.Add(new Fields()
                        {
                            FieldId = item.FieldId,
                            FieldName = GetDataSourceDisplayType(Convert.ToString(item.DatasourceDisplayType), item.ListValueField, item.ListTextField, item.FieldName),
                            ValidationType = item.ValidationType,
                            ControlType = item.ControlType,
                            Label = item.Label,
                            ShortLabel = item.ShortLabel,
                            Required = item.Required,
                            DefaultValue = item.DefaultValue,
                            IsPrimaryKey = item.IsPrimaryKey,
                            IsIdentity = item.IsIdentity,
                            HelpText = item.HelpText,
                            MaxLength = item.MaxLength,
                            Multiline = item.Multiline,
                            MinValue = item.MinValue,
                            MaxValue = item.MaxValue,
                            ListDataSourceName = item.ListDataSourceName,
                            ListValueField = item.ListValueField,
                            ListTextField = item.ListTextField,
                            UseShortLabel = item.UseShortLabel,
                            DisplayOrder = item.DisplayOrder,
                            DatasourceDisplayType = item.DatasourceDisplayType,
                            DisplayMaxLength = item.DisplayMaxLength,
                            ColumnWidthPer = item.ColumnWidthPer,
                            Hidden = item.Hidden,
                            ReadOnly = item.ReadOnly,
                            DataSource = (!string.IsNullOrEmpty(item.ListDataSourceName) && dataSourceResult.ContainsKey(item.ListDataSourceName)) ? dataSourceResult[item.ListDataSourceName] : null,
                            Validations = null,
                        });
                    }
                    else if (screen.ToLower().Trim() == "list")
                    {
                        listFields.Add(new Fields()
                        {
                            FieldId = item.FieldId,
                            FieldName = GetDataSourceDisplayType(Convert.ToString(item.DatasourceDisplayType), item.ListValueField, item.ListTextField, item.FieldName),
                            ValidationType = item.ValidationType,
                            ControlType = item.ControlType,
                            Label = item.Label,
                            ShortLabel = item.ShortLabel,
                            Required = item.Required,
                            DefaultValue = item.DefaultValue,
                            IsPrimaryKey = item.IsPrimaryKey,
                            IsIdentity = item.IsIdentity,
                            HelpText = item.HelpText,
                            MaxLength = item.MaxLength,
                            Multiline = item.Multiline,
                            MinValue = item.MinValue,
                            MaxValue = item.MaxValue,
                            ListDataSourceName = item.ListDataSourceName,
                            ListValueField = item.ListValueField,
                            ListTextField = item.ListTextField,
                            UseShortLabel = item.UseShortLabel,
                            DisplayOrder = item.DisplayOrder,
                            DatasourceDisplayType = item.DatasourceDisplayType,
                            DisplayMaxLength = item.DisplayMaxLength,
                            ColumnWidthPer = item.ColumnWidthPer,
                            Hidden = item.Hidden,
                            ReadOnly = item.ReadOnly,
                            DataSource = (!string.IsNullOrEmpty(item.ListDataSourceName) && dataSourceResult.ContainsKey(item.ListDataSourceName)) ? dataSourceResult[item.ListDataSourceName] : null,
                            Validations = null,
                        });
                    }
                    else
                    {
                        listFields.Add(new Fields()
                        {
                            FieldId = item.FieldId,
                            FieldName = item.FieldName,
                            ValidationType = item.ValidationType,
                            ControlType = item.ControlType,
                            Label = item.Label,
                            ShortLabel = item.ShortLabel,
                            Required = item.Required,
                            DefaultValue = item.DefaultValue,
                            IsPrimaryKey = item.IsPrimaryKey,
                            IsIdentity = item.IsIdentity,
                            HelpText = item.HelpText,
                            MaxLength = item.MaxLength,
                            Multiline = item.Multiline,
                            MinValue = item.MinValue,
                            MaxValue = item.MaxValue,
                            ListDataSourceName = item.ListDataSourceName,
                            ListValueField = item.ListValueField,
                            ListTextField = item.ListTextField,
                            UseShortLabel = item.UseShortLabel,
                            DisplayOrder = item.DisplayOrder,
                            DatasourceDisplayType = item.DatasourceDisplayType,
                            DisplayMaxLength = item.DisplayMaxLength,
                            ColumnWidthPer = item.ColumnWidthPer,
                            Hidden = item.Hidden,
                            ReadOnly = item.ReadOnly,
                            DataSource = (!string.IsNullOrEmpty(item.ListDataSourceName) && dataSourceResult.ContainsKey(item.ListDataSourceName)) ? dataSourceResult[item.ListDataSourceName] : null,
                            Validations = listValidationData != null ? listValidationData.Where(x => x.FieldId == item.FieldId).ToList() : null,
                        });
                    }
                    #endregion
                }

                #region Building Search
                if (searchTask != null && !searchTask.IsCompleted)
                {
                    int maxRetry = 3;
                    int retry = 0;
                    while (retry < maxRetry && !searchTask.IsCompleted)
                    {
                        maxRetry++;
                        searchTask.Wait(10);
                    }
                }

                if (searchTask != null && searchTask.IsCompleted)
                {
                    var searchResult = await searchTask;
                    foreach (SearchFields item in searchResult)
                    {
                        if (screen.ToLower().Trim() == "list")
                        {
                            SearchFields objSearchFields = new SearchFields();
                            objSearchFields.FieldId = item.FieldId;
                            objSearchFields.FieldName = GetDataSourceDisplayType(Convert.ToString(item.DatasourceDisplayType), item.ListValueField, item.ListTextField, item.FieldName);
                            objSearchFields.ValidationType = item.ValidationType;
                            objSearchFields.ControlType = item.ControlType;
                            objSearchFields.Label = item.Label;
                            objSearchFields.ShortLabel = item.ShortLabel;
                            objSearchFields.DefaultValue = item.DefaultValue;
                            objSearchFields.HelpText = item.HelpText;
                            objSearchFields.MaxLength = item.MaxLength;
                            objSearchFields.Multiline = item.Multiline;
                            objSearchFields.MinValue = item.MinValue;
                            objSearchFields.MaxValue = item.MaxValue;
                            objSearchFields.ListDataSourceName = item.ListDataSourceName;
                            objSearchFields.ListValueField = item.ListValueField;
                            objSearchFields.ListTextField = item.ListTextField;
                            objSearchFields.UseShortLabel = item.UseShortLabel;
                            objSearchFields.DisplayOrder = item.DisplayOrder;
                            objSearchFields.DatasourceDisplayType = item.DatasourceDisplayType;
                            objSearchFields.DisplayMaxLength = item.DisplayMaxLength;
                            objSearchFields.ColumnWidthPer = item.ColumnWidthPer;
                            objSearchFields.Required = item.Required;
                            objSearchFields.IsPrimaryKey = item.IsPrimaryKey;
                            objSearchFields.IsIdentity = item.IsIdentity;
                            objSearchFields.Hidden = item.Hidden;
                            objSearchFields.ReadOnly = item.ReadOnly;
                            if (!string.IsNullOrEmpty(item.ListDataSourceName))
                            {
                                StringBuilder sbDataSource = new StringBuilder();// For Add and Edit Screen, to get referncial table data
                                                                                 //Prepare Query for Datasource
                                sbDataSource.AppendLine($@"Select");
                                sbDataSource.AppendLine($@" ""{item.ListDataSourceName}"".""{item.ListValueField}"" as Id,");
                                sbDataSource.AppendLine($@" ""{item.ListDataSourceName}"".""{item.ListTextField}"" as Name");
                                sbDataSource.AppendLine($@" from ""{accountSchema}"".""{item.ListDataSourceName}""");

                                //Send Prepared Query for execution
                                GetDataSourceData(sbDataSource.ToString(), item.ListDataSourceName);
                            }
                            objSearchFields.DataSource = (!string.IsNullOrEmpty(item.ListDataSourceName) && dataSourceResult.ContainsKey(item.ListDataSourceName)) ? dataSourceResult[item.ListDataSourceName] : null;
                            objSearchFields.Validations = listValidationData != null ? listValidationData.Where(x => x.FieldId == item.FieldId).ToList() : null;
                            listSearchFields.Add(objSearchFields);
                        }
                    }
                }
                #endregion

            }

            var resultNew = result.Select(x => new ScreenControlsVM()
            {
                DataSourceId = x.DataSourceId,
                DataSourceName = x.DataSourceName,
                Description = x.Description,

                CanAdd = x.CanAdd,
                CanEdit = x.CanEdit,
                CanDelete = x.CanDelete,
                CanSearch = x.CanSearch,

                ListScreenTitle = x.ListScreenTitle,
                AddScreenTitle = x.AddScreenTitle,
                EditScreenTitle = x.EditScreenTitle,
                AccountId = x.AccountId,
                ScreenType = x.ScreenType,
                Fields = listFields,
                SearchFields = listSearchFields
            }).FirstOrDefault();

            return resultNew;
        }

       
        public async Task<ControlsDataVM> GetScreenControlsData(string screen, string datasource, long accId, long? dataId)
        {

            datasource = char.ToUpper(datasource[0]) + datasource.Substring(1);
            ControlsDataVM result = new ControlsDataVM();

            //*-------Common Query Start-------*
            var queryDataSourceFields = @$"

            SELECT 
		    ds.""DataSourceId"", ds.""DataSourceName"", ds.""Description"", ds.""AccountId"", s.""ScreenName"" as ScreenType,
            dsf.""FieldId"", dsf.""FieldName"", dsf.""ValidationType"",dsf.""IsPrimaryKey"", dsf.""IsIdentity"",
		    CASE
                WHEN s.""ScreenName"" = 'List' THEN dsf.""ControlTypeForList""
                WHEN s.""ScreenName"" = 'View' THEN dsf.""ControlTypeForView""
                ELSE dsf.""ControlTypeForAdd""
            END as ""ControlType"",
		    dsf.""ListDataSourceName"",
		    dsf.""ListValueField"",
		    dsf.""ListTextField"",
            sf.""DatasourceDisplayType""

            FROM ""{accountSchema}"".""DataSources"" AS ds

            JOIN ""{accountSchema}"".""DataSourceFields"" dsf ON ds.""DataSourceName"" = dsf.""DataSourceName""

            JOIN ""{accountSchema}"".""ScreenFields"" sf ON dsf.""FieldId"" = sf.""FieldId""

            JOIN ""{accountSchema}"".""Screens"" s ON s.""ScreenId"" = sf.""ScreenId""

            WHERE lower(ds.""DataSourceName"") = '{datasource.ToLower()}' AND lower(s.""ScreenName"") = '{screen.ToLower()}' ORDER BY sf.""DisplayOrder"";
            ";

            //*-------Common Query End-------*

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = queryDataSourceFields;
                context.Database.OpenConnection();
                using (var reader_DataSourceFields = await command.ExecuteReaderAsync())
                {
                    // Create to Child Method for Add and List
                    switch (screen.ToLower())
                    {
                        case "list":
                            result = await GetListScreenData(reader_DataSourceFields, command, datasource);
                            break;
                        case "add":
                            result = await GetAddScreenData(reader_DataSourceFields, command, datasource);
                            break;
                        case "edit":
                            result = await GetEditScreenData(reader_DataSourceFields, command, datasource, dataId);
                            break;
                        case "view":
                            result = await GetViewScreenData(reader_DataSourceFields, command, datasource, dataId);
                            break;
                        default:
                            break;
                    }
                }
            }

            return result;
        }

        //For List Screen
        public Task<ControlsDataVM> GetListScreenData(DbDataReader reader_DataSourceFields, DbCommand command,  string datasource)
        {
            StringBuilder sbJoins = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            ControlsDataVM result = new ControlsDataVM();

            while (reader_DataSourceFields.Read())
            {
                if (String.IsNullOrEmpty(reader_DataSourceFields["ListDataSourceName"].ToString()))
                {
                    if (Convert.ToBoolean(reader_DataSourceFields["IsIdentity"]))
                        sbColumns.AppendLine("\"" + reader_DataSourceFields["FieldName"].ToString() + "\" as Id,");

                    sbColumns.AppendLine("\"" + reader_DataSourceFields["FieldName"].ToString() + "\",");
                }
                else
                {
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["DataSourceName"]}"".""{reader_DataSourceFields["FieldName"]}"",");
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListValueField"]}"",");
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListTextField"]}"",");
                    sbJoins.AppendLine($@" INNER JOIN ""{accountSchema}"".""{reader_DataSourceFields["ListDataSourceName"]}"" ON ");
                    sbJoins.AppendLine($@" ""{reader_DataSourceFields["DataSourceName"]}"".""{reader_DataSourceFields["FieldName"]}""=""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListValueField"]}""");
                }

                if (result.Account_Id == null)
                {
                    result.Account_Id = reader_DataSourceFields["AccountId"].ToString();
                    result.DataSourceId = Convert.ToInt32(reader_DataSourceFields["DataSourceId"]);
                    result.DataSourceName = reader_DataSourceFields["DataSourceName"].ToString();
                    result.ScreenType = reader_DataSourceFields["ScreenType"].ToString();
                    result.Description = reader_DataSourceFields["Description"].ToString();
                }
            }

            reader_DataSourceFields.Close();

            var query_ListScreen_Flat = "Select ";

            if (sbColumns.ToString().LastIndexOf(",") > -1)
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns.ToString().Substring(0, sbColumns.ToString().LastIndexOf(",")) + " From " + accountSchema + ".\"" + datasource + "\"";
            else
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns + " From " + accountSchema + ".\"" + datasource + "\"";
            query_ListScreen_Flat = query_ListScreen_Flat + sbJoins.ToString();

            dynamic dataTask = GetFlatData(query_ListScreen_Flat);
            result.Rows1 = dataTask;

            return Task.FromResult(result);
        }

        //For List Search Screen
        public Task<ControlsDataVM> GetListSearchScreenData(DbDataReader reader_DataSourceFields, DbCommand command, string datasource, ControlsDataResponseVM obj)
        {

            StringBuilder sbJoins = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            ControlsDataVM result = new ControlsDataVM();
            while (reader_DataSourceFields.Read())
            {
                if (String.IsNullOrEmpty(reader_DataSourceFields["ListDataSourceName"].ToString()))
                {
                    if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("IsIdentity")) && Convert.ToBoolean(reader_DataSourceFields["IsIdentity"]))
                    {
                        sbColumns.AppendLine("\"" + reader_DataSourceFields["FieldName"].ToString() + "\" as Id,");
                    }
                    sbColumns.AppendLine("\"" + reader_DataSourceFields["FieldName"].ToString() + "\",");
                }
                else
                {
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["DataSourceName"]}"".""{reader_DataSourceFields["FieldName"]}"",");
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListValueField"]}"",");
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListTextField"]}"",");
                    sbJoins.AppendLine($@" INNER JOIN ""{accountSchema}"".""{reader_DataSourceFields["ListDataSourceName"]}"" ON ");
                    sbJoins.AppendLine($@" ""{reader_DataSourceFields["DataSourceName"]}"".""{reader_DataSourceFields["FieldName"]}""=""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListValueField"]}""");
                }

                if (result.Account_Id == null)
                {
                    result.Account_Id = reader_DataSourceFields["AccountId"].ToString();
                    result.DataSourceId = Convert.ToInt32(reader_DataSourceFields["DataSourceId"]);
                    result.DataSourceName = reader_DataSourceFields["DataSourceName"].ToString();
                    result.ScreenType = reader_DataSourceFields["ScreenType"].ToString();
                    result.Description = reader_DataSourceFields["Description"].ToString();
                }
            }

            reader_DataSourceFields.Close();

            StringBuilder sbWhere = new StringBuilder();
            foreach (var item in obj.Data)
            {
                int counter = 0;
                sbWhere.AppendLine($@" Where");
                var index = 0;
                foreach (var itemData in item)
                {
                    if(itemData.Value != null && !string.IsNullOrWhiteSpace(itemData.Value.ToString()))
                    {
                        if (counter == 0)
                        {
                            if (itemData.Value != null)
                                sbWhere.AppendLine($@" ""{datasource}"".""{itemData.Key}"" = '{itemData.Value}' ");
                        }
                        else
                        {
                            if (itemData.Value != null)
                                sbWhere.AppendLine($@" AND ""{datasource}"".""{itemData.Key}"" = '{itemData.Value}' ");
                        }
                        counter++;
                    }
                   
                    index++;
                }
            }

            var query_ListScreen_Flat = "Select ";

            if (sbColumns.ToString().LastIndexOf(",") > -1)
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns.ToString().Substring(0, sbColumns.ToString().LastIndexOf(",")) + " From " + accountSchema + ".\"" + datasource + "\"";
            else
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns + " From " + accountSchema + ".\"" + datasource + "\"";

            query_ListScreen_Flat = query_ListScreen_Flat + sbJoins.ToString() + sbWhere.ToString();

            dynamic dataTask = GetFlatData(query_ListScreen_Flat);
            result.Rows1 = dataTask;

            return Task.FromResult(result);
        }

        //For Add Screen
        public Task<ControlsDataVM> GetAddScreenData(DbDataReader reader_DataSourceFields, DbCommand command, string datasource)
        {
            ControlsDataVM result = new ControlsDataVM();
            List<int> listFieldIds = new List<int>();

            while (reader_DataSourceFields.Read())
            {
                //for making array of field Ids
                listFieldIds.Add((reader_DataSourceFields.GetInt16("FieldId")));

                if (String.IsNullOrEmpty(reader_DataSourceFields["ListDataSourceName"].ToString()))
                {
                    string ValidatonType = reader_DataSourceFields["ValidationType"].ToString();

                    if (!string.IsNullOrWhiteSpace(ValidatonType))
                    {
                        switch (ValidatonType)
                        {
                            case "Text":
                                dataSourceResult.TryAdd(reader_DataSourceFields["FieldName"].ToString(), "");
                                break;
                            case "WholeNumber":
                                dataSourceResult.TryAdd(reader_DataSourceFields["FieldName"].ToString(), 0);
                                break;
                            case "Decimal":
                                dataSourceResult.TryAdd(reader_DataSourceFields["FieldName"].ToString(), 0);
                                break;
                            default:
                                dataSourceResult.TryAdd(reader_DataSourceFields["FieldName"].ToString(), "");
                                break;
                        }
                    }
                    else
                        dataSourceResult.TryAdd(reader_DataSourceFields["FieldName"].ToString(), "");
                }
                else
                {
                    dataSourceResult.TryAdd(reader_DataSourceFields["FieldName"].ToString(), "");

                }

                if (result.Account_Id == null)
                {
                    result.Account_Id = reader_DataSourceFields["AccountId"].ToString();
                    result.DataSourceId = Convert.ToInt32(reader_DataSourceFields["DataSourceId"]);
                    result.DataSourceName = reader_DataSourceFields["DataSourceName"].ToString();
                    result.ScreenType = reader_DataSourceFields["ScreenType"].ToString();
                    result.Description = reader_DataSourceFields["Description"].ToString();
                }
            }

            reader_DataSourceFields.Close();

            //Foreach for Task Completion or not otherwise wait
            Dictionary<string, object> dataSet = new Dictionary<string, object>();

            foreach (var entry in dataSourceResult)
            {
                var resultSet = dataSourceResult[entry.Key];
                dataSet.Add(entry.Key, resultSet);
            }
            result.Rows1.Add(dataSet);

            return Task.FromResult(result);
        }

        //For Edit Screen
        public Task<ControlsDataVM> GetEditScreenData(DbDataReader reader_DataSourceFields, DbCommand command,  string datasource, long? dataId)
        {
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            ControlsDataVM result = new ControlsDataVM();
            List<Task> threadPool = new List<Task>();
            List<int> listFieldIds = new List<int>();

            while (reader_DataSourceFields.Read())
            {
                //for making array of field Ids
                listFieldIds.Add((reader_DataSourceFields.GetInt16("FieldId")));

                if (reader_DataSourceFields.GetBoolean("IsIdentity") && dataId != null)
                    sbWhere.AppendLine($@" Where ""{reader_DataSourceFields.GetString("FieldName")}"" = {dataId} ");

                if (String.IsNullOrEmpty(reader_DataSourceFields["ListDataSourceName"].ToString()))
                {
                    sbColumns.AppendLine("\"" + reader_DataSourceFields["FieldName"].ToString() + "\",");
                }
                else
                {
                    sbColumns.AppendLine($@" ""{reader_DataSourceFields["DataSourceName"]}"".""{reader_DataSourceFields["FieldName"]}"",");
                }

                if (result.Account_Id == null)
                {
                    result.Account_Id = reader_DataSourceFields["AccountId"].ToString();
                    result.DataSourceId = Convert.ToInt32(reader_DataSourceFields["DataSourceId"]);
                    result.DataSourceName = reader_DataSourceFields["DataSourceName"].ToString();
                    result.ScreenType = reader_DataSourceFields["ScreenType"].ToString();
                    result.Description = reader_DataSourceFields["Description"].ToString();
                }
            }

            reader_DataSourceFields.Close();

            var query_ListScreen_Flat = "Select ";

            if (sbColumns.ToString().LastIndexOf(",") > -1)
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns.ToString().Substring(0, sbColumns.ToString().LastIndexOf(",")) + " From " + accountSchema + ".\"" + datasource + "\"";
            else
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns + " From " + accountSchema + ".\"" + datasource + "\"";

            query_ListScreen_Flat += sbWhere.ToString();

            List<Dictionary<string, object>> rowData = GetFlatData(query_ListScreen_Flat);

            foreach (var entry in dataSourceResult)
            {
                var resultSet = dataSourceResult[entry.Key];
                rowData[0].Add(entry.Key, resultSet);
            }

            result.Rows1 = rowData;

            return Task.FromResult(result);
        }

        //For View Screen
        public Task<ControlsDataVM> GetViewScreenData(DbDataReader reader_DataSourceFields, DbCommand command, string datasource, long? dataId)
        {
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbJoins = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            ControlsDataVM result = new ControlsDataVM();

            while (reader_DataSourceFields.Read())
            {
                if (reader_DataSourceFields.GetBoolean("IsIdentity") && dataId != null)
                    sbWhere.AppendLine($@" Where ""{reader_DataSourceFields.GetString("FieldName")}"" = {dataId} ");

                if (String.IsNullOrEmpty(reader_DataSourceFields["ListDataSourceName"].ToString()))
                {
                    sbColumns.AppendLine("\"" + reader_DataSourceFields["FieldName"].ToString() + "\",");
                }
                else
                {
                    if (!string.IsNullOrEmpty(reader_DataSourceFields["DataSourceDisplayType"].ToString()))
                    {
                        switch (reader_DataSourceFields["DataSourceDisplayType"].ToString())
                        {
                            case "1":
                                sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListValueField"]}"",");
                                break;
                            case "2":
                                sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListTextField"]}"",");
                                break;
                            case "3":
                                sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListTextField"]}"",");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        sbColumns.AppendLine($@" ""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListTextField"]}"",");
                    }
                    sbJoins.AppendLine($@" INNER JOIN ""{accountSchema}"".""{reader_DataSourceFields["ListDataSourceName"]}"" ON ");
                    sbJoins.AppendLine($@" ""{reader_DataSourceFields["DataSourceName"]}"".""{reader_DataSourceFields["FieldName"]}""=""{reader_DataSourceFields["ListDataSourceName"]}"".""{reader_DataSourceFields["ListValueField"]}""");
                }

                if (result.Account_Id == null)
                {
                    result.Account_Id = reader_DataSourceFields["AccountId"].ToString();
                    result.DataSourceId = Convert.ToInt32(reader_DataSourceFields["DataSourceId"]);
                    result.DataSourceName = reader_DataSourceFields["DataSourceName"].ToString();
                    result.ScreenType = reader_DataSourceFields["ScreenType"].ToString();
                    result.Description = reader_DataSourceFields["Description"].ToString();
                }
            }

            reader_DataSourceFields.Close();

            var query_ListScreen_Flat = "Select ";

            if (sbColumns.ToString().LastIndexOf(",") > -1)
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns.ToString().Substring(0, sbColumns.ToString().LastIndexOf(",")) + " From " + accountSchema + ".\"" + datasource + "\"";
            else
                query_ListScreen_Flat = query_ListScreen_Flat + sbColumns + " From " + accountSchema + ".\"" + datasource + "\"";

            query_ListScreen_Flat += sbJoins.ToString() + sbWhere.ToString();

            List<Dictionary<string, object>> rowData = GetFlatData(query_ListScreen_Flat);

            result.Rows1 = rowData;

            return Task.FromResult(result);
        }

        //Execute Async Query
        public void GetDataSourceData(string rawSqlQuery, string columnName)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var result = connection.Query<dynamic>(rawSqlQuery);
                dataSourceResult.TryAdd(columnName, result);
            }
        }
        public async Task<dynamic> ExecuteQuery<T>(string rawSqlQuery)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                return connection.Query<T>(rawSqlQuery).ToList();
            }
        }
        public async Task<List<T>> GetListDataFromQuery<T>(string rawSqlQuery)
        { 
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var result = await connection.QueryAsync<T>(rawSqlQuery);
                return result.ToList();
            }
        }

        public Task<IEnumerable<ScreenControlValidationVM>> GetDataSourceValidationData(string dataSource)
        {
            StringBuilder sbValidationQuery = new StringBuilder();

            sbValidationQuery.AppendLine($@"Select ""DataSourceFields"".""FieldId"", ""ValidationTypes"".""ValidationType"" as name, ""ValidationTypes"".""ValidationType"" as validator, ""ValidationTypes"".""ErrorMessage"" as message from ""{accountSchema}"".""DataSourceFieldValidation""");
            sbValidationQuery.AppendLine($@" Inner Join ""{accountSchema}"".""DataSourceFields"" on ""DataSourceFieldValidation"".""FieldId"" = ""DataSourceFields"".""FieldId""");
            sbValidationQuery.AppendLine($@" Inner Join ""{accountSchema}"".""ValidationTypes"" on ""DataSourceFieldValidation"".""ValidationTypeId"" = ""ValidationTypes"".""ValidationTypeId""");

            sbValidationQuery.AppendLine($@" where lower(""DataSourceFields"".""DataSourceName"") = '{dataSource.ToLower()}'");

            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var result = connection.Query<ScreenControlValidationVM>(sbValidationQuery.ToString());
                return Task.FromResult(result);
            }
        }

        //For populating controls data
        public List<Dictionary<string, object>> GetFlatData(string sqlQuery)
        {
            //Console.WriteLine(sqlQuery);
            ControlsDataVM listObj1 = new ControlsDataVM();
            Dictionary<string, object> childRow;
            try
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())//row
                        {
                            childRow = new Dictionary<string, object>();
                            RowItem row = null;
                            for (int i = 0; i < reader.FieldCount; i++)//col
                            {
                                row = new RowItem();
                                if (!childRow.ContainsKey(reader.GetName(i)))
                                    childRow.Add(reader.GetName(i), reader.GetValue(i));
                            }

                            listObj1.Rows1.Add(childRow);
                        }
                        reader.Close();
                    }
                }
            }
            catch(Exception)
            {

            }
            return listObj1.Rows1;
        }

        public List<Dictionary<string, object>> GetFlatData_Backup(string sqlQuery, Dictionary<string, FieldCollection> screenFieldCollection)
        {
            //Console.WriteLine(sqlQuery);
            ControlsDataVM listObj1 = new ControlsDataVM();
            Dictionary<string, object> childRow;
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())//row
                    {
                        childRow = new Dictionary<string, object>();
                        RowItem row = null;
                        List<RowItem> listRows = null;

                        for (int i = 0; i < reader.FieldCount; i++)//col
                        {
                            listRows = new List<RowItem>();
                            row = new RowItem();
                            if (!childRow.ContainsKey(reader.GetName(i)))
                                childRow.Add(reader.GetName(i), reader.GetValue(i));
                            row.FieldName = reader.GetName(i);
                            row.FieldValue = reader.GetValue(i);
                            if (screenFieldCollection.ContainsKey(reader.GetName(i)))
                            {
                                row.ControlType = screenFieldCollection[reader.GetName(i)].ControlType;
                                row.IsPrimaryKeyld = screenFieldCollection[reader.GetName(i)].IsPrimaryKey;
                            }
                            else
                            {
                                row.ControlType = string.Empty;
                                row.IsPrimaryKeyld = false;
                            }

                            listRows.Add(row);
                        }
                        listObj1.Rows1.Add(childRow);
                    }

                    reader.Close();

                }

            }
            return listObj1.Rows1;
        }

        public async Task<ControlsDataVM> GetScreenControlsListSearchData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            var datasource = char.ToUpper(objControlsDataResponseVM.DataSourceName[0]) + objControlsDataResponseVM.DataSourceName.Substring(1);
            ControlsDataVM result = null;

            //*-------Common Query Start-------*
            var queryDataSourceFields = @$"

            SELECT 
		    ds.""DataSourceId"", ds.""DataSourceName"", ds.""Description"", ds.""AccountId"", s.""ScreenName"" as ScreenType,
            dsf.""FieldId"", dsf.""FieldName"",dsf.""IsPrimaryKey"",dsf.""IsIdentity"",
		    CASE
                WHEN s.""ScreenName"" = 'List' THEN dsf.""ControlTypeForList""
                WHEN s.""ScreenName"" = 'View' THEN dsf.""ControlTypeForView""
                ELSE dsf.""ControlTypeForAdd""
            END as ""ControlType"",
		    dsf.""ListDataSourceName"",
		    dsf.""ListValueField"",
		    dsf.""ListTextField"",
            sf.""DatasourceDisplayType""

            FROM ""{accountSchema}"".""DataSources"" AS ds

            JOIN ""{accountSchema}"".""DataSourceFields"" dsf ON ds.""DataSourceName"" = dsf.""DataSourceName""

            JOIN ""{accountSchema}"".""ScreenFields"" sf ON dsf.""FieldId"" = sf.""FieldId""

            JOIN ""{accountSchema}"".""Screens"" s ON s.""ScreenId"" = sf.""ScreenId""

            WHERE lower(ds.""DataSourceName"") = '{datasource.ToLower()}' AND lower(s.""ScreenName"") = '{objControlsDataResponseVM.ScreenType.ToLower()}' ORDER BY sf.""DisplayOrder"";
            ";
            //*-------Common Query End-------*

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = queryDataSourceFields;
                context.Database.OpenConnection();
                using (var reader_DataSourceFields = await command.ExecuteReaderAsync())
                {
                    result = await GetListSearchScreenData(reader_DataSourceFields, command, datasource, objControlsDataResponseVM);
                }
            }

            return result;
        }

        public async Task<ResponseResult> InsertUpdateFormData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            if (objControlsDataResponseVM == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Input Json is null"
                    }
                };
            }
            else
            {
                objControlsDataResponseVM.SourceSchema = accountSchema;
                string sqlQuery = @"call " + accountSchema + ".SP_InsertUpdateFormData('" + JsonConvert.SerializeObject(objControlsDataResponseVM) + "')";
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        context.Database.OpenConnection();
                        var result = await command.ExecuteNonQueryAsync();

                        return new ResponseResult()
                        {
                            Message = ResponseMessage.RecordSaved,
                            ResponseCode = ResponseCode.RecordSaved,
                            Data = "Data Saved Successfully"
                        };

                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult()
                        {
                            Message = ex.Message,
                            ResponseCode = ResponseCode.InternalServerError
                        };
                    }


                }
            }
        }

        public async Task<ResponseResult> DeleteFormData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            if (objControlsDataResponseVM == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Input Json is null"
                    }
                };
            }
            else
            {
                objControlsDataResponseVM.SourceSchema = accountSchema;
                string sqlQuery = @"call " + accountSchema + ".sp_deleteformdata('" + JsonConvert.SerializeObject(objControlsDataResponseVM) + "')";
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        context.Database.OpenConnection();
                        await command.ExecuteNonQueryAsync();
                        return new ResponseResult()
                        {
                            Message = ResponseMessage.RecordDeleted,
                            ResponseCode = ResponseCode.RecordDeleted,
                            Data = "Record Deleted sucessfully"
                        };

                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult()
                        {
                            Message = ex.Message,
                            ResponseCode = ResponseCode.InternalServerError
                        };
                    }


                }
            }
        }

        public async Task<ResponseResult> SaveFormMasterConfigurationData(FormsMasterConfiguration objFormsMasterConfiguration)
        {
            if (objFormsMasterConfiguration == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Input Json is null"
                    }
                };
            }
            else
            {
                string sqlQuery = @"call  " + accountSchema + ".sp_SaveFormMasterConfigurationData('" + JsonConvert.SerializeObject(objFormsMasterConfiguration) + "')";
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        context.Database.OpenConnection();
                        await command.ExecuteNonQueryAsync();
                        return new ResponseResult()
                        {
                            Message = ResponseMessage.RecordSaved,
                            ResponseCode = ResponseCode.RecordSaved,
                            Data = "Record Updated sucessfully"
                        };

                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult()
                        {
                            Message = ex.Message,
                            ResponseCode = ResponseCode.InternalServerError
                        };
                    }


                }
            }
        }
        public async Task<ResponseResult> GetFormMasterConfigurationData(int dataSourceID)
        {
            ResponseResult objResponseResult = new ResponseResult();
            FormsMasterConfiguration objFormsMasterConfiguration = new FormsMasterConfiguration();
            var dataSourceTableQuery = @"SELECT * FROM " + accountSchema + @".""DataSources"" WHERE ""DataSourceId""" + "=" + dataSourceID + ";";
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = dataSourceTableQuery;
                context.Database.OpenConnection();
                using (var reader_DataSource = await command.ExecuteReaderAsync())
                {
                    while (reader_DataSource.Read())
                    {
                        objFormsMasterConfiguration = new FormsMasterConfiguration();
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("DataSourceId")))
                            objFormsMasterConfiguration.DataSourceId = Convert.ToInt32(reader_DataSource["DataSourceId"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("DataSourceName")))
                            objFormsMasterConfiguration.DataSourceName = Convert.ToString(reader_DataSource["DataSourceName"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("Description")))
                            objFormsMasterConfiguration.Description = Convert.ToString(reader_DataSource["Description"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("AccountId")))
                            objFormsMasterConfiguration.AccountId = Convert.ToInt32(reader_DataSource["AccountId"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("CanAdd")))
                            objFormsMasterConfiguration.CanAdd = Convert.ToBoolean(reader_DataSource["CanAdd"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("CanEdit")))
                            objFormsMasterConfiguration.CanEdit = Convert.ToBoolean(reader_DataSource["CanEdit"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("CanDelete")))
                            objFormsMasterConfiguration.CanDelete = Convert.ToBoolean(reader_DataSource["CanDelete"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("CanSearch")))
                            objFormsMasterConfiguration.CanSearch = Convert.ToBoolean(reader_DataSource["CanSearch"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("ListScreenTitle")))
                            objFormsMasterConfiguration.ListScreenTitle = Convert.ToString(reader_DataSource["ListScreenTitle"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("AddScreenTitle")))
                            objFormsMasterConfiguration.AddScreenTitle = Convert.ToString(reader_DataSource["AddScreenTitle"]);
                        if (!reader_DataSource.IsDBNull(reader_DataSource.GetOrdinal("EditScreenTitle")))
                            objFormsMasterConfiguration.EditScreenTitle = Convert.ToString(reader_DataSource["EditScreenTitle"]);
                    }
                }

                List<SchemaTableFields> lstSchemaTableFields = CheckTableSchema(accountSchema, objFormsMasterConfiguration.DataSourceName);
                if (lstSchemaTableFields != null && lstSchemaTableFields.Count > 0)
                {
                    if (objFormsMasterConfiguration.DataSourceConcreteFields == null)
                        objFormsMasterConfiguration.DataSourceConcreteFields = new DataSourceConcreteFields();

                    if (objFormsMasterConfiguration.DataSourceConcreteFields.lstSchemaTableFields == null)
                        objFormsMasterConfiguration.DataSourceConcreteFields.lstSchemaTableFields = new List<SchemaTableFields>();

                    objFormsMasterConfiguration.DataSourceConcreteFields.lstSchemaTableFields = lstSchemaTableFields;

                    var queryDataSourceFields = @"SELECT * FROM " + accountSchema + @".""DataSourceFields"" WHERE ""DataSourceName""" + "=" + "'" + objFormsMasterConfiguration.DataSourceName + "';";
                    command.CommandText = queryDataSourceFields;
                    using (var reader_DataSourceFields = await command.ExecuteReaderAsync())
                    {
                        while (reader_DataSourceFields.Read())
                        {
                            if (objFormsMasterConfiguration.DataSourceFields == null)
                                objFormsMasterConfiguration.DataSourceFields = new DataSourceFieldsConf();
                            if (objFormsMasterConfiguration.DataSourceFields.Fields == null)
                                objFormsMasterConfiguration.DataSourceFields.Fields = new List<Field>();

                            Field objField = new Field();
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("FieldId")))
                                objField.FieldID = Convert.ToInt32(reader_DataSourceFields["FieldId"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("FieldName")))
                                objField.FieldName = Convert.ToString(reader_DataSourceFields["FieldName"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("Label")))
                                objField.Label = Convert.ToString(reader_DataSourceFields["Label"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ShortLabel")))
                                objField.ShortLabel = Convert.ToString(reader_DataSourceFields["ShortLabel"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("DefaultValue")))
                                objField.DefaultValue = Convert.ToString(reader_DataSourceFields["DefaultValue"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("IsPrimaryKey")))
                                objField.IsPrimaryKey = Convert.ToBoolean(reader_DataSourceFields["IsPrimaryKey"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("IsIdentity")))
                                objField.IsIdentity = Convert.ToBoolean(reader_DataSourceFields["IsIdentity"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("HelpText")))
                                objField.HelpText = Convert.ToString(reader_DataSourceFields["HelpText"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("MaxLength")))
                                objField.MaxLength = Convert.ToInt32(reader_DataSourceFields["MaxLength"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("Multiline")))
                                objField.Multiline = Convert.ToBoolean(reader_DataSourceFields["Multiline"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ListDataSourceName")))
                                objField.ListDataSourceName = Convert.ToString(reader_DataSourceFields["ListDataSourceName"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ListValueField")))
                                objField.ListValueField = Convert.ToString(reader_DataSourceFields["ListValueField"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ListTextField")))
                                objField.ListTextField = Convert.ToString(reader_DataSourceFields["ListTextField"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ControlTypeForAdd")))
                                objField.ControlTypeForAdd = Convert.ToString(reader_DataSourceFields["ControlTypeForAdd"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ControlTypeForView")))
                                objField.ControlTypeForView = Convert.ToString(reader_DataSourceFields["ControlTypeForView"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ControlTypeForList")))
                                objField.ControlTypeForList = Convert.ToString(reader_DataSourceFields["ControlTypeForList"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ControlTypeForSearch")))
                                objField.ControlTypeForSearch = Convert.ToString(reader_DataSourceFields["ControlTypeForSearch"]);
                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("ControlTypeForEdit")))
                                objField.ControlTypeForEdit = Convert.ToString(reader_DataSourceFields["ControlTypeForEdit"]);
                            objFormsMasterConfiguration.DataSourceFields.Fields.Add(objField);


                        }
                    }

                    // Schema Table Exist and record found case
                    if (objFormsMasterConfiguration != null && objFormsMasterConfiguration.DataSourceFields != null &&
                        objFormsMasterConfiguration.DataSourceFields.Fields != null && objFormsMasterConfiguration.DataSourceFields.Fields.Count > 0)
                    {
                        foreach (var items in objFormsMasterConfiguration.DataSourceFields.Fields)
                        {
                            if (items.FieldID > 0)
                            {
                                if (items.Validations == null)
                                    items.Validations = new List<Validation>();

                                string validationTableQuery = @"SELECT * FROM " + accountSchema + @".""DataSourceFieldValidation"" WHERE ""FieldId""" + "=" + items.FieldID + ";";
                                command.CommandText = validationTableQuery;
                                using (var validationDataReader = await command.ExecuteReaderAsync())
                                {
                                    while (validationDataReader.Read())
                                    {
                                        Validation objValidation = new Validation();
                                        if (!validationDataReader.IsDBNull(validationDataReader.GetOrdinal("FieldId")))
                                            objValidation.FieldId = Convert.ToInt32(validationDataReader["FieldId"]);
                                        if (!validationDataReader.IsDBNull(validationDataReader.GetOrdinal("ValidationTypeId")))
                                            objValidation.ValidationTypeId = Convert.ToString(validationDataReader["ValidationTypeId"]);
                                        if (!validationDataReader.IsDBNull(validationDataReader.GetOrdinal("ErrorMessage")))
                                            objValidation.ErrorMessage = Convert.ToString(validationDataReader["ErrorMessage"]);

                                        items.Validations.Add(objValidation);
                                    }
                                }
                            }


                            var queryScreenFields = @"SELECT * FROM " + accountSchema + @".""ScreenFields"" WHERE ""FieldId""" + "=" + items.FieldID + ";";
                            command.CommandText = queryScreenFields;
                            using (var reader_ScreenFields = await command.ExecuteReaderAsync())
                            {
                                while (reader_ScreenFields.Read())
                                {
                                    int screenID = 0;
                                    ScreensField objScreensField = new ScreensField();
                                    objScreensField.FieldName = items.FieldName;

                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("ScreenId")))
                                        screenID = Convert.ToInt32(reader_ScreenFields["ScreenId"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("UseShortLabel")))
                                        objScreensField.UseShortLabel = Convert.ToBoolean(reader_ScreenFields["UseShortLabel"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("DisplayOrder")))
                                        objScreensField.DisplayOrder = Convert.ToInt32(reader_ScreenFields["DisplayOrder"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("DatasourceDisplayType")))
                                        objScreensField.DatasourceDisplayType = Convert.ToInt32(reader_ScreenFields["DatasourceDisplayType"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("DisplayMaxLength")))
                                        objScreensField.DisplayMaxLength = Convert.ToString(reader_ScreenFields["DisplayMaxLength"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("ColumnWidthPer")))
                                        objScreensField.ColumnWidthPer = Convert.ToInt32(reader_ScreenFields["ColumnWidthPer"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("Hidden")))
                                        objScreensField.Hidden = Convert.ToBoolean(reader_ScreenFields["Hidden"]);
                                    if (!reader_ScreenFields.IsDBNull(reader_ScreenFields.GetOrdinal("ReadOnly")))
                                        objScreensField.ReadOnly = Convert.ToBoolean(reader_ScreenFields["ReadOnly"]);

                                    if (screenID == 1)
                                    {
                                        if (objFormsMasterConfiguration.listScreens == null)
                                            objFormsMasterConfiguration.listScreens = new ListScreens();

                                        if (objFormsMasterConfiguration.listScreens.Fields == null)
                                            objFormsMasterConfiguration.listScreens.Fields = new List<ScreensField>();

                                        objFormsMasterConfiguration.listScreens.Fields.Add(objScreensField);
                                    }
                                    if (screenID == 2)
                                    {
                                        if (objFormsMasterConfiguration.viewScreen == null)
                                            objFormsMasterConfiguration.viewScreen = new ViewScreen();

                                        if (objFormsMasterConfiguration.viewScreen.Fields == null)
                                            objFormsMasterConfiguration.viewScreen.Fields = new List<ScreensField>();

                                        objFormsMasterConfiguration.viewScreen.Fields.Add(objScreensField);
                                    }
                                    if (screenID == 3)
                                    {
                                        if (objFormsMasterConfiguration.addScreens == null)
                                            objFormsMasterConfiguration.addScreens = new AddScreens();

                                        if (objFormsMasterConfiguration.addScreens.Fields == null)
                                            objFormsMasterConfiguration.addScreens.Fields = new List<ScreensField>();

                                        objFormsMasterConfiguration.addScreens.Fields.Add(objScreensField);
                                    }
                                    if (screenID == 4)
                                    {
                                        if (objFormsMasterConfiguration.editScreens == null)
                                            objFormsMasterConfiguration.editScreens = new EditScreens();

                                        if (objFormsMasterConfiguration.editScreens.Fields == null)
                                            objFormsMasterConfiguration.editScreens.Fields = new List<ScreensField>();

                                        objFormsMasterConfiguration.editScreens.Fields.Add(objScreensField);
                                    }
                                    if (screenID == 5)
                                    {
                                        if (objFormsMasterConfiguration.SearchScreens == null)
                                            objFormsMasterConfiguration.SearchScreens = new SearchScreens();

                                        if (objFormsMasterConfiguration.SearchScreens.Fields == null)
                                            objFormsMasterConfiguration.SearchScreens.Fields = new List<ScreensField>();

                                        objFormsMasterConfiguration.SearchScreens.Fields.Add(objScreensField);
                                    }
                                }
                            }
                        }
                        objResponseResult = new ResponseResult()
                        {
                            Data = objFormsMasterConfiguration,
                            Message = ResponseMessage.RecordFetched,
                            ResponseCode = ResponseCode.RecordFetched,
                            Error = null,
                        };
                    }
                    else
                    {
                        // Schema Table Exist but no record found case
                        if (objFormsMasterConfiguration.DataSourceFields == null)
                            objFormsMasterConfiguration.DataSourceFields = new DataSourceFieldsConf();
                        if (objFormsMasterConfiguration.DataSourceFields.Fields == null)
                            objFormsMasterConfiguration.DataSourceFields.Fields = new List<Field>();

                        foreach (var schemaItems in lstSchemaTableFields)
                        {
                            Field objField = new Field();
                            objField.FieldName = schemaItems.ColumnName;
                            objField.IsPrimaryKey = schemaItems.IsPrimary;
                            objField.IsIdentity = schemaItems.IsIdentity;

                            //if (objField.Validations == null)
                            //    objField.Validations = new List<Validation>();
                            //Validation objValidation = new Validation();
                            //objField.Validations.Add(objValidation);

                            objFormsMasterConfiguration.DataSourceFields.Fields.Add(objField);

                            ScreensField objScreensField = new ScreensField();
                            objScreensField.FieldName = objField.FieldName;

                            objScreensField.FieldName = objField.FieldName;

                            if (objFormsMasterConfiguration.listScreens == null)
                                objFormsMasterConfiguration.listScreens = new ListScreens();

                            if (objFormsMasterConfiguration.listScreens.Fields == null)
                                objFormsMasterConfiguration.listScreens.Fields = new List<ScreensField>();

                            objFormsMasterConfiguration.listScreens.Fields.Add(objScreensField);

                            if (objFormsMasterConfiguration.viewScreen == null)
                                objFormsMasterConfiguration.viewScreen = new ViewScreen();

                            if (objFormsMasterConfiguration.viewScreen.Fields == null)
                                objFormsMasterConfiguration.viewScreen.Fields = new List<ScreensField>();

                            objFormsMasterConfiguration.viewScreen.Fields.Add(objScreensField);

                            if (objFormsMasterConfiguration.addScreens == null)
                                objFormsMasterConfiguration.addScreens = new AddScreens();

                            if (objFormsMasterConfiguration.addScreens.Fields == null)
                                objFormsMasterConfiguration.addScreens.Fields = new List<ScreensField>();

                            objFormsMasterConfiguration.addScreens.Fields.Add(objScreensField);

                            if (objFormsMasterConfiguration.editScreens == null)
                                objFormsMasterConfiguration.editScreens = new EditScreens();

                            if (objFormsMasterConfiguration.editScreens.Fields == null)
                                objFormsMasterConfiguration.editScreens.Fields = new List<ScreensField>();

                            objFormsMasterConfiguration.editScreens.Fields.Add(objScreensField);

                            if (objFormsMasterConfiguration.SearchScreens == null)
                                objFormsMasterConfiguration.SearchScreens = new SearchScreens();

                            if (objFormsMasterConfiguration.SearchScreens.Fields == null)
                                objFormsMasterConfiguration.SearchScreens.Fields = new List<ScreensField>();

                            objFormsMasterConfiguration.SearchScreens.Fields.Add(objScreensField);
                        }

                        objResponseResult = new ResponseResult()
                        {
                            Data = objFormsMasterConfiguration,
                            Message = ResponseMessage.NoRecordFound,
                            ResponseCode = ResponseCode.NoRecordFound,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.NoRecordFound
                            }
                        };
                        return objResponseResult;

                    }
                }
                else
                {
                    // Schema Table Not Exist Case
                    objResponseResult = new ResponseResult()
                    {
                        Data = objFormsMasterConfiguration,
                        Message = ResponseMessage.SchemaTableNotExist,
                        ResponseCode = ResponseCode.SchemaTableNotExist,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.SchemaTableNotExist,
                        }
                    };
                    return objResponseResult;
                }
            }
            return objResponseResult;
        }
        public async Task<List<ValidationTypes>> GetAllValidationTypesOptions()
        {
            return await this.context.ValidationTypes.OrderBy(x => x.ValidationTypeId).ToListAsync();

            //List<ValidationTypes> lstValidationTypes = new List<ValidationTypes>();
            //ValidationTypes objValidationTypes = null;
            //var queryDataSourceFields = @"SELECT * FROM " + accountSchema + @".""ValidationTypes"" ORDER BY ""ValidationTypeId"" ASC";
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandText = queryDataSourceFields;
            //    context.Database.OpenConnection();
            //    using (var reader_DataSourceFields = await command.ExecuteReaderAsync())
            //    {
            //        while (reader_DataSourceFields.Read())
            //        {
            //            objValidationTypes = new ValidationTypes();
            //            objValidationTypes.ValidationTypeId = Convert.ToInt32(reader_DataSourceFields["ValidationTypeId"]);
            //            objValidationTypes.ValidationType = reader_DataSourceFields["ValidationType"].ToString();
            //            objValidationTypes.ValidationRule = reader_DataSourceFields["ValidationRule"].ToString();
            //            objValidationTypes.ErrorMessage = reader_DataSourceFields["ErrorMessage"].ToString();
            //            lstValidationTypes.Add(objValidationTypes);
            //        }

            //    }
            //}
            //return lstValidationTypes;
        }
        public List<SchemaTableFields> CheckTableSchema(string schemaName, string dataSourceName)
        {
            List<SchemaTableFields> lstSchemaTableFields = new List<SchemaTableFields>();
            try
            {
                // need to create function in database which will return table schema 
                var queryDataSourceFields = @"select * from " + accountSchema + ".fn_gettableschemadata('" + schemaName + "','" + dataSourceName + "')";
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = queryDataSourceFields;
                    context.Database.OpenConnection();

                    using (var reader_DataSourceFields = command.ExecuteReader())
                    {
                        while (reader_DataSourceFields.Read())
                        {
                            int columnMinSize = 0;
                            int columnMaxSize = 0;
                            SchemaTableFields objSchemaTableFields = new SchemaTableFields();

                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("columnname")))
                                objSchemaTableFields.ColumnName = Convert.ToString(reader_DataSourceFields["columnname"]);

                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("datatypename")))
                                objSchemaTableFields.DataTypeName = Convert.ToString(reader_DataSourceFields["datatypename"]);

                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("constrainttype")))
                            {
                                objSchemaTableFields.ConstraintType = Convert.ToString(reader_DataSourceFields["constrainttype"]);
                                if (!string.IsNullOrWhiteSpace(objSchemaTableFields.ConstraintType))
                                {
                                    if (objSchemaTableFields.ConstraintType.Contains("PRIMARY KEY"))
                                        objSchemaTableFields.IsPrimary = true;

                                    if (objSchemaTableFields.ConstraintType.Contains("FOREIGN KEY"))
                                        objSchemaTableFields.IsForeginKey = true;

                                    if (objSchemaTableFields.ConstraintType.Contains("IDENTITY"))
                                        objSchemaTableFields.IsIdentity = true;
                                }
                            }

                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("isnullable")))
                                objSchemaTableFields.IsNull = Convert.ToBoolean(reader_DataSourceFields["isnullable"]);

                            int.TryParse(Convert.ToString(reader_DataSourceFields[4]), out columnMaxSize);
                            int.TryParse(Convert.ToString(reader_DataSourceFields[5]), out columnMinSize);

                            objSchemaTableFields.ColumnMinSize = columnMaxSize;
                            objSchemaTableFields.ColumnMaxSize = columnMinSize;

                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("foreign_table_name")))
                                objSchemaTableFields.ForeignTableName = Convert.ToString(reader_DataSourceFields["foreign_table_name"]);

                            if (!reader_DataSourceFields.IsDBNull(reader_DataSourceFields.GetOrdinal("foreign_column_name")))
                                objSchemaTableFields.ForeignColumnName = Convert.ToString(reader_DataSourceFields["foreign_column_name"]);


                            lstSchemaTableFields.Add(objSchemaTableFields);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSchemaTableFields;
        }

        public async Task<List<DataSources>> GetAllDataSources()
        {
            return await this.context.DataSources.OrderBy(x => x.DataSourceId).ToListAsync();
            //List<DataSources> lstDataSources = new List<DataSources>();
            //DataSources objDataSources = null;
            //var queryDataSourceFields = @"SELECT * FROM " + accountSchema + @".""DataSources"" ORDER BY ""DataSourceId"" ASC";
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandText = queryDataSourceFields;
            //    context.Database.OpenConnection();
            //    using (var reader_DataSourceFields = await command.ExecuteReaderAsync())
            //    {
            //        while (reader_DataSourceFields.Read())
            //        {
            //            objDataSources = new DataSources();
            //            objDataSources.DataSourceId = Convert.ToInt16(reader_DataSourceFields["DataSourceId"]);
            //            objDataSources.DataSourceName = reader_DataSourceFields["DataSourceName"].ToString();
            //            objDataSources.Description = reader_DataSourceFields["Description"].ToString();
            //            lstDataSources.Add(objDataSources);
            //        }

            //    }
            //}
            //return lstDataSources;
        }

        public async Task<List<ControlTypes>> GetAllControlTypes()
        {
            List<ControlTypes> lstControlTypes = new List<ControlTypes>();
            ControlTypes objControlTypes = null;
            var queryDataSourceFields = @"SELECT * FROM " + accountSchema + @".""ControlType"" ORDER BY ""ControlTypeId"" ASC";
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = queryDataSourceFields;
                context.Database.OpenConnection();
                using (var reader_DataSourceFields = await command.ExecuteReaderAsync())
                {
                    while (reader_DataSourceFields.Read())
                    {
                        objControlTypes = new ControlTypes();
                        objControlTypes.ControlTypeId = Convert.ToInt32(reader_DataSourceFields["ControlTypeId"]);
                        objControlTypes.ControlType = reader_DataSourceFields["ControlType"].ToString();
                        lstControlTypes.Add(objControlTypes);
                    }

                }
            }
            return lstControlTypes;
        }

        public async Task<ResponseResult> GetListDataSourceConfigurationData(int dataSourceID)
        {
            ResponseResult objResponseResult = new ResponseResult();
            string dataSourceName = string.Empty;
            var dataSourceTableQuery = @"SELECT * FROM " + accountSchema + @".""DataSources"" WHERE ""DataSourceId""" + "=" + dataSourceID + ";";
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = dataSourceTableQuery;
                context.Database.OpenConnection();
                using (var reader_DataSource = await command.ExecuteReaderAsync())
                {
                    while (reader_DataSource.Read())
                    {
                        dataSourceName = Convert.ToString(reader_DataSource["DataSourceName"]);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(dataSourceName))
            {
                List<SchemaReferenceTableFieldModel> lstSchemaReferenceTableFieldModel = CheckReferenceKeyTableSchema(accountSchema, dataSourceName);
                if (lstSchemaReferenceTableFieldModel != null && lstSchemaReferenceTableFieldModel.Count > 0)
                {
                    foreach (var items in lstSchemaReferenceTableFieldModel)
                    {
                        if (items.ReferenceTable != null && items.ReferenceCol != null)
                        {
                            if (items.lstSchemaTableFieldsData == null)
                                items.lstSchemaTableFieldsData = new List<SchemaTableFields>();

                            if (!string.IsNullOrWhiteSpace(items.TableName))
                                items.lstSchemaTableFieldsData = CheckTableSchema(items.TableSchema, items.ReferenceTable);
                        }
                    }
                    objResponseResult = new ResponseResult()
                    {
                        Data = lstSchemaReferenceTableFieldModel,
                        Message = ResponseMessage.RecordFetched,
                        ResponseCode = ResponseCode.RecordFetched
                    };
                    return objResponseResult;
                }
                else
                {
                    // Schema Table Not Exist Case
                    objResponseResult = new ResponseResult()
                    {
                        Message = ResponseMessage.SchemaReferenceTableNotExist,
                        ResponseCode = ResponseCode.SchemaReferenceTableNotExist,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.SchemaReferenceTableNotExist,
                        }
                    };
                    return objResponseResult;
                }
            }


            return objResponseResult;
        }
        public List<SchemaReferenceTableFieldModel> CheckReferenceKeyTableSchema(string schemaName, string dataSourceName)
        {
            List<SchemaReferenceTableFieldModel> lstSchemaReferenceTableFieldModel = new List<SchemaReferenceTableFieldModel>();
            try
            {
                // need to create function in database which will return table schema 
                var queryDataSourceFields = @"select * from " + accountSchema + ".fn_getreferencekeytableschema('" + schemaName + "','" + dataSourceName + "')";
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    context.Database.OpenConnection();

                    command.CommandText = queryDataSourceFields;
                    using (var reader_DataSourceFields = command.ExecuteReader())
                    {
                        while (reader_DataSourceFields.Read())
                        {
                            SchemaReferenceTableFieldModel objSchemaReferenceTableFieldModel = new SchemaReferenceTableFieldModel();

                            if (!reader_DataSourceFields.IsDBNull(0))
                                objSchemaReferenceTableFieldModel.TableSchema = reader_DataSourceFields.GetString(0);
                            if (!reader_DataSourceFields.IsDBNull(1))
                                objSchemaReferenceTableFieldModel.TableName = reader_DataSourceFields.GetString(1);
                            if (!reader_DataSourceFields.IsDBNull(2))
                                objSchemaReferenceTableFieldModel.ColumnName = reader_DataSourceFields.GetString(2);
                            if (!reader_DataSourceFields.IsDBNull(3))
                                objSchemaReferenceTableFieldModel.ReferenceTable = reader_DataSourceFields.GetString(3);
                            if (!reader_DataSourceFields.IsDBNull(4))
                                objSchemaReferenceTableFieldModel.ReferenceCol = reader_DataSourceFields.GetString(4);

                            if (objSchemaReferenceTableFieldModel.ReferenceTable != null && objSchemaReferenceTableFieldModel.ReferenceCol != null)
                                lstSchemaReferenceTableFieldModel.Add(objSchemaReferenceTableFieldModel);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return lstSchemaReferenceTableFieldModel;
        }
        private string GetDataSourceDisplayType(string displayType, string listValueFieldName, string listTextFieldName, string fieldName)
        {
            var returnType = string.Empty;
            if (!string.IsNullOrEmpty(displayType) && !string.IsNullOrEmpty(listValueFieldName) && !string.IsNullOrEmpty(listTextFieldName))
            {
                switch (displayType)
                {
                    case "1":
                        returnType = listValueFieldName;
                        break;
                    case "2":
                        returnType = listTextFieldName;
                        break;
                    case "3":
                        returnType = listTextFieldName;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                returnType = fieldName;
            }
            return returnType;
        }


        public async Task<List<string>> GetAllDataSourceTables()
        {
            var result = new List<string>(); 

            var queryDataSourceFields = $"SELECT TABLE_NAME::text FROM information_schema.tables WHERE table_schema = quote_ident('{accountSchema}') AND table_type = 'BASE TABLE' ";

        
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = queryDataSourceFields;
                context.Database.OpenConnection();
                using (var readerResult = await command.ExecuteReaderAsync())
                {
                    while (readerResult.Read())
                    {
                        result.Add(readerResult.GetString("TABLE_NAME"));
                    }
                }
            }
            return result;
        }


        public async Task<bool> SaveAllDataSourceTables(string[] tableList, long accountId)
        {
            var rtnValue = false;
            string sqlQuery = @"CALL " + accountSchema + ".sp_insert_datasource('{" + string.Join(",", tableList) + "}', " + accountId + ", '"+ accountSchema + "')";

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                command.CommandText = sqlQuery;
                await command.ExecuteNonQueryAsync();
                rtnValue = true;
            }
            return rtnValue;
        }
    }
}
