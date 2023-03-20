using System.ComponentModel.DataAnnotations;

namespace Sample.Admin.Model
{
    public class ModulesModel
    {
        public long ModuleId { get; set; }
        public long? ParentModule { get; set; }
        public bool IsNavigationItem { get; set; }
        public int? Service { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsVisible { get; set; }
        public short Status { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
