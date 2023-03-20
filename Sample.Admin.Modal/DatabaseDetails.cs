namespace Sample.Admin.Model
{
    public class DatabaseDetails
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public int PortNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserManagementServiceSchema { get; set; }
        public string ReportServiceSchema { get; set; }
    }
}
